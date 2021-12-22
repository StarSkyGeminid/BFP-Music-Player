using BFPMusicPlayer.Backend;
using BFPMusicPlayer.Backend.Page;
using BFPMusicPlayer.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BFPMusicPlayer.ViewModel.Page
{
    public class MusicViewModel : ViewModelBase
    {
        public readonly int MusicOwnerCode = 1;

        private readonly MusicControl musicControl = new MusicControl();

        private MainControl mainControl;
        private MusicModel selectedValue;

        private Action delegateAction1;

        private ImageSource albumImage;

        public MusicModel CurrentPlay { get; private set; }

        public void SetMusicModel(MusicModel musicModel, int trackOwner)
        {
            CurrentPlay = musicModel;

            if (musicModel != null)
            {
                UpdateAlbumInfo();

                UpdateSelectedData(musicModel, trackOwner);
            }

            UpdateAlbumImage();
        }

        public void GetRequestTrack(int type, int loopStatus)
        {
            if (CurrentPlay != null)
            {
                MusicModel nextPlay = musicControl.GetRequestTrack(MusicList, CurrentPlay, type, loopStatus);
                if (CurrentPlay != nextPlay)
                {
                    CurrentPlay = nextPlay;
                    NewTrack();
                }
            }
        }

        public ObservableCollection<MusicModel> MusicList { get; private set; }

        public ObservableCollection<HistoryModel> HistoryPlayList { get; private set; }

        public void SetSearcData(ObservableCollection<MusicModel> data)
        {
            MusicList = data;
            OnPropertyChanged(nameof(MusicList));
        }

        public string SelectedSetLocation { get; private set; }

        public MusicModel SelectedData
        {
            get => CurrentPlay;
            set
            {
                CurrentPlay = value;
                OnPropertyChanged(nameof(SelectedData));
                if (CurrentPlay != null)
                {
                    NewTrack();
                }
            }
        }

        public MusicModel SelectionValue
        {
            get => selectedValue;
            set
            {
                selectedValue = value;
                OnPropertyChanged(nameof(SelectionValue));
            }
        }

        public void SetAction(Action action1)
        {
            delegateAction1 = action1;
        }

        public string Title
        {
            get => CurrentPlay.Title;
        }

        public string Album
        {
            get => CurrentPlay.Album;
        }

        public ImageSource AlbumImage
        {
            get => albumImage;
            private set
            {
                albumImage = value;
                OnPropertyChanged(nameof(AlbumImage));
            }
        }

        public MusicViewModel()
        {
            ObjectInit();

            SelectedSetLocation = "0";

            SetDefaultView();
        }

        private void SetDefaultView()
        {
            _ = Task.Run(() =>
            {

                MusicList = mainControl.GetMusicList();

                HistoryPlayList = mainControl.GetHistory();
            });


            CurrentPlay.Title = "Unknown";
            CurrentPlay.Album = "Unknown";

            SetDefaultAlbumImage();

            UpdateAlbumInfo();
        }

        private void ObjectInit()
        {
            mainControl = new MainControl();
            CurrentPlay = new MusicModel();
            selectedValue = new MusicModel();

            MusicList = new ObservableCollection<MusicModel>();
            HistoryPlayList = new ObservableCollection<HistoryModel>();
        }

        private void NewTrack()
        {
            if (MusicList.Contains(CurrentPlay))
            {
                delegateAction1();
                SelectionValue = CurrentPlay;
            }

            UpdateAlbumImage();

            UpdateAlbumInfo();
        }

        private void UpdateAlbumImage()
        {
            Task.Run(() =>
            {
                if (CurrentPlay.UID != null)
                {
                    BitmapImage bitmap = new BitmapImage();

                    if (musicControl.GetAlbumImage(CurrentPlay.UID, bitmap))
                    {
                        bitmap.Freeze();
                        _ = Dispatcher.CurrentDispatcher.Invoke(() => AlbumImage = bitmap);
                        bitmap = null;
                    }
                    else
                    {
                        SetDefaultAlbumImage();
                    }
                }
                else
                {
                    SetDefaultAlbumImage();
                }
            });
        }

        private void SetDefaultAlbumImage()
        {
            AlbumImage = musicControl.GetDefaultAlbumImage();
        }

        private void UpdateAlbumInfo()
        {
            if (CurrentPlay.UID != null)
            {
                HistoryPlayList = mainControl.GetHistory();
                OnPropertyChanged(nameof(HistoryPlayList));

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Album));
                OnPropertyChanged(nameof(SelectedData));

            }
        }

        private void UpdateSelectedData(MusicModel musicModel, int trackOwner)
        {
            int count = 0;
            if (trackOwner == MusicOwnerCode)
            {
                foreach (MusicModel data in MusicList)
                {
                    if (data.UID == musicModel.UID)
                    {

                        CurrentPlay = MusicList[count];
                        OnPropertyChanged(nameof(SelectedData));
                        break;
                    }
                    count++;
                }
            }
        }
    }
}

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
    public class DashboardViewModel : ViewModelBase
    {
        public readonly int DashboardOwnerCode = 0;

        private readonly MusicControl musicControl = new MusicControl();

        private MainControl mainControl;

        private MusicModel selectedValue;

        private Action delegateAction1;

        private ImageSource albumImage;

        private bool isPlaySong = false;

        public MusicModel CurrentPlay { get; private set; }

        public void SetMusicModel(MusicModel musicModel, int trackOwner)
        {
            CurrentPlay = musicModel;

            if (musicModel != null)
            {
                UpdateView();
                UpdateSelectedData(musicModel, trackOwner);
            }
            UpdateAlbumInfo();
        }

        public void SetSearcData(ObservableCollection<MusicModel> data)
        {
            PlayList = data;
            OnPropertyChanged(nameof(PlayList));
        }

        public ObservableCollection<MusicModel> playList;
        public ObservableCollection<HistoryModel> historyList;
        public ObservableCollection<MusicModel> PlayList
        {
            get
            {
                return playList;
            }
            private set
            {
                playList = value;
                OnPropertyChanged(nameof(PlayList));
            }
        }

        public ObservableCollection<HistoryModel> HistoryPlayList
        {
            get
            {
                return historyList;
            }
            private set
            {
                historyList = value;
                OnPropertyChanged(nameof(HistoryPlayList));
            }
        }

        public void GetRequestTrack(int type, int loopStatus)
        {
            if (CurrentPlay != null)
            {
                MusicModel nextPlay = musicControl.GetRequestTrack(PlayList, CurrentPlay, type, loopStatus);
                if (CurrentPlay != nextPlay)
                {
                    CurrentPlay = nextPlay;
                    NewTrack();
                }
            }
        }

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
            get
            {
                if (CurrentPlay.Title != null)
                {
                    return CurrentPlay.Title;
                }
                return string.Empty;
            }
        }

        public string Album
        {
            get
            {
                if (CurrentPlay.Album != null)
                {
                    return CurrentPlay.Album;
                }
                return string.Empty;
            }
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

        public bool IsPlaySong
        {
            get => isPlaySong;
            set
            {
                isPlaySong = value;
                OnPropertyChanged(nameof(IsPlaySong));
            }
        }

        public DashboardViewModel()
        {
            ObjectInit();

            SetDefaultView();
        }

        private void SetDefaultView()
        {
            _ = Task.Run(() =>
            {

                PlayList = mainControl.GetPlaylist();

                HistoryPlayList = mainControl.GetHistory();
            });


            CurrentPlay.Title = "Unknown";
            CurrentPlay.Album = "Unknown";

            SetDefaultAlbumImage();

            UpdateView();
        }

        private void ObjectInit()
        {
            mainControl = new MainControl();
            CurrentPlay = new MusicModel();
            selectedValue = new MusicModel();

            PlayList = new ObservableCollection<MusicModel>();
            HistoryPlayList = new ObservableCollection<HistoryModel>();
        }

        private void NewTrack()
        {
            delegateAction1();

            IsPlaySong = true;

            SelectionValue = CurrentPlay;

            HistoryPlayList = mainControl.GetHistory();

            UpdateAlbumInfo();

            UpdateView();
        }

        private void UpdateAlbumInfo()
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

        private void UpdateView()
        {
            if (CurrentPlay != null)
            {
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Album));
            }
        }

        private void UpdateSelectedData(MusicModel musicModel, int trackOwner)
        {
            int count = 0;
            if (trackOwner == DashboardOwnerCode)
            {
                foreach (MusicModel data in PlayList)
                {
                    if (data.UID == musicModel.UID)
                    {

                        CurrentPlay = PlayList[count];
                        OnPropertyChanged(nameof(SelectedData));
                        break;
                    }
                    count++;
                }
            }
        }
    }
}

using BFPMusicPlayer.Model;
using BFPMusicPlayer.Theme;
using BFPMusicPlayer.ViewModel.Page;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BFPMusicPlayer.View
{
    public partial class Music : Page
    {
        private MusicViewModel musicViewModel = new MusicViewModel();

        public delegate void SendMusicModel(MusicModel musicModel, int requester);
        public static event SendMusicModel OnSetPlay;

        public delegate void SendGetCurrentTrack(int requester);
        public static event SendGetCurrentTrack OnGetCurrentTrack;

        public Music()
        {
            InitializeComponent();
            DataContext = musicViewModel;
            musicViewModel.SetAction(SendMusicModelFunc);
            PlayControll.OnGetCurrentTrack += CopyCurrentTrack;

            SearchBox.onSendData += GetSearchData;
            PlayControll.OnSkipTrack += GetTrackRequest;

            _ = Task.Factory.StartNew(() =>
              {
                  System.Threading.Thread.Sleep(10);
                  GetCurrentTrack();
              });
        }

        private void GetTrackRequest(int type, int statusLoop, int owner)
        {
            if (owner == musicViewModel.MusicOwnerCode)
            {
                musicViewModel.GetRequestTrack(type, statusLoop);
            }
            else
            {
                OnGetCurrentTrack(1);
            }
        }

        private void GetSearchData(ObservableCollection<MusicModel> data)
        {
            musicViewModel.SetSearcData(data);
        }

        private void CopyCurrentTrack(MusicModel musicModel, int requester, int trackOnwer)
        {
            if (requester == musicViewModel.MusicOwnerCode)
            {
                musicViewModel.SetMusicModel(musicModel, trackOnwer);
            }
        }

        public void SendMusicModelFunc()
        {
            OnSetPlay(musicViewModel.CurrentPlay, musicViewModel.MusicOwnerCode);
        }

        public void GetCurrentTrack()
        {
            OnGetCurrentTrack(musicViewModel.MusicOwnerCode);
        }
    }
}

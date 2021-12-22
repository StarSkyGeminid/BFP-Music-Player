using BFPMusicPlayer.Model;
using BFPMusicPlayer.Theme;
using BFPMusicPlayer.ViewModel.Page;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BFPMusicPlayer.View
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private DashboardViewModel dashboardViewModel;

        public delegate void SendMusicModel(MusicModel musicModel, int i);
        public static event SendMusicModel OnMusicModelSend;

        public delegate void SendGetCurrentTrack(int requester);
        public static event SendGetCurrentTrack OnGetCurrentTrack;

        public delegate void SetCurrentView(int i);
        public static event SetCurrentView onSetCurrentView;

        public Dashboard()
        {
            InitializeComponent();
            dashboardViewModel = new DashboardViewModel();

            dashboardViewModel.SetAction(SendMusicModelFunc);

            SearchBox.onSendData += GetSearchData;

            PlayControll.OnGetCurrentTrack += CopyCurrentTrack;
            PlayControll.OnSkipTrack += GetTrackRequest;

            DataContext = dashboardViewModel;

            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(10);
                SetType();
                GetCurrentTrack();
            });
        }

        ~Dashboard()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void GetTrackRequest(int i, int statusLoop, int owner)
        {
            if (owner == dashboardViewModel.DashboardOwnerCode)
            {
                dashboardViewModel.GetRequestTrack(i, statusLoop);
            }
            else
            {
                OnGetCurrentTrack(dashboardViewModel.DashboardOwnerCode);
            }
        }

        private void GetSearchData(ObservableCollection<MusicModel> data)
        {
            dashboardViewModel.SetSearcData(data);
        }

        private void CopyCurrentTrack(MusicModel musicModel, int requester, int trackOwner)
        {
            if (requester == dashboardViewModel.DashboardOwnerCode)
            {
                dashboardViewModel.SetMusicModel(musicModel, trackOwner);
            }
        }

        private void SetType()
        {
            onSetCurrentView(dashboardViewModel.DashboardOwnerCode);
        }

        public void SendMusicModelFunc()
        {
            OnMusicModelSend(dashboardViewModel.CurrentPlay, dashboardViewModel.DashboardOwnerCode);
        }

        public void GetCurrentTrack()
        {
            OnGetCurrentTrack(dashboardViewModel.DashboardOwnerCode);
        }

    }
}

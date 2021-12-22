using BFPMusicPlayer.Model;
using BFPMusicPlayer.View;
using BFPMusicPlayer.ViewModel.UserControl;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BFPMusicPlayer.Theme
{
    /// <summary>
    /// Interaction logic for PlayControll.xaml
    /// </summary>
    public partial class PlayControll : UserControl
    {
        private PlayBackViewModel playBackViewModel;
        public Delegate del;
        public MusicModel musicModel;

        public delegate void SendSkipTrack(int i, int k, int j);
        public static event SendSkipTrack OnSkipTrack;

        public delegate void SendGetCurrentTrack(MusicModel musicModel, int requester, int trackOwner);
        public static event SendGetCurrentTrack OnGetCurrentTrack;

        public PlayControll()
        {
            playBackViewModel = new PlayBackViewModel();
            InitializeComponent();
            DataContext = playBackViewModel;
            Music.OnSetPlay += GetMusicModel;
            Music.OnGetCurrentTrack += GetCurrentTrack;
            Dashboard.OnMusicModelSend += GetMusicModel;
            Dashboard.OnGetCurrentTrack += GetCurrentTrack;

            playBackViewModel.SetAction(BackTrack, NextTrack);
        }

        private void GetMusicModel(MusicModel musicModel, int owner)
        {
            playBackViewModel.SetCurrentTrackOwner(owner);
            playBackViewModel.SetCurrentPlay(musicModel);
        }

        private void ThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            playBackViewModel.ThumbDragStarted = !playBackViewModel.ThumbDragStarted;
        }

        private void ThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            playBackViewModel.ThumbDragCompleted = !playBackViewModel.ThumbDragStarted;
        }

        private void ThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            playBackViewModel.UpdateCurrentTime();
        }

        private void BackTrack()
        {
            OnSkipTrack(1, (int)playBackViewModel.CurrentLoopType, playBackViewModel.GetCurrentTrackOwner);
        }

        private void NextTrack()
        {
            OnSkipTrack(0, (int)playBackViewModel.CurrentLoopType, playBackViewModel.GetCurrentTrackOwner);
        }

        private void GetCurrentTrack(int requester)
        {
            OnGetCurrentTrack(playBackViewModel.GetCurrentPlay, requester, playBackViewModel.GetCurrentTrackOwner);
        }
    }
}

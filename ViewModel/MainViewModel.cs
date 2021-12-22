using BFPMusicPlayer.Model;
using System;

namespace BFPMusicPlayer.ViewModel
{
    public class MainViewModel
    {
        private MusicModel musicModel;
        private Action delegateAction1;

        public int GetCurrentRequester { get; private set; }
        public void SetCurrentRequester(int i) { GetCurrentRequester = i; }

        public void SetAction(Action action1)
        {
            delegateAction1 = action1;
        }

        public void SetMusicModel(MusicModel musicModel)
        {
            this.musicModel = musicModel;
            delegateAction1();
        }

        public MusicModel GetMusicModel()
        {
            return musicModel;
        }

        public MainViewModel()
        {
            musicModel = new MusicModel();
        }
    }
}

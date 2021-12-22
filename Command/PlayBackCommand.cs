using BFPMusicPlayer.ViewModel.UserControl;
using System;
using System.Windows.Input;

namespace BFPMusicPlayer.Command
{
    public class PlayBackCommand : ICommand
    {
        private readonly PlayBackViewModel viewModel;

        public PlayBackCommand(PlayBackViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void Execute(object o)
        {
            if (viewModel.Playing)
            {
                viewModel.OnPlay();
            }
            else
            {
                viewModel.OnPause();
            }
        }

        public bool CanExecute(object o)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}

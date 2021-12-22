using BFPMusicPlayer.ViewModel.UserControl;
using System;
using System.Windows.Input;

namespace BFPMusicPlayer.Command
{
    public class AddMusicCommand : ICommand
    {
        private readonly PlayBackViewModel viewModel;

        public AddMusicCommand(PlayBackViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void Execute(object o)
        {
            viewModel.Add();
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

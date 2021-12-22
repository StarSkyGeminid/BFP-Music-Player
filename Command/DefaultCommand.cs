using System;
using System.Windows.Input;

namespace BFPMusicPlayer.Command
{
    public class DefaultCommand : ICommand
    {
        private readonly Action action;
        public DefaultCommand(Action action)
        {
            this.action = action;
        }

        public void Execute(object o)
        {
            action();
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

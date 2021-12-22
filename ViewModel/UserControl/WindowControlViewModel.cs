using BFPMusicPlayer.Command;
using System.Windows;

namespace BFPMusicPlayer.ViewModel.UserControl
{
    public class WindowControlViewModel
    {
        public DefaultCommand ExitCommand { get; set; }
        public DefaultCommand MinimizeCommand { get; set; }

        public WindowControlViewModel()
        {
            ExitCommand = new DefaultCommand(ExitClick);
            MinimizeCommand = new DefaultCommand(MinimizeClick);
        }

        private void ExitClick()
        {
            Application.Current.Shutdown();
        }

        private void MinimizeClick()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).WindowState = WindowState.Minimized;
                }
            }
        }

    }
}

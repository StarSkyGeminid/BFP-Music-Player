using BFPMusicPlayer.ViewModel.UserControl;
using System.Windows;
using System.Windows.Controls;

namespace BFPMusicPlayer.Theme
{
    /// <summary>
    /// Interaction logic for WindowControl.xaml
    /// </summary>
    public partial class WindowControl : UserControl
    {
        public WindowControl()
        {
            InitializeComponent();
            Window window = Window.GetWindow(this);
            DataContext = new WindowControlViewModel();
        }
    }
}

using BFPMusicPlayer.View;
using System.Windows;
using System.Windows.Input;

namespace BFPMusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowFrame.Navigate(new Dashboard());


            MouseDown += delegate (object sender, MouseButtonEventArgs e)
            { if (e.ChangedButton == MouseButton.Left) DragMove(); };
        }
    }
}

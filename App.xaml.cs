using System.Runtime;
using System.Windows;

namespace BFPMusicPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private App()
        {
            GCLatencyMode = GCLatencyMode.Interactive;
        }

        public GCLatencyMode GCLatencyMode { get; private set; }
    }
}

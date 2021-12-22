using BFPMusicPlayer.Model;
using BFPMusicPlayer.View;
using BFPMusicPlayer.ViewModel.UserControl;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace BFPMusicPlayer.Theme
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public delegate void SendMusicModel(ObservableCollection<MusicModel> data);
        public static event SendMusicModel onSendData;
        private readonly SearchBoxViewModel searchBoxViewModel = new SearchBoxViewModel();

        public SearchBox()
        {
            InitializeComponent();
            DataContext = searchBoxViewModel;
            searchBoxViewModel.SetAction(SendData);

            Dashboard.onSetCurrentView += SetView1;
        }

        private void SetView1(int i)
        {
            searchBoxViewModel.SetCurrentView(i);
        }

        public void SendData()
        {
            onSendData(searchBoxViewModel.Data);
        }

        //public string Title
        //{
        //    get { return (string)GetValue(TitleProperty); }
        //    set { SetValue(TitleProperty, value); }
        //}

        //public static readonly DependencyProperty TitleProperty =
        //    DependencyProperty.Register("Title", typeof(string), typeof(AlbumInfo), new UIPropertyMetadata(null));
    }
}

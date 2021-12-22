using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BFPMusicPlayer.Theme
{
    /// <summary>
    /// Interaction logic for AlbumInfo.xaml
    /// </summary>
    public partial class AlbumInfo : UserControl
    {
        public AlbumInfo()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Album
        {
            get { return (string)GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }

        public ImageSource AlbumImage
        {
            get { return (ImageSource)GetValue(AlbumImageProperty); }
            set { SetValue(AlbumImageProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(AlbumInfo), new UIPropertyMetadata(null));

        public static readonly DependencyProperty AlbumProperty =
            DependencyProperty.Register("Album", typeof(string), typeof(AlbumInfo), new UIPropertyMetadata(null));

        public static readonly DependencyProperty AlbumImageProperty =
            DependencyProperty.Register("AlbumImage", typeof(ImageSource), typeof(AlbumInfo), new UIPropertyMetadata(null));
    }
}

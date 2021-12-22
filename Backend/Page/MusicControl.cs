using BFPMusicPlayer.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BFPMusicPlayer.Backend.Page
{
    public class MusicControl
    {
        private readonly MainControl mainControl;

        public MusicControl()
        {
            mainControl = new MainControl();
        }

        public MusicModel GetRequestTrack(ObservableCollection<MusicModel> PlayList, MusicModel currentPlay, int type, int loopStatus)
        {
            if (currentPlay != null)
            {
                int nextIndex = PlayList.IndexOf(currentPlay);
                nextIndex = type == 0 ? (nextIndex + 1) : (nextIndex - 1);
                if (nextIndex >= 0 && nextIndex < PlayList.Count)
                {
                    currentPlay = PlayList[nextIndex];
                }
                else if (loopStatus == 0 && nextIndex == PlayList.Count)
                {
                    try
                    {
                        currentPlay = PlayList[0];
                    }
                    catch
                    {
                        currentPlay = PlayList[1];
                    }
                }
            }
            return currentPlay;
        }

        public bool GetAlbumImage(string UID, BitmapImage bitmap)
        {
            try
            {
                TagLib.File tagFile = TagLib.File.Create(mainControl.GetPath(UID));

                TagLib.IPicture pic = tagFile.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);

                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ImageSource GetDefaultAlbumImage()
        {
            ImageSource image = new ImageSourceConverter().ConvertFrom("pack://application:,,,/BFPMusicPlayer;component/Assets/Vinyl Disk.png") as ImageSource;
            return image;
        }
    }
}

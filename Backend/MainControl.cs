using BFPMusicPlayer.Model;
using System.Collections.ObjectModel;

namespace BFPMusicPlayer.Backend
{
    public class MainControl
    {
        private ServerBackend serverBackend;

        public MainControl()
        {
            Initialize();
        }

        private void Initialize()
        {
            serverBackend = new ServerBackend();
        }

        public ObservableCollection<HistoryModel> GetHistory()
        {
            ObservableCollection<HistoryModel> history = new ObservableCollection<HistoryModel>();

            int count = 0;
            foreach (HistoryModel data in serverBackend.GetHistory())
            {
                count++;
                data.Number = count;
                history.Add(data);

            }
            return history;
        }

        public ObservableCollection<MusicModel> GetPlaylist()
        {
            ObservableCollection<MusicModel> music = new ObservableCollection<MusicModel>();

            int count = 0;
            foreach (MusicModel data in serverBackend.GetPlaylist())
            {
                if (serverBackend.CheckFileAndDelete(data.UID))
                {
                    count++;
                    data.Number = count;
                    music.Add(data);
                }
            }
            return music;
        }

        public ObservableCollection<MusicModel> GetMusicList()
        {
            ObservableCollection<MusicModel> music = new ObservableCollection<MusicModel>();

            int count = 0;
            foreach (MusicModel data in serverBackend.GetMusic())
            {
                if (serverBackend.CheckFileAndDelete(data.UID))
                {
                    count++;
                    data.Number = count;
                    music.Add(data);
                }
            }
            return music;
        }

        public ObservableCollection<MusicModel> SearchMusic(string query, bool inPlayList = false)
        {
            ObservableCollection<MusicModel> playlist = new ObservableCollection<MusicModel>();

            foreach (MusicModel data in serverBackend.SearchMusic(query, inPlayList))
            {
                playlist.Add(data);
            }
            return playlist;
        }

        public void AddHistory(string UID)
        {
            serverBackend.AddHistory(UID);
        }

        public string GetPath(string UID)
        {
            return serverBackend.GetPath(UID);
        }

        public bool CheckUIDInPlaylist(string UID)
        {
            return serverBackend.CheckUIDInPlaylist(UID);
        }

        public void SetToPlaylist(string UID)
        {
            serverBackend.SetToPlaylist(UID);
        }
    }
}

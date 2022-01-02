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

        public ObservableCollection<MusicModel> GetMusicObser(bool inPlaylist)
        {
            ObservableCollection<MusicModel> music = new ObservableCollection<MusicModel>();

            int count = 0;
            foreach (MusicModel data in inPlaylist ? serverBackend.GetPlaylist() : serverBackend.GetMusic())
            {
                if (serverBackend.CheckFileAutoDelete(data.UID))
                {
                    count++;
                    data.Number = count;
                    music.Add(data);
                }
            }
            return music;
        }

        public ObservableCollection<HistoryModel> GetHistory()
        {
            return serverBackend.GetHistory();
        }

        public ObservableCollection<MusicModel> GetPlaylist()
        {

            return GetMusicObser(true);
        }

        public ObservableCollection<MusicModel> GetMusicList()
        {
            return GetMusicObser(false);
        }

        public ObservableCollection<MusicModel> SearchMusic(string query, bool inPlayList = false)
        {
            return serverBackend.SearchMusic(query, inPlayList);
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

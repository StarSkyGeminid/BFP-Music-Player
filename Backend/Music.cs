using BFPMusicPlayer.Model;
using Microsoft.Win32;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BFPMusicPlayer.Backend
{
    public class Music
    {
        private const int _defaultNumberOfCharacters = 9;
        private static readonly string _allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


        public void Add()
        {
            _ = Task.Run(() => NewFile());
        }

        public void Update(string UID)
        {
            _ = Task.Run(() => UpdateFile(UID));
        }

        public void UpdateAllDB()
        {
            _ = Task.Run(() =>
            {
                ServerBackend serverBackend = new ServerBackend();
                foreach (MusicModel data in serverBackend.GetMusic())
                {
                    MusicModel musicModel = GetMusicData(serverBackend.GetPath(data.UID));
                    musicModel.UID = data.UID;
                    serverBackend.UpdateMusic(musicModel);
                }
            });
        }

        private void UpdateFile(string UID)
        {
            ServerBackend serverBackend = new ServerBackend();
            MusicModel musicModel = GetMusicData(serverBackend.GetPath(UID));
            musicModel.UID = UID;

            serverBackend.UpdateMusic(musicModel);
        }

        private void NewFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ServerBackend serverBackend = new ServerBackend();
                if (!serverBackend.CheckFile(GetPath(openFileDialog.FileName), GetFileName(openFileDialog.FileName)))
                {
                    MusicModel musicModel = GetMusicData(openFileDialog.FileName);

                    while (true)
                    {
                        musicModel.UID = GenerateKey(_defaultNumberOfCharacters);
                        if (!serverBackend.CheckUID(musicModel.UID))
                        {
                            break;
                        }
                    }
                    serverBackend.AddMusic(GetPath(openFileDialog.FileName), GetFileName(openFileDialog.FileName), musicModel);
                }
                else
                {
                    _ = MessageBox.Show("File already exist!");
                }
            }
        }

        private string GetFileName(string data)
        {
            string fileName = "";

            MatchCollection mc = Regex.Matches(data, @"[^\\]*$");

            foreach (Match m in mc)
            {
                fileName += m;
            }

            return fileName;
        }

        private string GetPath(string data)
        {
            string location = "";

            MatchCollection mc = Regex.Matches(data, @".*\\");

            foreach (Match m in mc)
            {
                location += m;
            }

            return location;
        }

        private MusicModel GetMusicData(string path)
        {
            TagLib.File tagFile = TagLib.File.Create(path);

            MusicModel musicModel = new MusicModel();
            TimeSpan duration = tagFile.Properties.Duration;

            musicModel.Title = string.IsNullOrEmpty(tagFile.Tag.Title) ? GetFileName(path) : tagFile.Tag.Title;
            musicModel.Album = string.IsNullOrEmpty(tagFile.Tag.Album) ? "Unknown" : tagFile.Tag.Album;
            try
            {
                musicModel.Artist = string.IsNullOrEmpty(tagFile.Tag.AlbumArtists[0]) ? "Unknown" : tagFile.Tag.AlbumArtists[0];
            }
            catch (IndexOutOfRangeException)
            {
                musicModel.Artist = "Unknown";
            }

            musicModel.Time = duration.TotalSeconds.ToString();

            return musicModel;
        }

        private static string GenerateKey(int numberOfCharacters)
        {
            const int from = 0;
            int to = _allowedCharacters.Length;
            Random r = new Random();

            StringBuilder qs = new StringBuilder();
            for (int i = 0; i < numberOfCharacters; i++)
            {
                _ = qs.Append(_allowedCharacters.Substring(r.Next(from, to), 1));
            }
            return qs.ToString();
        }
    }
}

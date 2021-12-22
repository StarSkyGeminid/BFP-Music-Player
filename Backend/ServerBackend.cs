using BFPMusicPlayer.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace BFPMusicPlayer.Backend
{
    public class ServerBackend
    {
        private readonly MySqlConnection connection;

        public ServerBackend()
        {
            string connectionString = "SERVER=localhost;DATABASE=bfpmusicplayer;UID=root;";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        _ = MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        _ = MessageBox.Show("Invalid username/password, please try again");
                        break;
                    default:
                        _ = MessageBox.Show("Cannot connect to the server, make sure the server is active!");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                _ = MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void DeleteMusic(string UID)
        {
            if (OpenConnection())
            {
                string query = "DELETE FROM musicinfo WHERE UID=@Uid";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                _ = cmd.Parameters.AddWithValue("@Uid", UID);
                _ = cmd.ExecuteNonQuery();

                query = "DELETE FROM history WHERE UID=@Uid";
                cmd = new MySqlCommand(query, connection);
                _ = cmd.Parameters.AddWithValue("@Uid", UID);
                _ = cmd.ExecuteNonQuery();

                query = "DELETE FROM file WHERE UID=@Uid";
                cmd = new MySqlCommand(query, connection);
                _ = cmd.Parameters.AddWithValue("@Uid", UID);
                _ = cmd.ExecuteNonQuery();

                _ = CloseConnection();
            }
        }

        public bool CheckFileAndDelete(string UID)
        {
            string query = string.Format("SELECT * FROM file WHERE UID='{0}'", UID);
            string path = "";

            if (connection.State != System.Data.ConnectionState.Open)
            {
                _ = OpenConnection();
            }

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                path = string.Format("{0}/{1}", dataReader["location"].ToString(), dataReader["filename"].ToString());
            }

            dataReader.Close();

            _ = CloseConnection();

            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                DeleteMusic(UID);
            }
            return false;
        }

        public List<HistoryModel> GetHistory()
        {
            string query = "SELECT * FROM history";

            List<HistoryModel> list = new List<HistoryModel>(5);

            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    list.Add(new HistoryModel()
                    {
                        Time = dataReader["waktu"].ToString(),
                        UID = dataReader["UID"].ToString(),
                    });
                }

                dataReader.Close();

                foreach (HistoryModel data in list)
                {
                    string musicInfo = string.Format("SELECT * FROM musicinfo WHERE UID='{0}'", data.UID);
                    cmd = new MySqlCommand(musicInfo, connection);
                    dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        data.Title = dataReader["title"].ToString();
                    }
                    dataReader.Close();
                }
                _ = CloseConnection();
                list.Reverse();
                return list;
            }
            else
            {
                _ = CloseConnection();
                return list;
            }
        }

        public List<MusicModel> GetPlaylist()
        {
            string query = "SELECT * FROM musicinfo WHERE playlist=1";
            List<MusicModel> list = new List<MusicModel>();

            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(int.Parse(dataReader["waktu"].ToString()));
                    string currentTime = int.Parse(dataReader["waktu"].ToString()) >= 3600 ? timeSpan.ToString(@"hh\:mm\:ss") : timeSpan.ToString(@"mm\:ss");

                    list.Add(new MusicModel()
                    {
                        UID = dataReader["UID"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Album = dataReader["album"].ToString(),
                        Artist = dataReader["artist"].ToString(),
                        Time = currentTime,

                    });

                }

                dataReader.Close();

                _ = CloseConnection();

                return list;
            }
            else
            {
                return list;
            }
        }

        public List<MusicModel> GetMusic()
        {
            string query = "SELECT * FROM musicinfo";
            List<MusicModel> list = new List<MusicModel>();

            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(int.Parse(dataReader["waktu"].ToString()));
                    string currentTime = int.Parse(dataReader["waktu"].ToString()) >= 3600 ? timeSpan.ToString(@"hh\:mm\:ss") : timeSpan.ToString(@"mm\:ss");

                    list.Add(new MusicModel()
                    {
                        UID = dataReader["UID"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Album = dataReader["album"].ToString(),
                        Artist = dataReader["artist"].ToString(),
                        Time = currentTime,

                    });
                }

                dataReader.Close();

                _ = CloseConnection();

                return list;
            }
            else
            {
                return list;
            }
        }

        public List<MusicModel> SearchMusic(string data, bool inPlaylist = false)
        {
            List<MusicModel> list = new List<MusicModel>();

            if (OpenConnection())
            {
                string query = inPlaylist
                    ? "SELECT * FROM musicinfo WHERE title LIKE '%" + data + "%' AND playlist=1 OR artist LIKE '%" + data + "%' AND playlist=1 OR album LIKE '%" + data + "%' AND playlist=1"
                    : "SELECT * FROM musicinfo WHERE title LIKE '%" + data + "%' OR artist LIKE '%" + data + "%' OR album LIKE '%" + data + "%'";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                int count = 0;
                while (dataReader.Read())
                {
                    count++;
                    TimeSpan timeSpan = TimeSpan.FromSeconds(int.Parse(dataReader["waktu"].ToString()));
                    string currentTime = int.Parse(dataReader["waktu"].ToString()) >= 3600 ? timeSpan.ToString(@"hh\:mm\:ss") : timeSpan.ToString(@"mm\:ss");
                    list.Add(new MusicModel()
                    {
                        UID = dataReader["UID"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Album = dataReader["album"].ToString(),
                        Artist = dataReader["artist"].ToString(),
                        Time = currentTime,

                        Number = count
                    });
                }

                dataReader.Close();

                _ = CloseConnection();

                return list;
            }
            else
            {
                return list;
            }
        }

        public void AddHistory(string UID)
        {
            if (OpenConnection())
            {
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;

                string query = "INSERT INTO history VALUES(@Uid, @Time)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                _ = cmd.Parameters.AddWithValue("@Uid", UID);
                _ = cmd.Parameters.AddWithValue("@Time", secondsSinceEpoch.ToString());
                _ = cmd.ExecuteNonQuery();


                query = "SELECT COUNT(waktu) FROM history";
                cmd = new MySqlCommand(query, connection);

                int totalHistory = int.Parse(cmd.ExecuteScalar().ToString());
                if (totalHistory > 5)
                {
                    query = string.Format("DELETE FROM history LIMIT " + (totalHistory - 5).ToString());
                    cmd = new MySqlCommand(query, connection);
                    _ = cmd.ExecuteNonQuery();
                }
                _ = CloseConnection();

            }
        }

        public void AddMusic(string path, string fileName, MusicModel musicModel)
        {
            if (OpenConnection())
            {
                string query = "INSERT INTO file(UID, location, filename) VALUES(@Uid, @Path, @Filename)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                _ = cmd.Parameters.AddWithValue("@Uid", musicModel.UID);
                _ = cmd.Parameters.AddWithValue("@Path", path);
                _ = cmd.Parameters.AddWithValue("@Filename", fileName);
                _ = cmd.ExecuteNonQuery();

                query = "INSERT INTO musicinfo VALUES(@Uid, @Title, @Artist, @Album, @Time, @Playlist)";
                cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Uid", musicModel.UID);
                cmd.Parameters.AddWithValue("@Title", musicModel.Title);
                cmd.Parameters.AddWithValue("@Artist", musicModel.Artist);
                cmd.Parameters.AddWithValue("@Album", musicModel.Album);
                cmd.Parameters.AddWithValue("@Time", musicModel.Time);
                cmd.Parameters.AddWithValue("@Playlist", 0);
                _ = cmd.ExecuteNonQuery();

                _ = CloseConnection();
            }
        }

        public void UpdateMusic(MusicModel musicModel)
        {
            bool statusInPlaylist = CheckUIDInPlaylist(musicModel.UID);
            if (OpenConnection())
            {
                int time = (int)double.Parse(musicModel.Time);
                string query = "UPDATE musicinfo SET UID=@Uid, title=@Title, artist=@Artist, album=@Album, waktu=@Waktu, playlist=@Playlist WHERE UID=@Uidd";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Uid", musicModel.UID);
                cmd.Parameters.AddWithValue("@Uidd", musicModel.UID);
                cmd.Parameters.AddWithValue("@Title", musicModel.Title);
                cmd.Parameters.AddWithValue("@Artist", musicModel.Artist);
                cmd.Parameters.AddWithValue("@Album", musicModel.Album);
                cmd.Parameters.AddWithValue("@Waktu", time);
                cmd.Parameters.AddWithValue("@Playlist", statusInPlaylist ? 1 : 0);
                _ = cmd.ExecuteNonQuery();

                _ = CloseConnection();
            }
        }

        public string GetPath(string UID)
        {
            string query = string.Format("SELECT * FROM file WHERE UID='{0}'", UID);
            string path = "";

            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    path = string.Format("{0}/{1}", dataReader["location"].ToString(), dataReader["filename"].ToString());
                }

                dataReader.Close();

                _ = CloseConnection();

                return path;
            }
            else
            {
                return path;
            }
        }

        public bool CheckFile(string path, string fileName)
        {

            if (OpenConnection())
            {
                string query = "SELECT * FROM file WHERE location=@Location AND filename=@Filename";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Location", path);
                cmd.Parameters.AddWithValue("@Filename", fileName);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                bool status = dataReader.Read();

                dataReader.Close();

                _ = CloseConnection();

                return status;
            }
            else
            {
                return false;
            }
        }

        public bool CheckUIDInPlaylist(string UID)
        {
            string query = string.Format("SELECT * FROM musicinfo WHERE UID='{0}'", UID);
            bool isInPlaylist = false;
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    if (dataReader["playlist"].ToString().Equals("1"))
                    {
                        isInPlaylist = true;
                    }
                }

                dataReader.Close();

                _ = CloseConnection();

                return isInPlaylist;
            }
            else
            {
                return isInPlaylist;
            }
        }

        public bool CheckUID(string UID)
        {
            string query = string.Format("SELECT * FROM file WHERE UID='{0}'", UID);
            bool isInPlaylist = false;
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    if (dataReader["UID"].ToString().Equals(UID))
                    {
                        isInPlaylist = true;
                    }
                }

                dataReader.Close();

                _ = CloseConnection();

                return isInPlaylist;
            }
            else
            {
                return isInPlaylist;
            }
        }

        public void SetToPlaylist(string UID)
        {
            int currentStatus = CheckUIDInPlaylist(UID) ? 0 : 1;
            if (OpenConnection())
            {
                string query = "UPDATE musicinfo SET playlist=@Status WHERE UID=@Uid";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                _ = cmd.Parameters.AddWithValue("@Uid", UID);
                _ = cmd.Parameters.AddWithValue("@Status", currentStatus);
                _ = cmd.ExecuteNonQuery();

                _ = CloseConnection();
            }
        }
    }
}

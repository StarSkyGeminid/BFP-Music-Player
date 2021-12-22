using System.IO;

namespace BFPMusicPlayer.Backend
{
    public class LastVolume
    {

        private readonly string fileName = @"volLast.conf";
        private readonly string configDir = @"./Config/";

        public void WriteLastVolume(double value)
        {
            if (!Directory.Exists(configDir))
            {
                System.IO.Directory.CreateDirectory(configDir);
            }

            File.WriteAllText(configDir + fileName, value.ToString());
        }

        public double GetLastVolume()
        {
            if (File.Exists(configDir + fileName))
            {
                return double.Parse(File.ReadAllText(configDir + fileName));
            }
            return 0.03;
        }
    }
}

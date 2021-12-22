namespace BFPMusicPlayer.Model
{
    internal class PlayBackModel
    {
        public string CurrentTime { get; set; }
        public string EndTime { get; set; }
        public bool IsPlaying { get; set; }
        public bool SliderDragging { get; set; }
        public bool SliderDragComplete { get; set; }
        public double SliderValue { get; set; }
        public double Volume { get; set; }
        public double SliderMaxValue { get; set; }
    }
}

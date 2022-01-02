using BFPMusicPlayer.Backend;
using BFPMusicPlayer.Command;
using BFPMusicPlayer.Model;
using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace BFPMusicPlayer.ViewModel.UserControl
{
    public class PlayBackViewModel : ViewModelBase
    {
        private readonly Music music = new Music();

        private MainControl mainControl;
        private MediaPlayer mediaPlayer;
        private LastVolume lastVolume;
        private PlayBackModel playBackModel;

        private bool dragStarted = new bool();
        private bool isDragging = new bool();
        private bool dragCompleted = new bool();
        private bool isMediaEnded = new bool();

        private Action delegateAction1;
        private Action delegateAction2;

        public LoopTypeEnum CurrentLoopType { get; private set; }

        public int GetCurrentRequester { get; private set; }

        public int GetCurrentTrackOwner { get; private set; }

        public string CurrentTime => playBackModel.CurrentTime;

        public string EndTime => playBackModel.EndTime;

        public void SetCurrentRequester(int requester)
        {
            GetCurrentRequester = requester;
        }

        public void SetCurrentTrackOwner(int owner)
        {
            GetCurrentTrackOwner = owner;
        }

        public void SetCurrentPlay(MusicModel musicModel)
        {
            GetCurrentPlay = musicModel;
            NewMusic(mainControl.GetPath(GetCurrentPlay.UID));
        }

        public MusicModel GetCurrentPlay { get; private set; } = new MusicModel();

        public bool Playing
        {
            get => playBackModel.IsPlaying;
            set
            {
                playBackModel.IsPlaying = value;
                OnPropertyChanged(nameof(Playing));
            }
        }

        public double SliderMaxLocation => playBackModel.SliderMaxValue;

        public void SetAction(Action action1, Action action2)
        {
            delegateAction1 = action1;
            delegateAction2 = action2;
        }

        public double SliderVolValue
        {
            get => playBackModel.Volume;
            set
            {
                playBackModel.Volume = value;
                OnPropertyChanged(nameof(SliderVolValue));

                mediaPlayer.Volume = playBackModel.Volume;
                lastVolume.WriteLastVolume(playBackModel.Volume);
            }
        }

        public string FillLoveIcon { get; private set; }

        public string StrokeLoveIcon { get; private set; }

        public string LoopButtonForeground { get; private set; }

        public string LoopType { get; private set; }

        public double SliderValue
        {
            get => playBackModel.SliderValue;
            set
            {
                playBackModel.SliderValue = value;
                OnPropertyChanged(nameof(SliderValue));
            }
        }

        public bool ThumbDragStarted
        {
            get => dragStarted;
            set
            {
                dragStarted = value;
                isDragging = true;
            }
        }

        public bool ThumbDragCompleted
        {
            get => dragCompleted;
            set
            {
                dragCompleted = value;
                dragStarted = false;
                isDragging = false;
                if (!dragCompleted)
                {
                    SkipTrack();
                }
            }
        }

        public void UpdateCurrentTime()
        {
            isDragging = true;

            TimeSpan timeSpan = TimeSpan.FromSeconds(playBackModel.SliderValue);
            playBackModel.CurrentTime = double.Parse(playBackModel.SliderValue.ToString()) >= 3600 ? timeSpan.ToString(@"hh\:mm\:ss") : timeSpan.ToString(@"mm\:ss");

            OnPropertyChanged(nameof(CurrentTime));
        }

        public DefaultCommand BackCommand { get; set; }
        public DefaultCommand NextCommand { get; set; }
        public PlayBackCommand PlayCommand { get; set; }
        public DefaultCommand VolDownCommand { get; set; }
        public DefaultCommand VolUpCommand { get; set; }
        public DefaultCommand AddPlaylistCommand { get; set; }
        public DefaultCommand AddMusic { get; set; }
        public DefaultCommand LoopButton { get; set; }

        public PlayBackViewModel()
        {
            ObjectInit();

            SetDefaultView();

            mediaPlayer.Volume = playBackModel.Volume - 0.01;
            StartTimer();

            mediaPlayer.MediaEnded += PlayEnded;

            music.UpdateAllDB();
        }

        private void SetDefaultView()
        {
            OnPropertyChanged(nameof(SliderValue));
            OnPropertyChanged(nameof(SliderMaxLocation));
            OnPropertyChanged(nameof(CurrentTime));
            OnPropertyChanged(nameof(EndTime));

            isMediaEnded = false;
            isDragging = false;

            playBackModel.Volume = lastVolume.GetLastVolume();
            OnPropertyChanged(nameof(SliderVolValue));

            FillLoveIcon = "Transparent";
            OnPropertyChanged(nameof(FillLoveIcon));

            StrokeLoveIcon = "#909195";
            OnPropertyChanged(nameof(StrokeLoveIcon));
        }

        private void ObjectInit()
        {
            mainControl = new MainControl();
            mediaPlayer = new MediaPlayer();
            lastVolume = new LastVolume();

            PlayCommand = new PlayBackCommand(this);
            VolDownCommand = new DefaultCommand(VolDownClick);
            VolUpCommand = new DefaultCommand(VolUpClick);
            AddPlaylistCommand = new DefaultCommand(AddToPlaylist);
            AddMusic = new DefaultCommand(Add);
            BackCommand = new DefaultCommand(BackTrack);
            NextCommand = new DefaultCommand(NextTrack);
            LoopButton = new DefaultCommand(LoopTrack);

            playBackModel = new PlayBackModel
            {
                SliderValue = 0,
                SliderMaxValue = 100,
                CurrentTime = "00:00",
                EndTime = "00:00",
            };

            LoopType = "";
            LoopButtonForeground = "#909195";
            OnPropertyChanged(nameof(LoopType));
            OnPropertyChanged(nameof(LoopButtonForeground));

            CurrentLoopType = LoopTypeEnum.noLoop;
        }

        public enum LoopTypeEnum
        {
            all = 0,
            once,
            noLoop,
        }

        private void LoopTrack()
        {
            if (CurrentLoopType - 2 == LoopTypeEnum.all)
            {
                LoopType = "";
                LoopButtonForeground = "Black";
                OnPropertyChanged(nameof(LoopType));
                OnPropertyChanged(nameof(LoopButtonForeground));
                CurrentLoopType = LoopTypeEnum.all;
            }
            else if (CurrentLoopType + 1 == LoopTypeEnum.once)
            {
                LoopType = "1";
                LoopButtonForeground = "Black";
                OnPropertyChanged(nameof(LoopType));
                OnPropertyChanged(nameof(LoopButtonForeground));
                CurrentLoopType = LoopTypeEnum.once;
            }
            else if (CurrentLoopType + 1 == LoopTypeEnum.noLoop)
            {
                LoopType = "";
                LoopButtonForeground = "#909195";
                OnPropertyChanged(nameof(LoopType));
                OnPropertyChanged(nameof(LoopButtonForeground));
                CurrentLoopType = LoopTypeEnum.noLoop;
            }
        }

        private void BackTrack()
        {
            delegateAction1();
        }

        private void NextTrack()
        {
            delegateAction2();
        }

        private void AddToPlaylist()
        {
            mainControl.SetToPlaylist(GetCurrentPlay.UID);

            if (mainControl.CheckUIDInPlaylist(GetCurrentPlay.UID))
            {
                FillLoveIcon = "#EE1702";
                OnPropertyChanged(nameof(FillLoveIcon));

                StrokeLoveIcon = "#EE1702";
                OnPropertyChanged(nameof(StrokeLoveIcon));
            }
            else
            {
                FillLoveIcon = "Transparent";
                OnPropertyChanged(nameof(FillLoveIcon));

                StrokeLoveIcon = "#909195";
                OnPropertyChanged(nameof(StrokeLoveIcon));
            }
        }

        public void Add()
        {
            music.Add();
        }

        private void NewMusic(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {

                mediaPlayer.Open(new Uri(path));

                OnPlay(true);

                playBackModel.IsPlaying = true;
                OnPropertyChanged(nameof(Playing));

                if (mainControl.CheckUIDInPlaylist(GetCurrentPlay.UID))
                {
                    FillLoveIcon = "#EE1702";
                    OnPropertyChanged(nameof(FillLoveIcon));

                    StrokeLoveIcon = "#EE1702";
                    OnPropertyChanged(nameof(StrokeLoveIcon));
                }
                else
                {
                    FillLoveIcon = "Transparent";
                    OnPropertyChanged(nameof(FillLoveIcon));

                    StrokeLoveIcon = "#909195";
                    OnPropertyChanged(nameof(StrokeLoveIcon));
                }

                mainControl.AddHistory(GetCurrentPlay.UID);

                music.Update(GetCurrentPlay.UID);
            }
        }

        private void PlayEnded(object sender, EventArgs e)
        {
            isMediaEnded = true;

            if (CurrentLoopType == LoopTypeEnum.once)
            {
                NewMusic(mainControl.GetPath(GetCurrentPlay.UID));
                OnPlay(true);
            }
            else if (CurrentLoopType == LoopTypeEnum.all)
            {
                NextTrack();
            }
            else if (CurrentLoopType == LoopTypeEnum.noLoop)
            {
                playBackModel.IsPlaying = false;
                OnPropertyChanged(nameof(Playing));
            }
        }

        private void VolDownClick()
        {
            playBackModel.Volume = (playBackModel.Volume - 0.01 > 0) ? (playBackModel.Volume - 0.01) : 0;
            OnPropertyChanged(nameof(SliderVolValue));

            mediaPlayer.Volume = playBackModel.Volume;
            lastVolume.WriteLastVolume(playBackModel.Volume);
        }

        private void VolUpClick()
        {
            playBackModel.Volume = (playBackModel.Volume + 0.01 < 1) ? (playBackModel.Volume + 0.01) : 1;
            OnPropertyChanged(nameof(SliderVolValue));

            mediaPlayer.Volume = playBackModel.Volume;
            lastVolume.WriteLastVolume(playBackModel.Volume);
        }

        private void StartTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object obj, EventArgs e)
        {
            if (mediaPlayer.Source != null && mediaPlayer.NaturalDuration.HasTimeSpan && !isDragging && playBackModel.IsPlaying)
            {
                playBackModel.CurrentTime = mediaPlayer.Position.ToString(@"mm\:ss");
                OnPropertyChanged(nameof(EndTime));

                playBackModel.EndTime = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                OnPropertyChanged(nameof(CurrentTime));
            }
            else if (!playBackModel.IsPlaying && !isDragging && playBackModel.CurrentTime.Equals(playBackModel.EndTime))
            {
                playBackModel.CurrentTime = "00:00";
                OnPropertyChanged(nameof(CurrentTime));

                playBackModel.EndTime = "00:00";
                OnPropertyChanged(nameof(EndTime));
            }
            if (mediaPlayer.Source != null && mediaPlayer.NaturalDuration.HasTimeSpan && !dragStarted && !isDragging && playBackModel.IsPlaying)
            {
                playBackModel.SliderMaxValue = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                OnPropertyChanged(nameof(SliderMaxLocation));

                playBackModel.SliderValue = mediaPlayer.Position.TotalSeconds;
                OnPropertyChanged(nameof(SliderValue));
            }
        }

        public void OnPlay(bool isReset = false)
        {
            if (!isReset)
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(playBackModel.SliderValue);
            }

            mediaPlayer.Play();

            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                playBackModel.SliderMaxValue = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                OnPropertyChanged(nameof(SliderMaxLocation));

                playBackModel.EndTime = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                OnPropertyChanged(nameof(EndTime));
            }
            else
            {
                Playing = false;
            }

            if (isMediaEnded)
            {
                playBackModel.SliderValue = 0;
                mediaPlayer.Position = TimeSpan.FromSeconds(playBackModel.SliderValue);
                mediaPlayer.Play();

                isMediaEnded = false;
                OnPropertyChanged(nameof(SliderValue));
            }

        }

        public void OnPause()
        {
            mediaPlayer.Pause();
        }

        public void SkipTrack()
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(playBackModel.SliderValue);

            if (!playBackModel.IsPlaying)
            {
                mediaPlayer.Pause();
            }
        }
    }
}

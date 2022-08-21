using LibVLCSharp.Shared;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skmr.ClipToTok.Utility;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Timers;
using System.Windows.Input;

namespace Skmr.ClipToTok.ViewModels
{
    public class PlayerViewModel : ReactiveObject
    {
        public MediaPlayer MediaPlayer { get; set; }
        public string VideoFile { get; set; }
        public bool HasTimeFrame { get; set; }



        private TimeSpan _time;
        public TimeSpan Time 
        {
            get
            {
                _time = TimeSpan.FromMilliseconds(MediaPlayer.Time);
                return _time;
            } 
            set
            {
                _time = value;
                MediaPlayer.Time = (long) _time.TotalMilliseconds;
                this.RaiseAndSetIfChanged(ref _time, value); 
            }
        }


        LibVLC _libVLC;

        public PlayerViewModel()
        {
            _libVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(_libVLC);
            MediaPlayer.EndReached += MediaPlayer_EndReached;

            PlayCommand = ReactiveCommand.Create(PlayVideo);
            PlaySelectionCommand = ReactiveCommand.Create(PlaySection);
            JumpAheadCommand = ReactiveCommand.Create(() => JumpRelative(10));
            JumpBackCommand = ReactiveCommand.Create(() => JumpRelative(-10));
            JumpToStartCommand = ReactiveCommand.Create(() => JumpAbsolute(0));
            MuteCommand = ReactiveCommand.Create(() => MediaPlayer.Mute = !MediaPlayer.Mute);

            //This man is my hero: ASanch
            //https://stackoverflow.com/questions/34906839/how-do-i-implement-a-countdown-timer-using-reactiveui
            var currentTime = 
                Observable
                .Interval(TimeSpan.FromMilliseconds(100))
                .Select(x => MediaPlayer.Time)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    onNext: x => CurrentTime = new TimeSpan(
                        (TimeSpan.FromMilliseconds(x).Ticks / (TimeSpan.TicksPerSecond))* (TimeSpan.TicksPerSecond)),
                    onCompleted: () => { this.CurrentTime = TimeSpan.Zero; });

        }




        #region Current Time Notifier
        [Reactive]
        public TimeSpan CurrentTime { get; set; }
        public void UpdateCurrentTime()
        {

        }
        #endregion
        public ICommand CurrentTimeCommand { get; }
        

        public ICommand PlayCommand { get; set; }
        public ICommand PlaySelectionCommand { get; set; }
        public ICommand JumpAheadCommand { get; set; }
        public ICommand JumpBackCommand { get; set; }
        public ICommand JumpToStartCommand { get; set; }
        public ICommand MuteCommand { get; set; }

        


        //Time Skipping
        private void JumpRelative(int seconds)
        {
            if (Time.TotalSeconds + seconds < 0) Time = TimeSpan.Zero;
            else Time += TimeSpan.FromSeconds(seconds);

            if (!MediaPlayer.IsPlaying)
            {
                MediaPlayer.NextFrame();
            }
        }
        private void JumpAbsolute(int seconds)
        {
            if (seconds < 0) Time = TimeSpan.Zero;
            else Time = TimeSpan.FromSeconds(seconds);
            if (!MediaPlayer.IsPlaying)
            {
                
                MediaPlayer.NextFrame();
            }
        }


        //Default Playback
        private string playedBackVideoCurrent = String.Empty;
        private TimeSpan timeFrameDuration;
        private void PlayVideo()
        {
            if (File.Exists(VideoFile))
            {
                if (!playedBackVideoCurrent.Equals(VideoFile))
                {
                    MediaPlayer.Play(new Media(_libVLC, new Uri(VideoFile)));
                    playedBackVideoCurrent = VideoFile;
                }
                else if (!MediaPlayer.IsPlaying && playedBackVideoCurrent.Equals(VideoFile))
                {
                    MediaPlayer.Play();
                }
                else
                {
                    MediaPlayer.Pause();
                }
            }
            else if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
            }
        }
        private void MediaPlayer_EndReached(object? sender, EventArgs e)
        {
            playedBackVideoCurrent = String.Empty;
        }

        //TimeFrame Playback
        public TimeSpan Start { get; set; }
        public TimeSpan Duration { get; set; }
        private void PlaySection()
        {
            PlaySection(Start, timeFrameDuration);
        }
        public void PlaySection(TimeSpan start, TimeSpan duration)
        {
            if (File.Exists(VideoFile))
            {
                MediaPlayer.Play(new Media(_libVLC, new Uri(VideoFile)));
                MediaPlayer.Time = (long)start.TotalMilliseconds;

                timeFrameDuration = duration;
                var timer = new System.Timers.Timer(100);
                timer.Elapsed += Timer_Elapsed;
                timer.AutoReset = true;
                timer.Enabled = true;

                playedBackVideoCurrent = VideoFile;
            }
        }
        private void Timer_Elapsed(object sender, EventArgs e)
        {
            if (MediaPlayer.Time > (long)Duration.TotalMilliseconds + (long)Start.TotalMilliseconds)
            {
                MediaPlayer.Pause();
                (sender as System.Timers.Timer).Enabled = false;
            }
        }



        //Overlay Drawing
        public ScreenPosViewModel Gameplay { get; set; } = new ScreenPosViewModel();
        public ScreenPosViewModel Webcam { get; set; } = new ScreenPosViewModel();
    }
}
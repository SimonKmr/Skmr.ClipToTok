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
                    onNext: x => CurrentTime = TimeSpan.FromMilliseconds(x).Floor(),
                    onCompleted: () => { this.CurrentTime = TimeSpan.Zero; });
        }




        #region Current Time Notifier
        [Reactive]
        public TimeSpan CurrentTime { get; set; }
        public ICommand CurrentTimeCommand { get; }
        #endregion

        public ICommand PlayCommand { get; set; }
        public ICommand PlaySelectionCommand { get; set; }
        public ICommand JumpAheadCommand { get; set; }
        public ICommand JumpBackCommand { get; set; }
        public ICommand JumpToStartCommand { get; set; }
        public ICommand MuteCommand { get; set; }

        


        //Time Skipping
        private void JumpRelative(int seconds)
        {
            if (MediaPlayer.Time + seconds * 1000 < 0) MediaPlayer.Time = 0;
            else MediaPlayer.Time += seconds * 1000;

            if (!MediaPlayer.IsPlaying) MediaPlayer.NextFrame();
        }
        private void JumpAbsolute(int seconds)
        {
            if (seconds < 0) MediaPlayer.Time = 0;
            else MediaPlayer.Time = seconds * 1000;
            
            if (!MediaPlayer.IsPlaying) MediaPlayer.NextFrame();
        }


        //Default Playback
        private string playedBackVideoCurrent = String.Empty;

        private void PlayVideo()
        {
            var videoFile = ViewModelBus.SettingsViewModel.VideoFile;
            if (File.Exists(videoFile))
            {
                if (!playedBackVideoCurrent.Equals(videoFile))
                {
                    MediaPlayer.Play(new Media(_libVLC, new Uri(videoFile)));
                    playedBackVideoCurrent = videoFile;
                }
                else if (!MediaPlayer.IsPlaying && playedBackVideoCurrent.Equals(videoFile))
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
        private TimeSpan timeFrameDuration;
        private TimeSpan timeFrameStart;

        private void PlaySection()
        {
            PlaySection(
                ViewModelBus.SettingsViewModel.TimeFrameStart, 
                ViewModelBus.SettingsViewModel.TimeFrameDuration);
        }
        public void PlaySection(TimeSpan start, TimeSpan duration)
        {
            var videoFile = ViewModelBus.SettingsViewModel.VideoFile;
            if (File.Exists(videoFile))
            {
                MediaPlayer.Play(new Media(_libVLC, new Uri(videoFile)));
                MediaPlayer.Time = start.ToMediaPlayerTime();

                timeFrameDuration = duration;
                timeFrameStart = start;

                var timer = new System.Timers.Timer(100);
                timer.Elapsed += Timer_Elapsed;
                timer.AutoReset = true;
                timer.Enabled = true;

                playedBackVideoCurrent = videoFile;
            }
        }
        private void Timer_Elapsed(object sender, EventArgs e)
        {
            if (MediaPlayer.Time > (timeFrameDuration + timeFrameStart).ToMediaPlayerTime())
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
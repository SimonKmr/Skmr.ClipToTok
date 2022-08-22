using ReactiveUI;
using Skmr.Editor;
using Skmr.Editor.Instructions;
using Skmr.Editor.Instructions.Interfaces;
using Skmr.Editor.Media;
using System.Windows.Input;
using static Skmr.Editor.Ffmpeg;
using SkiaSharp;
using System.Reactive;
using Splat;
using Skmr.ClipToTok.Utility;

namespace Skmr.ClipToTok.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public RoutingState Router { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoSettings { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHighlighter { get; }


        public HighlighterViewModel Hvm { get; }
        public SettingsViewModel Svm { get; }
        public PlayerViewModel Pvm { get; }

        public MainViewModel()
        {
            Svm = new SettingsViewModel();
            Hvm = new HighlighterViewModel();
            Pvm = new PlayerViewModel();

            Svm.OnRenderEvent += Svm_OnRenderEvent;
            Svm.ScreenPosGameplay.OnScreenPosChanged += ScreenPosGameplay_OnScreenPosChanged;
            Svm.ScreenPosWebcam.OnScreenPosChanged += ScreenPosWebcam_OnScreenPosChanged;

            Hvm.OnHighlightSelected += Hvm_OnHighlightSelected;

            Router = new RoutingState();
            GoSettings = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(Svm));
            GoHighlighter = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(Hvm));
            Router.Navigate.Execute(Svm);

            ViewModelBus.PlayerViewModel = Pvm;
            ViewModelBus.SettingsViewModel = Svm;
        }

        private void ScreenPosWebcam_OnScreenPosChanged(int x, int y, int width, int height)
        {
            Pvm.Gameplay.Set(x,y, width, height);
        }

        private void ScreenPosGameplay_OnScreenPosChanged(int x, int y, int width, int height)
        {
            Pvm.Webcam.Set(x, y, width, height);
        }

        private void Svm_OnRenderEvent(object sender, string e)
        {
            AddLogEntry(e);
        }

        public string _Log;
        public delegate void LogHandler(string line);
        public event LogHandler OnLog = delegate { };
        private void AddLogEntry(string line)
        {
            OnLog(line);
            Log += $"{DateTime.Now.ToString("hh:mm:ss.FFF")}    {line}\n";
        }
        public string Log
        {
            get { return _Log; }
            set { this.RaiseAndSetIfChanged(ref _Log, value); }
        }


        private void Hvm_OnHighlightSelected(object sender, TimeSpan start, TimeSpan duration, bool select)
        {
            if (select)
            {
                Svm.TimeFrameStart = start;
                Svm.TimeFrameDuration = duration;
                Svm.HasTimeFrame = select;
            }
            else
            {
                Pvm.PlaySection(start, duration);
            }
        }
    }
}


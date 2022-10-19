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
    public class MainViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }
        public RoutingState Router { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoSettings { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHighlighter { get; }


        public HighlighterViewModel HighlighterViewModel { get; }
        public ProjectViewModel ProjectViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        public VideoViewModel VideoViewModel { get; }

        public MainViewModel()
        {
            Activator = new ViewModelActivator();

            VideoViewModel = new VideoViewModel();
            ProjectViewModel = new ProjectViewModel();
            HighlighterViewModel = new HighlighterViewModel();
            PlayerViewModel = new PlayerViewModel();

            ProjectViewModel.OnRenderEvent += Svm_OnRenderEvent;

            HighlighterViewModel.OnHighlightSelected += Hvm_OnHighlightSelected;

            Router = new RoutingState();
            GoSettings = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(ProjectViewModel));
            GoHighlighter = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(HighlighterViewModel));
            Router.Navigate.Execute(ProjectViewModel);

            ViewModelBus.PlayerViewModel = PlayerViewModel;
            ViewModelBus.SettingsViewModel = ProjectViewModel;
            ViewModelBus.VideoViewModel = VideoViewModel;
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
                VideoViewModel.TimeFrameStart = start;
                VideoViewModel.TimeFrameDuration = duration;
                VideoViewModel.HasTimeFrame = select;
            }
            else
            {
                PlayerViewModel.PlaySection(start, duration);
            }
        }
    }
}


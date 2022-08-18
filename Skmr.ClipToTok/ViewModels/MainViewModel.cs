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

namespace Skmr.ClipToTok.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public RoutingState Router { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoSettings { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHighlighter { get; }


        public HighlighterViewModel Hvm { get; }
        public SettingsViewModel Svm { get; }

        public MainViewModel()
        {
            Svm = new SettingsViewModel();
            Hvm = new HighlighterViewModel();

            Svm.OnRenderEvent += Svm_OnRenderEvent;

            Router = new RoutingState();
            GoSettings = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(Svm));
            GoHighlighter = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(Hvm));
            Router.Navigate.Execute(Svm);
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

        

    }
}


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
using Skmr.ClipToTok.ViewModels.Analyzer;
using Skmr.ClipToTok.ViewModels.ClipToTok;

namespace Skmr.ClipToTok.ViewModels
{
    public class ClipToTokViewModel : ReactiveObject, IRoutableViewModel
    {
        public ProjectViewModel ProjectViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }

        public string? UrlPathSegment => "ClipToTok";
        public IScreen HostScreen { get; }




        public ClipToTokViewModel(IScreen screen = null)
        {
            HostScreen = screen;

            ProjectViewModel = new ProjectViewModel();
            PlayerViewModel = new PlayerViewModel();

            ProjectViewModel.Renderer.OnRenderEvent += Svm_OnRenderEvent;


            ViewModelBus.PlayerViewModel = PlayerViewModel;
            ViewModelBus.SettingsViewModel = ProjectViewModel;
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
                //VideoViewModel.TimeFrameStart = start;
                //VideoViewModel.TimeFrameDuration = duration;
                //VideoViewModel.HasTimeFrame = select;
            }
            else
            {
                PlayerViewModel.PlaySection(start, duration);
            }
        }
    }
}


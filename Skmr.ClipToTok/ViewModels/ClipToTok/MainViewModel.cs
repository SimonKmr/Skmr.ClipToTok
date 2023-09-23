using ReactiveUI;
using Skmr.ClipToTok.Utility;
using Skmr.ClipToTok.ViewModels.Video;

namespace Skmr.ClipToTok.ViewModels.ClipToTok
{
    public class MainViewModel : RenderableViewModel, IRoutableViewModel
    {
        public ProjectViewModel ProjectViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }

        public string? UrlPathSegment => "ClipToTok";
        public IScreen HostScreen { get; }




        public MainViewModel(IScreen screen = null)
        {
            HostScreen = screen;

            ProjectViewModel = new ProjectViewModel();
            PlayerViewModel = new PlayerViewModel();


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

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}


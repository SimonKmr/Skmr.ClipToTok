using ReactiveUI;
using Skmr.ClipToTok.ViewModels.Video;
using Splat;

namespace Skmr.ClipToTok.ViewModels.Analyzer
{
    public class MainViewModel : ReactiveObject, IRoutableViewModel
    {
        public PlayerViewModel PlayerViewModel { get; }
        public HighlighterViewModel HighlighterViewModel { get; }

        public string? UrlPathSegment => "Analyzer";
        public IScreen HostScreen { get; }

        public MainViewModel(IScreen screen = null)
        {
            HostScreen = screen;
        }
    }
}

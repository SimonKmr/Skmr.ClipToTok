using ReactiveUI;
using Skmr.ClipToTok.ViewModels.Analyzer;
using Splat;

namespace Skmr.ClipToTok.ViewModels
{
    public class AnalyzerViewModel : ReactiveObject, IRoutableViewModel
    {
        public PlayerViewModel PlayerViewModel { get; }
        public HighlighterViewModel HighlighterViewModel { get; }

        public string? UrlPathSegment => "Analyzer";
        public IScreen HostScreen { get; }

        public AnalyzerViewModel(IScreen screen = null)
        {
            HostScreen = screen;
        }
    }
}

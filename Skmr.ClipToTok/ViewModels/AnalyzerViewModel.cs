using ReactiveUI;
using Skmr.ClipToTok.ViewModels.Analyzer;

namespace Skmr.ClipToTok.ViewModels
{
    public class AnalyzerViewModel : ReactiveObject
    {
        public PlayerViewModel PlayerViewModel { get; }
        public HighlighterViewModel HighlighterViewModel { get; }

        public AnalyzerViewModel()
        {

        }
    }
}

using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Skmr.ClipToTok.ViewModels;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class HighlighterView : ReactiveUserControl<HighlighterViewModel>
    {
        public HighlighterView()
        {
            InitializeComponent();
        }
    }
}

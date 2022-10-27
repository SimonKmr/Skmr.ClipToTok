using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Skmr.ClipToTok.ViewModels.Analyzer;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class HighlightView : ReactiveUserControl<HighlightViewModel>
    {
        public HighlightView()
        {
            InitializeComponent();
        }
    }
}

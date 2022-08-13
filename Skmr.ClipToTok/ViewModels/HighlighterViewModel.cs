using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.ViewModels
{
    public class HighlighterViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "Highlighter";
        public IScreen HostScreen { get; }

        public HighlighterViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}

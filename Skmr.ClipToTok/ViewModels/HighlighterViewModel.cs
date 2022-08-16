using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;

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



        private void AddHighlight(HighlightViewModel vm)
        {
            vm.OnButtonPressed += Vm_OnButtonPressed;
            Highlights.Add(vm);
        }

        private void Vm_OnButtonPressed(object sender, TimeSpan start, TimeSpan duration, bool select)
        {
            OnHighlightSelected(this,start, duration, select);
        }

        public delegate void HighlightSelectedHandler(object sender, TimeSpan start, TimeSpan duration, bool select);
        public event HighlightSelectedHandler OnHighlightSelected = delegate { };

        public SourceList<HighlightViewModel> Highlights { get; set; } = new SourceList<HighlightViewModel>();

    }
}

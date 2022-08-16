using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using System.Collections.ObjectModel;
using DynamicData.Binding;

namespace Skmr.ClipToTok.ViewModels
{
    public class HighlighterViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "Highlighter";
        public IScreen HostScreen { get; }

        public HighlighterViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            var disposable = _highlightSources.Connect().Bind(out _highlights).Subscribe();

            _highlightSources.Add(new HighlightViewModel()
            {
                Start = TimeSpan.FromSeconds(0),
                Duration = TimeSpan.FromSeconds(50),
                Score = .75,
            }) ;
        }

        private SourceList<HighlightViewModel> _highlightSources = new SourceList<HighlightViewModel>();
        private ReadOnlyObservableCollection<HighlightViewModel> _highlights;
        
        
        public ReadOnlyObservableCollection<HighlightViewModel> Highlights => _highlights;
        
        
        private void AddHighlight(HighlightViewModel vm)
        {
            vm.OnButtonPressed += Vm_OnButtonPressed;
            _highlightSources.Add(vm);
        }

        public void Analyze()
        {
            //RunAnaylzer

            //Add ViewModels to Highlight List
        }
        
        
        private void Vm_OnButtonPressed(object sender, TimeSpan start, TimeSpan duration, bool select)
        {
            OnHighlightSelected(this,start, duration, select);
        }

        public delegate void HighlightSelectedHandler(object sender, TimeSpan start, TimeSpan duration, bool select);
        public event HighlightSelectedHandler OnHighlightSelected = delegate { };

        

    }
}

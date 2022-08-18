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
using System.Windows.Input;

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

            AnalyzeCommand = ReactiveCommand.Create(Analyze);
        }

        private SourceList<HighlightViewModel> _highlightSources = new SourceList<HighlightViewModel>();
        private ReadOnlyObservableCollection<HighlightViewModel> _highlights;
        
        public ReadOnlyObservableCollection<HighlightViewModel> Highlights => _highlights;
        
        
        private void AddHighlight(HighlightViewModel vm)
        {
            vm.OnSelectPressed += Vm_OnSelectPressed;
            vm.OnPlayPressed += Vm_OnPlayPressed;
            _highlightSources.Add(vm);
        }

        private void Vm_OnSelectPressed(object sender, TimeSpan start, TimeSpan duration, bool select)
        {
            OnHighlightSelected(this, start, duration, select);
        }
        private void Vm_OnPlayPressed(object sender, TimeSpan start, TimeSpan duration, bool select)
        {
            
        }


        public ICommand AnalyzeCommand { get; set; }
        public void Analyze()
        {
            Random random = new Random();
            //RunAnaylzer

            //Add ViewModels to Highlight List
            //This is just a dummy for testing Purposes
            AddHighlight(new HighlightViewModel()
            {
                Start = TimeSpan.FromSeconds(random.Next(20,4200)),
                Duration = TimeSpan.FromSeconds(random.Next(20,135)),
                Score = random.NextDouble(),
            });
        }
        
        




        public delegate void HighlightSelectedHandler(object sender, TimeSpan start, TimeSpan duration, bool select);
        public event HighlightSelectedHandler OnHighlightSelected = delegate { };

        
        
    }
}

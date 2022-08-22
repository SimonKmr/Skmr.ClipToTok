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
using Skmr.ClipToTok.Utility;
using Newtonsoft.Json;
using System.Reactive.Linq;

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
            NewCommand = ReactiveCommand.Create(Clear);
            ManualCommand = ReactiveCommand.Create(ManualAdd);
            ManualQuickCommand = ReactiveCommand.Create(ManualQuickAdd);
            
            SaveCommand = ReactiveCommand.Create(SaveAsync);
            ImportCommand = ReactiveCommand.Create(ImportAsync);
        }

        private SourceList<HighlightViewModel> _highlightSources = new SourceList<HighlightViewModel>();
        private ReadOnlyObservableCollection<HighlightViewModel> _highlights;
        
        public ReadOnlyObservableCollection<HighlightViewModel> Highlights => _highlights;
        
        
        private void AddHighlight(HighlightViewModel vm)
        {
            vm.OnSelectPressed += Vm_OnSelectPressed;
            vm.OnPlayPressed += Vm_OnPlayPressed;
            vm.DeleteRequest += Vm_DeleteRequest;
            _highlightSources.Add(vm);
        }

        private void Vm_DeleteRequest(object sender, EventArgs e)
        {
            var s = sender as HighlightViewModel;
            _highlightSources.Remove(s);
        }

        private void Vm_OnSelectPressed(object sender, TimeSpan start, TimeSpan duration, bool select)
        {
            OnHighlightSelected(this, start, duration, select);
        }
        private void Vm_OnPlayPressed(object sender, TimeSpan start, TimeSpan duration, bool select)
        {
            OnHighlightSelected(this, start, duration, select);
        }


        public ICommand AnalyzeCommand { get; set; }
        public ICommand NewCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public void Analyze()
        {

            //RunAnaylzer

            //Add ViewModels to Highlight List
            
            
            
            //This is just a dummy for testing Purposes
            Random random = new Random();
            AddHighlight(new HighlightViewModel()
            {
                Start = TimeSpan.FromSeconds(random.Next(20, 4200)),
                Duration = TimeSpan.FromSeconds(random.Next(20, 135)),
                Title = "Sample"
            }) ;
        }
        public void Clear()
        {
            _highlightSources.Clear();
        }


        public ICommand ManualCommand { get; set; }
        public ICommand ManualQuickCommand { get; set; }
        public TimeSpan ManualStart { get; set; } = TimeSpan.Zero;
        public TimeSpan ManualDuration { get; set; } = TimeSpan.FromSeconds(59);
        public string ManualComment { get; set; } = String.Empty;
        public void ManualAdd()
        {
            AddHighlight(new HighlightViewModel()
            {
                Start = ManualStart,
                Duration = ManualDuration,
                Title = ManualComment,
            });
        }

        public void ManualQuickAdd()
        {
            AddHighlight(new HighlightViewModel()
            {
                Start = TimeSpan.FromMilliseconds(ViewModelBus.PlayerViewModel.MediaPlayer.Time).Floor(),
                Duration = TimeSpan.FromSeconds(59),
                Title = String.Empty,
            });
        }


        public ICommand SaveCommand { get; set; }
        public async Task SaveAsync()
        {
            var dialogResult = await Interactions.SaveFileDialog.Handle(String.Empty);

            List<Highlight> highlights = new List<Highlight>();
            for(int i = 0; i < _highlightSources.Count; i++)
            {
                highlights.Add(Highlights[i].ToHighlight());
            }
            var highlightsArr = highlights.ToArray();

            var json = JsonConvert.SerializeObject(highlightsArr);
            using(var sw = new StreamWriter(dialogResult))
            {
                sw.Write(json);
            }
        }
        public async Task ImportAsync()
        {
            var file = await Interactions.OpenFileDialog.Handle(String.Empty);
            if (!File.Exists(file)) return;

            if (file.EndsWith(".json"))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    var highlightArr = JsonConvert.DeserializeObject<Highlight[]>(sr.ReadToEnd());
                    for(int i = 0; i < highlightArr.Length; i++)
                    {
                        _highlightSources.Add(highlightArr[i].ToHighlightViewModel());
                    }
                }
                return;
            }
            else if (file.EndsWith(".txt"))
                using (StreamReader sr = new StreamReader(file))
                    while (!sr.EndOfStream)
                        _highlightSources.Add(
                            Parser.CreateHighlightFromTxt(sr.ReadLine())
                            .ToHighlightViewModel());


            else if (file.EndsWith(".csv"))
                using (StreamReader sr = new StreamReader(file))
                    while (!sr.EndOfStream)
                        _highlightSources.Add(
                            Parser.CreateHighlightFromCsv(sr.ReadLine())
                            .ToHighlightViewModel());

        }

        public delegate void HighlightSelectedHandler(object sender, TimeSpan start, TimeSpan duration, bool select);
        public event HighlightSelectedHandler OnHighlightSelected = delegate { };
    }
}

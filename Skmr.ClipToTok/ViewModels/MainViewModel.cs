using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skmr.ClipToTok.Utility;
using Skmr.ClipToTok.ViewModels.Analyzer;
using Skmr.ClipToTok.ViewModels.ClipToTok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public ReactiveCommand<Unit, IRoutableViewModel> GoClipToTok { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHighlighter { get; }
        public ViewModelActivator Activator { get; }
        public RoutingState Router { get; }

        [Reactive]
        public ClipToTok.MainViewModel ClipToTok { get; set; }
        [Reactive]
        public Analyzer.MainViewModel Analyzer { get; set; }

        public MainViewModel()
        {
            Activator = new ViewModelActivator();

            ClipToTok = new ClipToTok.MainViewModel(this);
            Analyzer = new Analyzer.MainViewModel(this);

            Router = new RoutingState();
            GoClipToTok = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(ClipToTok));
            GoHighlighter = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(Analyzer));
            Router.Navigate.Execute(ClipToTok);
        }
    }
}

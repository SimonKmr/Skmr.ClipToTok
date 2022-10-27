using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skmr.ClipToTok.Utility;
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
        public ClipToTokViewModel ClipToTok { get; set; }
        [Reactive]
        public AnalyzerViewModel Analyzer { get; set; }

        public MainViewModel()
        {
            Activator = new ViewModelActivator();

            ClipToTok = new ClipToTokViewModel(this);
            Analyzer = new AnalyzerViewModel(this);

            Router = new RoutingState();
            GoClipToTok = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(ClipToTok));
            GoHighlighter = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(Analyzer));
            Router.Navigate.Execute(ClipToTok);
        }
    }
}

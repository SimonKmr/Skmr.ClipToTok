using ReactiveUI;
using Skmr.ClipToTok.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für HighlighterView.xaml
    /// </summary>
    public partial class HighlighterView : ReactiveUserControl<HighlighterViewModel>
    {
        // https://www.reactiveui.net/docs/getting-started/compelling-example
        public HighlighterView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Highlights, v => v.AnalyzedHighlights.ItemsSource).DisposeWith(d);
                
                this.BindCommand(ViewModel, vm => vm.AnalyzeCommand, v => v.btnAnalyze).DisposeWith(d);
            });
        }
    }
}

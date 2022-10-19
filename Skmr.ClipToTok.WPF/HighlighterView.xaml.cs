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
                this.Bind(ViewModel, vm => vm.ManualStart, v => v.txtManualStart.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ManualDuration, v => v.txtManualDuration.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ManualComment, v => v.txtManualComment.Text).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Highlights, v => v.icAnalyzedHighlights.ItemsSource).DisposeWith(d);
                
                this.BindCommand(ViewModel, vm => vm.AnalyzeCommand, v => v.btnAnalyze).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.NewCommand, v => v.btnNew).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.ManualCommand, v => v.btnManual).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.btnSave).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.ImportCommand, v => v.btnImport).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.ManualQuickCommand, v => v.btnManualQuick).DisposeWith(d);
            });
        }
    }
}

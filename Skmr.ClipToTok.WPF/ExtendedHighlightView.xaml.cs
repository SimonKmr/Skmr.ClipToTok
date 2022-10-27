using ReactiveUI;
using Skmr.ClipToTok.ViewModels.Analyzer;
using System.Reactive.Disposables;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für ExtendedHighlightView.xaml
    /// </summary>
    public partial class ExtendedHighlightView : ReactiveWindow<HighlightViewModel>
    {
        public ExtendedHighlightView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Title, v => v.lbTitle.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Description, v => v.atDescription.Text).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Score, v => v.lbScore.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Start, v => v.lbStart.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.End, v => v.lbEnd.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.DurationText, v => v.lbDuration.Content).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Title, v => v.txtTitle.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Description, v => v.txtDescription.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Score, v => v.txtScore.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Start, v => v.txtStart.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Duration, v => v.txtDuration.Text).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.DeleteCommand, v => v.btnDelete).DisposeWith(d);
            });
        }
    }
}

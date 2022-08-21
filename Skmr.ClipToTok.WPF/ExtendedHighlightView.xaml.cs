using ReactiveUI;
using Skmr.ClipToTok.ViewModels;

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
                this.OneWayBind(ViewModel, vm => vm.Title, v => v.lbTitle.Content);
                this.OneWayBind(ViewModel, vm => vm.Description, v => v.atDescription.Text);
                this.OneWayBind(ViewModel, vm => vm.Score, v => v.lbScore.Content);
                this.OneWayBind(ViewModel, vm => vm.Start, v => v.lbStart.Content);
                this.OneWayBind(ViewModel, vm => vm.End, v => v.lbEnd.Content);
                this.OneWayBind(ViewModel, vm => vm.DurationText, v => v.lbDuration.Content);

                this.Bind(ViewModel, vm => vm.Title, v => v.txtTitle.Text);
                this.Bind(ViewModel, vm => vm.Description, v => v.txtDescription.Text);
                this.Bind(ViewModel, vm => vm.Score, v => v.txtScore.Text);
                this.Bind(ViewModel, vm => vm.Start, v => v.txtStart.Text);
                this.Bind(ViewModel, vm => vm.Duration, v => v.txtDuration.Text);

                this.BindCommand(ViewModel, vm => vm.DeleteCommand, v => v.btnDelete);
            });
        }
    }
}

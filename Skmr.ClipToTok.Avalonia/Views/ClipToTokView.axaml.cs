using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Skmr.ClipToTok.Avalonia.Utils;
using Skmr.ClipToTok.ViewModels;
using System.Reactive.Disposables;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class ClipToTokView : ReactiveUserControl<ClipToTokViewModel>
    {
        public PlayerView PlayerView => this.FindControl<PlayerView>("playerView");
        ProjectView ProjectView => this.FindControl<ProjectView>("projectView");

        public ClipToTokView()
        {
            InitializeComponent();

            this.WhenActivated((d) =>
            {
                this.Bind(ViewModel, vm => vm.PlayerViewModel, v => v.PlayerView.DataContext).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ProjectViewModel, v => v.ProjectView.DataContext).DisposeWith(d);
            });

        }
    }
}

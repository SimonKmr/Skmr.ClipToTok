using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Skmr.ClipToTok.Avalonia.Utils;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Reactive.Disposables;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {

        //https://www.reactiveui.net/docs/handbook/data-binding/avalonia
        //https://docs.avaloniaui.net/guides/deep-dives/reactiveui
        public static MainWindow Instance { get; private set; }
        PlayerView PlayerView => this.FindControl<PlayerView>("playerView");
        ProjectView ProjectView => this.FindControl<ProjectView>("projectView");
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

            this.WhenActivated((d) =>
            {
                this.OneWayBind(ViewModel, vm => vm.PlayerViewModel, v => v.PlayerView.DataContext).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.ProjectViewModel, v => v.ProjectView.DataContext).DisposeWith(d);
            });

            this.Opened += MainWindow_Opened;
        }

        private void MainWindow_Opened(object? sender, EventArgs e)
        {
            LoadedEventManager.Fire();
        }
    }
}

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Skmr.ClipToTok.Avalonia.Utils;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {

        //https://www.reactiveui.net/docs/handbook/data-binding/avalonia
        //https://docs.avaloniaui.net/guides/deep-dives/reactiveui
        ClipToTokView ClipToTok => this.FindControl<ClipToTokView>("clipToTok");
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            this.WhenActivated((d) =>
            {
                this.Bind(ViewModel, vm => vm.ClipToTok, v => v.ClipToTok.DataContext);
            });
            this.Opened += MainWindow_Opened;
        }

        private void MainWindow_Opened(object? sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                LoadedEventManager.Fire();


            });
        }
    }
}

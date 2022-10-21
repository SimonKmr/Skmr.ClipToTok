using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ReactiveUI;
using Skmr.ClipToTok.Avalonia.Utils;
using Skmr.ClipToTok.Avalonia.Views.ExtendedViews;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class VideoView : ReactiveUserControl<VideoViewModel>
    {
        public ItemsControl FrameItemsControl => this.FindControl<ItemsControl>("icScreenPositions");
        public TextBox SourceVideoTextBox => this.FindControl<TextBox>("txtSourceVideo");
        public Border SourceVideoDropPanel => this.FindControl<Border>("dpSourceVideo");
        public Button AddFrameButton => this.FindControl<Button>("btnAddFrame");
        public Button AttributeButton => this.FindControl<Button>("btnAttributes");
        public VideoView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.VideoFile, v => v.SourceVideoTextBox.Text).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.ScreenPositions, v => v.FrameItemsControl.Items).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.AddFrameCommand, v => v.AddFrameButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.OpenAttributesCommand, v => v.AttributeButton).DisposeWith(d);
                LoadedEventManager.Loaded += LoadedEventManager_Loaded;
            });
            SourceVideoDropPanel.AddHandler(DragDrop.DropEvent, Drop);
        }

        private void LoadedEventManager_Loaded(object? sender, EventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ViewModel!.AttributesWindow.RegisterHandler(DoShowAttributesWindow);
            });

        }

        private async Task DoShowAttributesWindow(InteractionContext<VideoViewModel, object?> interaction)
        {
            var window = new ExtendedVideoView();
            window.DataContext = interaction.Input;
            var result = await window.ShowDialog<object?>(MainWindow.Instance);
            interaction.SetOutput(result);
        }

        private void Drop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                SourceVideoTextBox.Text = e.Data.GetFileNames().ToArray()[0];
            }
        }
    }
}

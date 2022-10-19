using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Skmr.ClipToTok;
using System;
using Avalonia.ReactiveUI;
using Skmr.ClipToTok.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using vlc = LibVLCSharp.Avalonia;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class PlayerView : ReactiveUserControl<PlayerViewModel>
    {
        public Button JumpBackButton => this.FindControl<Button>("btnJumpBack");
        public Button JumpAheadButton => this.FindControl<Button>("btnJumpAhead");
        public Button RestartButton => this.FindControl<Button>("btnRestart");
        public Button PlayButton => this.FindControl<Button>("btnPlay");
        public Button MuteButton => this.FindControl<Button>("btnMute");
        public Button PlaySectionButton => this.FindControl<Button>("btnPlaySelection");
        public Label TimeLabel => this.FindControl<Label>("lbCurrentTime");
        public vlc.VideoView VideoView => this.FindControl<vlc.VideoView>("videoView");
        
        public PlayerView()
        {
            InitializeComponent();
            this.WhenActivated((d) =>
            {
                this.OneWayBind(ViewModel, vm => vm.CurrentTime, v => v.TimeLabel.Content).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.MediaPlayer, v => v.VideoView.MediaPlayer).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.JumpBackCommand, v => v.JumpBackButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.JumpAheadCommand, v => v.JumpAheadButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.JumpToStartCommand, v => v.RestartButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.PlayCommand, v => v.PlayButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.MuteCommand, v => v.MuteButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.PlaySelectionCommand, v => v.PlaySectionButton).DisposeWith(d);
            });
        }
    }
}

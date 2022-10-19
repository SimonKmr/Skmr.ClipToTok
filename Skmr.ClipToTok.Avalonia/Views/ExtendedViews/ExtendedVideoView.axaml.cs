using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Skmr.ClipToTok.ViewModels;
using System.Reactive.Disposables;

namespace Skmr.ClipToTok.Avalonia.Views.ExtendedViews
{
    public partial class ExtendedVideoView : ReactiveWindow<VideoViewModel>
    {
        public CheckBox TimeFrameCheckBox => this.FindControl<CheckBox>("cbTimeFrameEnabled");
        public TextBox TimeFrameStartTextBox => this.FindControl<TextBox>("txtTimeFrameStart");
        public TextBox TimeFrameEndTextBox => this.FindControl<TextBox>("txtTimeFrameDuration");

        public CheckBox SpeedCheckBox => this.FindControl<CheckBox>("cbSpeed");
        public Slider TimeFrameSlider => this.FindControl<Slider>("slSpeed");
        public TextBox SpeedTextBox => this.FindControl<TextBox>("txtSpeed");
        public ExtendedVideoView()
        {
            InitializeComponent();

            this.WhenActivated((d) =>
            {
                this.Bind(ViewModel, vm => vm.HasTimeFrame, v => v.TimeFrameCheckBox.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TimeFrameStart, v => v.TimeFrameStartTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TimeFrameDuration, v => v.TimeFrameEndTextBox.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.HasChangeSpeed, v => v.SpeedCheckBox.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Speed, v => v.TimeFrameSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Speed, v => v.SpeedTextBox.Text).DisposeWith(d);
            });
        }


    }
}

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
        public Utils.UserControls.TimePicker TimePickerStart => this.FindControl<Utils.UserControls.TimePicker>("tpStart");
        public Utils.UserControls.TimePicker TimePickerDuration => this.FindControl<Utils.UserControls.TimePicker>("tpDuration");

        public CheckBox SpeedCheckBox => this.FindControl<CheckBox>("cbSpeed");
        public Slider TimeFrameSlider => this.FindControl<Slider>("slSpeed");
        public TextBox SpeedTextBox => this.FindControl<TextBox>("txtSpeed");
        public ExtendedVideoView()
        {
            InitializeComponent();

            this.WhenActivated((d) =>
            {
                this.Bind(ViewModel, vm => vm.HasTimeFrame, v => v.TimeFrameCheckBox.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TimeFrameStart, v => v.TimePickerStart.SelectedTime).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TimeFrameDuration, v => v.TimePickerDuration.SelectedTime).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.HasChangeSpeed, v => v.SpeedCheckBox.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Speed, v => v.TimeFrameSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Speed, v => v.SpeedTextBox.Text).DisposeWith(d);
            });
        }


    }
}

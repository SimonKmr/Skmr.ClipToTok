using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Skmr.ClipToTok.ViewModels;
using System.Reactive.Disposables;

namespace Skmr.ClipToTok.Avalonia.Views.ExtendedViews
{
    public partial class ExtendedFrameView : ReactiveWindow<FrameViewModel>
    {
        public CheckBox ColorGradingCheckBox => this.FindControl<CheckBox>("cbColorGrading");
        public Slider ContrastSlider => this.FindControl<Slider>("slContrast");
        public Slider BrighnessSlider => this.FindControl<Slider>("slBrightness");
        public Slider SaturationSlider => this.FindControl<Slider>("slSaturation");
        public Slider GammaSlider => this.FindControl<Slider>("slGamma");
        public Slider GammaRSlider => this.FindControl<Slider>("slGammaR");
        public Slider GammaGSlider => this.FindControl<Slider>("slGammaG");
        public Slider GammaBSlider => this.FindControl<Slider>("slGammaB");
        public Slider GammaWeightSlider => this.FindControl<Slider>("slGammaWeight");
        public TextBox ContrastTextBox => this.FindControl<TextBox>("txtContrast");
        public TextBox BrighnessTextBox => this.FindControl<TextBox>("txtBrightness");
        public TextBox SaturationTextBox => this.FindControl<TextBox>("txtSaturation");
        public TextBox GammaTextBox => this.FindControl<TextBox>("txtGamma");
        public TextBox GammaRTextBox => this.FindControl<TextBox>("txtGammaR");
        public TextBox GammaGTextBox => this.FindControl<TextBox>("txtGammaG");
        public TextBox GammaBTextBox => this.FindControl<TextBox>("txtGammaB");
        public TextBox GammaWeightTextBox => this.FindControl<TextBox>("txtGammaWeight");

        public ExtendedFrameView()
        {
            InitializeComponent();
            this.WhenActivated((d) =>
            {
                this.Bind(ViewModel, vm => vm.HasColorGrading, v => v.ColorGradingCheckBox.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Contrast, v => v.ContrastSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Brighness, v => v.BrighnessSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Saturation, v => v.SaturationSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Gamma, v => v.GammaSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaR, v => v.GammaRSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaG, v => v.GammaGSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaB, v => v.GammaBSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaWeight, v => v.GammaWeightSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Contrast, v => v.ContrastTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Brighness, v => v.BrighnessTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Saturation, v => v.SaturationTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Gamma, v => v.GammaTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaR, v => v.GammaRTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaG, v => v.GammaGTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaB, v => v.GammaBTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaWeight, v => v.GammaWeightTextBox.Text).DisposeWith(d);
            });
        }
    }
}

using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Skmr.ClipToTok.Avalonia.Views.ExtendedViews;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class FrameView : ReactiveUserControl<FrameViewModel>
    {
        public TextBox PosXTextBox => this.FindControl<TextBox>("txtPosX");
        public TextBox PosYTextBox => this.FindControl<TextBox>("txtPosY");
        public TextBox WidthTextBox => this.FindControl<TextBox>("txtWidth");
        public TextBox HeightTextBox => this.FindControl<TextBox>("txtHeight");
        public Slider PosXSlider => this.FindControl<Slider>("slPosX");
        public Slider PosYSlider => this.FindControl<Slider>("slPosY");
        public Slider WidthSlider => this.FindControl<Slider>("slWidth");
        public Slider HeightSlider => this.FindControl<Slider>("slHeight");
        public TextBox RedTextBox => this.FindControl<TextBox>("txtRed");
        public TextBox GreenTextBox => this.FindControl<TextBox>("txtGreen");
        public TextBox BlueTextBox => this.FindControl<TextBox>("txtBlue");
        public TextBox AlphaTextBox => this.FindControl<TextBox>("txtAlpha");

        public Button RemoveButton => this.FindControl<Button>("btnRemove");
        public Button AttributesButton => this.FindControl<Button>("btnAttributes");

        public FrameView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                ViewModel!.AttributesWindow.RegisterHandler(DoShowAttributesWindow);
                this.Bind(ViewModel, vm => vm.PosX, v => v.PosXTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.PosY, v => v.PosYTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Width, v => v.WidthTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Height, v => v.HeightTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.PosX, v => v.PosXSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.PosY, v => v.PosYSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Width, v => v.WidthSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Height, v => v.HeightSlider.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Red, v => v.RedTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Green, v => v.GreenTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Blue, v => v.BlueTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Alpha, v => v.AlphaTextBox.Text).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.RemoveCommand, v => v.btnRemove).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.OpenAttributesCommand, v => v.btnAttributes).DisposeWith(d);
            });
        }
        //https://docs.avaloniaui.net/tutorials/music-store-app/opening-a-dialog
        //https://docs.avaloniaui.net/docs/controls/window
        private async Task DoShowAttributesWindow(InteractionContext<FrameViewModel,object?> interaction)
        {
            var window = new ExtendedFrameView();
            window.DataContext = interaction.Input;
            var result = await window.ShowDialog<object?>(MainWindow.Instance);
            interaction.SetOutput(result);
        }

    }
}

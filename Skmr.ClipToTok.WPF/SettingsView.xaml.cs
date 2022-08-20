using ReactiveUI;
using Skmr.ClipToTok.ViewModels;
using System.Reactive.Disposables;
using System.Windows;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ReactiveUserControl<SettingsViewModel>, IViewFor<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.VideoFile, v => v.txtSourceVideo.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Resolution, v => v.txtResolution.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ResultFolder, v => v.txtResFolder.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.HasTimeFrame, v => v.cbTimeFrameEnabled.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TimeFrameStart, v => v.txtTimeFrameStart.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TimeFrameDuration, v => v.txtTimeFrameDuration.Text).DisposeWith(d);
                
                this.Bind(ViewModel, vm => vm.HasChangeSpeed, v => v.cbSpeed.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Speed, v => v.slSpeed.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Speed, v => v.txtSpeed.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.ScreenPosGameplay.PosX, v => v.slGameplayPosX.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosGameplay.PosX, v => v.txtGameplayPosX.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosGameplay.PosY, v => v.slGameplayPosY.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosGameplay.PosY, v => v.txtGameplayPosY.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosGameplay.Width, v => v.slGameplayWidth.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosGameplay.Width, v => v.txtGameplayWidth.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosGameplay.Height, v => v.slGameplayHeight.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosGameplay.Height, v => v.txtGameplayHeight.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.HasWebcam, v => v.cbWebcam.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosWebcam.PosX, v => v.slWebcamPosX.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosWebcam.PosX, v => v.txtWebcamPosX.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosWebcam.PosY, v => v.slWebcamPosY.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosWebcam.PosY, v => v.txtWebcamPosY.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosWebcam.Width, v => v.slWebcamWidth.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosWebcam.Width, v => v.txtWebcamWidth.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosWebcam.Height, v => v.slWebcamHeight.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ScreenPosWebcam.Height, v => v.txtWebcamHeight.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.HasBackground, v => v.cbBackground.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BackgroundImage, v => v.txtBackgroundImage.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.HasColorGrading, v => v.cbColorGrading.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Contrast, v => v.txtContrast.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Contrast, v => v.slContrast.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Brighness, v => v.txtBrightness.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Brighness, v => v.slBrightness.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Saturation, v => v.txtSaturation.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Saturation, v => v.slSaturation.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Gamma, v => v.txtGamma.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Gamma, v => v.slGamma.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaR, v => v.txtGammaR.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaR, v => v.slGammaR.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaG, v => v.txtGammaG.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaG, v => v.slGammaG.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaB, v => v.txtGammaB.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaB, v => v.slGammaB.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaWeight, v => v.txtGammaWeight.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.GammaWeight, v => v.slGammaWeight.Value).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.LoadCommand, v => v.btnLoad).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.btnSave).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.RenderCommand, v => v.btnRender).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.NewCommand, v => v.btnNew).DisposeWith(d);
            });
        }

        #region Drag and Drops
        private void SourceVideo_Drop(object sender, DragEventArgs e)
        {
            txtSourceVideo.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void ResultFolder_Drop(object sender, DragEventArgs e)
        {
            txtResFolder.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void BackgroundImage_Drop(object sender, DragEventArgs e)
        {
            txtBackgroundImage.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }
        #endregion
    }
}

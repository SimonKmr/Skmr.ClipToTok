using ReactiveUI;
using Skmr.ClipToTok.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

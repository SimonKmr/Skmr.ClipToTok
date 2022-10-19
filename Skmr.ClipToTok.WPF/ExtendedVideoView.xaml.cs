using ReactiveUI;
using Skmr.ClipToTok.ViewModels;
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
using System.Windows.Shapes;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für ExtendedVideoView.xaml
    /// </summary>
    public partial class ExtendedVideoView : ReactiveWindow<VideoViewModel>
    {
        public ExtendedVideoView()
        {
            this.WhenActivated((d) =>
            {
                this.Bind(ViewModel, vm => vm.HasTimeFrame, v => v.cbTimeFrameEnabled.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TimeFrameStart, v => v.txtTimeFrameStart.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TimeFrameDuration, v => v.txtTimeFrameDuration.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.HasChangeSpeed, v => v.cbSpeed.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Speed, v => v.slSpeed.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Speed, v => v.txtSpeed.Text).DisposeWith(d);
            });

            InitializeComponent();
        }
    }
}

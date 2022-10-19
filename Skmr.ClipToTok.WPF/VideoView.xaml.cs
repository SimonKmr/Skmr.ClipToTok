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
using ReactiveUI;
using Skmr.ClipToTok.ViewModels;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für VideoView.xaml
    /// </summary>
    public partial class VideoView : ReactiveUserControl<VideoViewModel>
    {
        public VideoView()
        {
            this.WhenActivated((d) =>
            {
                this.OneWayBind(ViewModel, vm => vm.ScreenPositions, v => v.icScreenPositions.ItemsSource).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.VideoFile, v => v.txtSourceVideo.Text).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.AddFrameCommand, v => v.btnAddFrame).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.OpenAttributesCommand, v => v.btnAttributes).DisposeWith(d);

                ViewModel.RegisterWindow(NewWindowRequest);
            });
            InitializeComponent();
        }

        private void SourceVideo_Drop(object sender, DragEventArgs e)
        {
            txtSourceVideo.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void NewWindowRequest(object sender, EventArgs e)
        {
            ExtendedVideoView window = new ExtendedVideoView()
            {
                ViewModel = this.ViewModel,
            };
            window.Show();
        }
    }
}

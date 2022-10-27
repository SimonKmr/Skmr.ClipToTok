using ReactiveUI;
using Skmr.ClipToTok.ViewModels.ClipToTok;
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
    /// Interaktionslogik für ScreenPositionView.xaml
    /// </summary>
    public partial class FrameView : ReactiveUserControl<FrameViewModel>
    {
        public FrameView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.PosX, v => v.slPosX.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.PosX, v => v.txtPosX.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.PosY, v => v.slPosY.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.PosY, v => v.txtPosY.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Width, v => v.slWidth.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Width, v => v.txtWidth.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Height, v => v.slHeight.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Height, v => v.txtHeight.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Red, v => v.txtRed.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Green, v => v.txtGreen.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Blue, v => v.txtBlue.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Alpha, v => v.txtAlpha.Text).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.RemoveCommand, v => v.btnRemove).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.OpenAttributesCommand, v => v.btnAttributes).DisposeWith(d);
                ViewModel.RegisterWindow(NewWindowRequest);
            });
        }


        private void NewWindowRequest(object sender, EventArgs e)
        {
            ExtendedFrameView window = new ExtendedFrameView()
            {
                ViewModel = this.ViewModel,
            };
            window.Show();
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für HighlightView.xaml
    /// </summary>
    public partial class HighlightView : ReactiveUserControl<HighlightViewModel>
    {
        public HighlightView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.DurationText, v => v.txtDuration.Content).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Start, v => v.txtStart.Content).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.End, v => v.txtEnd.Content).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Comment, v => v.txtTitle.Content).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.PlayCommand, v => v.btnPlay).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.SelectCommand, v => v.btnSelect).DisposeWith(d);
            });

            imgPreview.Source = new BitmapImage(new Uri(@"C:\Users\darkf\OneDrive\Bilder\Screenshot_1.png"));
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 420)
            {
                grdBody.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                grdBody.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                grdBody.ColumnDefinitions[1].Width = new GridLength(210, GridUnitType.Pixel);
                grdBody.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
            }
        }
    }
}

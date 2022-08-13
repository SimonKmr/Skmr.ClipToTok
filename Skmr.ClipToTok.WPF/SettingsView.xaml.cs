using ReactiveUI;
using Skmr.ClipToTok.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
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

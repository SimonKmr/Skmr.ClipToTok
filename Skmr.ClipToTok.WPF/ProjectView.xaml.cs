using ReactiveUI;
using Skmr.ClipToTok.ViewModels.ClipToTok;
using System.Reactive.Disposables;
using System.Windows;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für SettingsView.xaml
    /// </summary>
    public partial class ProjectView : ReactiveUserControl<ProjectViewModel>, IViewFor<ProjectViewModel>
    {
        

        public ProjectView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Video, v => v.vvVideo.ViewModel).DisposeWith(d);
                

                this.Bind(ViewModel, vm => vm.ResultFolder, v => v.txtResFolder.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.HasBackground, v => v.cbBackground.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BackgroundImage, v => v.txtBackgroundImage.Text).DisposeWith(d);
                

                this.BindCommand(ViewModel, vm => vm.LoadCommand, v => v.btnLoad).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.btnSave).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.RenderCommand, v => v.btnRender).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.NewCommand, v => v.btnNew).DisposeWith(d);

            });
        }

        #region Drag and Drops
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

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Skmr.ClipToTok.Utility;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace Skmr.ClipToTok.Avalonia.Views
{
    public partial class ProjectView : ReactiveUserControl<ProjectViewModel>
    {
        public VideoView VideoView => this.FindControl<VideoView>("vvVideo");
        public CheckBox BackgroundImageCheckBox => this.FindControl<CheckBox>("cbBackground");
        public TextBox ResultFolderTextBox => this.FindControl<TextBox>("txtResFolder");
        public TextBox ResolutionTextBox => this.FindControl<TextBox>("txtResolution");
        public TextBox BackgroundImageTextBox => this.FindControl<TextBox>("txtBackgroundImage");
        public Button LoadButton => this.FindControl<Button>("btnLoad");
        public Button SaveButton => this.FindControl<Button>("btnSave");
        public Button NewButton => this.FindControl<Button>("btnNew");
        public Button RenderButton => this.FindControl<Button>("btnRender");

        public Border ResultDropPanel => this.FindControl<Border>("dpResultFolder");
        public Border BackgroundImageDropPanel => this.FindControl<Border>("dpBackgroundImage");


        public ProjectView()
        {

            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.SaveButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.LoadCommand, v => v.LoadButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.NewCommand, v => v.NewButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.RenderCommand, v => v.RenderButton).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.ResultFolder, v => v.ResultFolderTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Resolution, v => v.ResolutionTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.BackgroundImage, v => v.BackgroundImageTextBox.Text).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.HasBackground, v => v.BackgroundImageCheckBox.IsChecked).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Video, v => v.VideoView.DataContext).DisposeWith(d);
            });
            ResultDropPanel.AddHandler(DragDrop.DropEvent, DropResult);
            BackgroundImageDropPanel.AddHandler(DragDrop.DropEvent, DropBackground);

            Interactions.OpenFileDialog.RegisterHandler(
                async interaction =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Filters.Add(GetFileDialogFilter());
                    var result = await fileDialog.ShowAsync(MainWindow.Instance);
                    if (result != null)
                    {
                        interaction.SetOutput(result[0]);
                    }
                    else
                    {
                        interaction.SetOutput(String.Empty);
                    }
                });

            Interactions.SaveFileDialog.RegisterHandler(
                async interaction =>
                {
                    SaveFileDialog fileDialog = new SaveFileDialog();
                    fileDialog.Filters.Add(GetFileDialogFilter()); 
                    var result = await fileDialog.ShowAsync(MainWindow.Instance);
                    if (result != null)
                    {
                        interaction.SetOutput(result);
                    }
                    else
                    {
                        interaction.SetOutput(String.Empty);
                    }
                });

        }

        private FileDialogFilter GetFileDialogFilter()
        {
            List<string> jsonExtentions = new List<string>();
            jsonExtentions.Add("json");
            return new FileDialogFilter() { Name = "Json File", Extensions = jsonExtentions };
        }

        private void DropResult(object sender, DragEventArgs e)
        {
            var s = sender as Border;

                if (e.Data.Contains(DataFormats.FileNames))
                {
                    ResultFolderTextBox.Text = e.Data.GetFileNames().ToArray()[0];
                }

        }

        private void DropBackground(object sender, DragEventArgs e)
        {
            var s = sender as Border;

            if (e.Data.Contains(DataFormats.FileNames))
            {
                BackgroundImageTextBox.Text = e.Data.GetFileNames().ToArray()[0];
            }

        }
    }
}

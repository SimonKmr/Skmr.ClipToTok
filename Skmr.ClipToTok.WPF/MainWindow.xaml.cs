using LibVLCSharp.Shared;
using Microsoft.Win32;
using ReactiveUI;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using Skmr.ClipToTok.Utility;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

            //MainViewModel
            var vm = new MainViewModel();
            ViewModel = vm;

            var player = new PlayerViewModel();
            playerPreview.ViewModel = player;

            //Bindings
            this.WhenActivated(disposables =>
            {   
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.PlayerViewModel, x => x.playerPreview.ViewModel).DisposeWith(disposables);
                
                this.BindCommand(ViewModel, x => x.GoClipToTok, x => x.SettingsTabButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.GoHighlighter, x => x.HighlighterTabButton).DisposeWith(disposables);
            });

            Interactions.OpenFileDialog.RegisterHandler(
                async interaction =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "json file (*.json)|*.json|text file (*.txt)|*.txt";
                    var result = fileDialog.ShowDialog();
                    if(result == true)
                    {
                        interaction.SetOutput(fileDialog.FileName);
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
                    fileDialog.Filter = "json file (*.json)|*.json";
                    var result = fileDialog.ShowDialog();
                    if (result == true)
                    {
                        interaction.SetOutput(fileDialog.FileName);
                    }
                    else
                    {
                        interaction.SetOutput(String.Empty);
                    }
                });
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = e.Uri.AbsoluteUri;
            Process.Start(psi);
            e.Handled = true;
        }
    }
}
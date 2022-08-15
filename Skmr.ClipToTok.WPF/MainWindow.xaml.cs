using LibVLCSharp.Shared;
using ReactiveUI;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Navigation;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private SettingsViewModel svm;
        public MainWindow()
        {
            InitializeComponent();

            //MainViewModel
            var vm = new MainViewModel();
            svm = vm.Svm;
            ViewModel = vm;
            vm.Settings.ScreenPosWebcam.OnScreenPosChanged += ScreenPosWebcam_OnScreenPosChanged;
            vm.Settings.ScreenPosGameplay.OnScreenPosChanged += ScreenPosGameplay_OnScreenPosChanged;

            //Vlc Player
            videoView.Loaded += VideoView_Loaded;

            //Bindings
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.GoSettings, x => x.SettingsTabButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.GoHighlighter, x => x.HighlighterTabButton).DisposeWith(disposables);
            });
        }


        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;

        void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            videoView.MediaPlayer = _mediaPlayer;
        }

        private void ScreenPosWebcam_OnScreenPosChanged(int x, int y, int width, int height)
        {
            this.xWebc = x;
            this.yWebc = y;
            this.widthWebc = width;
            this.heightWebc = height;
            canvasView.InvalidateVisual();
        }

        int xWebc;
        int yWebc;
        int widthWebc;
        int heightWebc;

        private void ScreenPosGameplay_OnScreenPosChanged(int x, int y, int width, int height)
        {
            this.xGamep = x;
            this.yGamep = y;
            this.widthGamep = width;
            this.heightGamep = height;
            canvasView.InvalidateVisual();
        }

        int xGamep;
        int yGamep;
        int widthGamep;
        int heightGamep;

        private void SKElement_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var z = canvasView.CanvasSize;
            float y1 = z.Height / 1080f;
            float x1 = z.Width / 1920f;
            //var image = SKImage.FromEncodedData(@"C:\Users\darkf\Documents\Lightshot\Screenshot_2.png");
            //var bm = SKBitmap.FromImage(image);
            
            var dbm = new SKBitmap((int)z.Width, (int)z.Height);
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColor.FromHsv(0,0,0,0));
            canvas.DrawBitmap(dbm, new SKPoint(0,0));
            
            canvas.DrawRect(xWebc * x1,yWebc * y1, widthWebc * x1, heightWebc * y1, new SKPaint() { Color = new SKColor(255, 0, 0,122) });
            canvas.DrawRect(xGamep * x1, yGamep * y1, widthGamep * x1, heightGamep * y1, new SKPaint() { Color = new SKColor(0, 255, 0, 122) });
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

        private bool isPlaying = false;
        private string playedBackVideo = String.Empty;
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(svm.VideoFile))
            {
                if (!playedBackVideo.Equals(svm.VideoFile))
                {
                    _mediaPlayer.Play(new Media(_libVLC, new Uri(svm.VideoFile)));
                    playedBackVideo = svm.VideoFile;
                    isPlaying = true;
                }
                else if(!isPlaying && playedBackVideo.Equals(svm.VideoFile))
                {
                    _mediaPlayer.Play();
                    isPlaying = true;
                }
                else
                {
                    _mediaPlayer.Pause();
                }
            } 
            else
            {
                _mediaPlayer.Stop();
            }

        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var normalizedWidth = e.NewSize.Width / 16d;
            var normalizedHeight = e.NewSize.Height / 9d;
            if (normalizedHeight > normalizedWidth)
            {
                var height = videoView.ActualWidth * 9d / 16d;
                var width = videoView.ActualWidth;
                canvasView.Height = height;
                canvasView.Width = width;
            }
            else
            {
                var height = videoView.ActualHeight;
                var width = videoView.ActualHeight * 16d / 9d;
                canvasView.Height = height;
                canvasView.Width = width;
            }
        }
    }
}
using LibVLCSharp.Shared;
using ReactiveUI;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using Skmr.ClipToTok.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaktionslogik für PlayerView.xaml
    /// </summary>
    public partial class PlayerView : ReactiveUserControl<PlayerViewModel>
    {
        public PlayerView()
        {
            InitializeComponent();

            //Vlc Player
            videoView.Loaded += VideoView_Loaded;
            //Drawing


            //Bindings
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.MediaPlayer, v => v.videoView.MediaPlayer).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.PlayCommand, v => v.btnPlay).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.MuteCommand, v => v.btnMute).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.JumpAheadCommand, v => v.btnJumpAhead).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.JumpBackCommand, v => v.btnJumpBack).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.JumpToStartCommand, v => v.btnJumpToStart).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.PlaySelectionCommand, v => v.btnPlaySelection).DisposeWith(disposables);

                ViewModel.Webcam.OnScreenPosChanged += Webcam_OnScreenPosChanged;
                ViewModel.Gameplay.OnScreenPosChanged += Gameplay_OnScreenPosChanged;
            });
        }

        private void Gameplay_OnScreenPosChanged(int x, int y, int width, int height)
        {
            DrawGameplayArea(x, y, width, height);
        }

        private void Webcam_OnScreenPosChanged(int x, int y, int width, int height)
        {
            DrawWebcamArea(x, y, width, height);
        }

        void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            Core.Initialize();
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

        #region Skia - Video Part Selection
        int xWebc;
        int yWebc;
        int widthWebc;
        int heightWebc;

        int xGamep;
        int yGamep;
        int widthGamep;
        int heightGamep;
        public void DrawGameplayArea(int x, int y, int width, int height)
        {
            this.xGamep = x;
            this.yGamep = y;
            this.widthGamep = width;
            this.heightGamep = height;
            canvasView.InvalidateVisual();
        }

        public void DrawWebcamArea(int x, int y, int width, int height)
        {
            this.xWebc = x;
            this.yWebc = y;
            this.widthWebc = width;
            this.heightWebc = height;
            canvasView.InvalidateVisual();
        }


        private void SKElement_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var z = canvasView.CanvasSize;
            float y1 = z.Height / 1080f;
            float x1 = z.Width / 1920f;
            //var image = SKImage.FromEncodedData(@"C:\Users\darkf\Documents\Lightshot\Screenshot_2.png");
            //var bm = SKBitmap.FromImage(image);

            var dbm = new SKBitmap((int)z.Width, (int)z.Height);
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColor.FromHsv(0, 0, 0, 0));
            canvas.DrawBitmap(dbm, new SKPoint(0, 0));

            canvas.DrawRect(xWebc * x1, yWebc * y1, widthWebc * x1, heightWebc * y1, new SKPaint() { Color = new SKColor(255, 0, 0, 122) });
            canvas.DrawRect(xGamep * x1, yGamep * y1, widthGamep * x1, heightGamep * y1, new SKPaint() { Color = new SKColor(0, 255, 0, 122) });
        }
        #endregion
    }
}

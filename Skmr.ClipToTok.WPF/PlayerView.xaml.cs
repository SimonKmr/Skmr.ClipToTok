using LibVLCSharp.Shared;
using ReactiveUI;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using Skmr.ClipToTok.Utility;
using Skmr.ClipToTok.ViewModels;
using Skmr.ClipToTok.ViewModels.ClipToTok;
using System.Reactive.Disposables;
using System.Windows;

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
            
            //Bindings
            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.MediaPlayer, v => v.videoView.MediaPlayer).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.CurrentTime, v => v.lbCurrentTime.Content).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.PlayCommand, v => v.btnPlay).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.MuteCommand, v => v.btnMute).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.JumpAheadCommand, v => v.btnJumpAhead).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.JumpBackCommand, v => v.btnJumpBack).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.JumpToStartCommand, v => v.btnJumpToStart).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.PlaySelectionCommand, v => v.btnPlaySelection).DisposeWith(d);

                ViewModelBus.SettingsViewModel.Video.OnFramesChanged += SettingsViewModel_OnFramesChanged;
            });
        }

        private void SettingsViewModel_OnFramesChanged(object sender, System.EventArgs e)
        {
            var s = sender as VideoViewModel;
            var sp = new FrameViewModel[s.ScreenPositions.Count];
            s.ScreenPositions.CopyTo(sp, 0);
            DrawArea(sp);
        }

        private void OnScreenPosChanged(int x, int y, int width, int height)
        {

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
        FrameViewModel[] _areas;
        public void DrawArea(FrameViewModel[] areas)
        {
            _areas = areas;
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

            if(_areas != null)
            {
                foreach(var area in _areas)
                {
                    canvas.DrawRect(area.PosX * x1, area.PosY * y1, area.Width * x1, area.Height * y1, new SKPaint() { Color = new SKColor(area.Red, area.Green, area.Blue, area.Alpha) });
                }
            }


        }
        #endregion
    }
}

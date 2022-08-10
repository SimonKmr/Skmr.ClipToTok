using ReactiveUI;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using Skmr.ClipToTok.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace Skmr.ClipToTok.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            DataContext = vm;
            vm.Settings.ScreenPosWebcam.OnScreenPosChanged += ScreenPosWebcam_OnScreenPosChanged;
            vm.Settings.ScreenPosGameplay.OnScreenPosChanged += ScreenPosGameplay_OnScreenPosChanged;
            canvasView.SizeChanged += CanvasView_SizeChanged;
        }



        private void CanvasView_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var element = sender as SKElement;
            element.Height = e.NewSize.Width * 9f / 16f;
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

        private void SourceVideo_Drop(object sender, System.Windows.DragEventArgs e)
        {
            txtSourceVideo.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void PreviewImage_Drop(object sender, DragEventArgs e)
        {
            txtPrevImage.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void ResultFolder_Drop(object sender, DragEventArgs e)
        {
            txtResFolder.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void BackgroundImage_Drop(object sender, DragEventArgs e)
        {
            txtBackgroundImage.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
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
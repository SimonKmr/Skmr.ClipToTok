using ReactiveUI;
using Skmr.Editor;
using Skmr.Editor.Instructions;
using Skmr.Editor.Instructions.Interfaces;
using Skmr.Editor.Media;
using System.Windows.Input;
using static Skmr.Editor.Ffmpeg;
using SkiaSharp;

namespace Skmr.ClipToTok.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            Settings = new SettingsViewModel();

            RenderCommand = ReactiveCommand.Create(RenderThreaded);
        }

        private SettingsViewModel _Settings;
        public SettingsViewModel Settings 
        { 
            get { return _Settings; }
            set { this.RaiseAndSetIfChanged(ref _Settings, value); } 
        }

        public ICommand RenderCommand { get; set; }

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set { this.RaiseAndSetIfChanged(ref _Image, value); }
        }


        public delegate void LogHandler(string line);
        public event LogHandler OnLog = delegate { };
        private void AddLogEntry(string line)
        {
            OnLog(line);
            Log += $"{DateTime.Now.ToString("hh:mm:ss.FFF")}    {line}\n";
        }
        public string _Log;
        public string Log
        {
            get { return _Log; }
            set { this.RaiseAndSetIfChanged(ref _Log, value); }
        }

        
        Thread renderThread;
        public void RenderThreaded()
        {
            if(renderThread != null)
            {
                if (renderThread.IsAlive)
                {
                    AddLogEntry("already Rendering!");
                    return;
                }
            }

            renderThread = new Thread(Render);
            renderThread.Start();
        }
        public void Render()
        {
            #region Setup
            AddLogEntry("Rendering started!");
            //if (Environment.GetCommandLineArgs().Length > 2)
            //{
            //    settingsManager.LoadJson(Environment.GetCommandLineArgs()[2]);
            //}


            Ffmpeg ffmpeg = new Ffmpeg();
            ffmpeg.HideBanner = true;
            ffmpeg.HideWindow = true;
            ffmpeg.OverrideState = Overwrite.overwriting;
            ffmpeg.LogLevel = logLevel.info;


            TmpDirectoryManager tdm = new TmpDirectoryManager();
            tdm.Add();
            Directory.CreateDirectory("Input");
            //Directory.CreateDirectory(Settings.ResultFolder);




            List<Medium> composition = new List<Medium>();
            List<IInstruction> instructions = new List<IInstruction>();


            Medium result = new Medium(
                Settings.ResultFolder,
                DateTime.Now.Ticks.ToString(),
                Ffmpeg.Format.Video.Mp4);

            var videoFolder = Path.GetDirectoryName(Settings.Video);
            var videoName = Path.GetFileName(Settings.Video).Split('.')[0];

            Medium Clip = new Medium(
                videoFolder,
                videoName,
                Ffmpeg.Format.Video.Mp4);
            #endregion


            #region Ffmpeg - Normalizing
            Medium ClipNormalized = new Medium(
                tdm.TmpDirectoryList[0],
                "normalized",
                Ffmpeg.Format.Video.Mp4);

            int height1 = Settings.Resolution;
            if (height1 % 2 != 0) height1--;
            int width1 = (int)(((double)1920 / (double)1080) * (double)Settings.Resolution);
            instructions.Add(new ResizeVideo(width1, height1)
            {
                Ffmpeg = ffmpeg,
                Input = Clip,
                Output = ClipNormalized
            });
            #endregion

            #region Ffmpeg - Speedup
            #endregion

            #region Ffmpeg - Webcam
            Medium Webcam = new Medium(
                tdm.TmpDirectoryList[0],
                "webcam",
                Ffmpeg.Format.Video.Mp4);

            Medium tmp1 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            if (Settings.HasWebcam)
            {
                instructions.Add(new CropVideo(
                    Settings.ScreenPosWebcam.Width,
                    Settings.ScreenPosWebcam.Height,
                    Settings.ScreenPosWebcam.PosX,
                    Settings.ScreenPosWebcam.PosY)
                {
                    Ffmpeg = ffmpeg,
                    Input = ClipNormalized,
                    Output = tmp1,
                });

                int h1 = (int)(Settings.Resolution * ((double)Settings.ScreenPosWebcam.Height / (double)Settings.ScreenPosWebcam.Width));
                if (h1 % 2 == 1) h1--;

                instructions.Add(new ResizeVideo(Settings.Resolution, h1)
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp1,
                    Output = Webcam,
                });

                composition.Add(Webcam);
            }
            #endregion

            #region Ffmpeg - Gameplay
            Medium GameplayArea = new Medium(
                tdm.TmpDirectoryList[0],
                "gameplay",
                Ffmpeg.Format.Video.Mp4);

            Medium tmp2 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            instructions.Add(new CropVideo(
                Settings.ScreenPosGameplay.Width,
                Settings.ScreenPosGameplay.Height,
                Settings.ScreenPosGameplay.PosX,
                Settings.ScreenPosGameplay.PosY)
            {
                Ffmpeg = ffmpeg,
                Input = ClipNormalized,
                Output = tmp2,
            });

            int h2 = (int)((double)Settings.Resolution * ((double)Settings.ScreenPosGameplay.Height / (double)Settings.ScreenPosGameplay.Width));
            if (h2 % 2 == 1) h2--;

            instructions.Add(new ResizeVideo(Settings.Resolution, h2)
            {
                Ffmpeg = ffmpeg,
                Input = tmp2,
                Output = GameplayArea
            });

            composition.Add(GameplayArea);
            #endregion

            #region Ffmpeg - VStack
            Medium tmp3 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            composition.Reverse();
            if (composition.Count > 1)
            {

                instructions.Add(new VStack()
                {
                    Input = composition.ToArray(),
                    Output = tmp3,
                    Ffmpeg = ffmpeg
                });
            }
            else
            {
                tmp3 = composition[0];
            }
            #endregion

            #region Ffmpeg - Background
            Medium tmp4 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            if (Settings.HasBackground)
            {
                var backgroundImgPath = Path.GetDirectoryName(Settings.BackgroundImage);
                var backgroundImgFile = Path.GetFileName(Settings.BackgroundImage).Split('.')[0];

                instructions.Add(new BackgroundImage()
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp3,
                    Image = new Medium(
                        backgroundImgPath,
                        backgroundImgFile,
                        Format.Image.Jpg),
                    Output = tmp4
                });
            }
            else
            {
                tmp4 = tmp3;
            }
            #endregion

            #region Ffmpeg - TextBubble
            //Medium tmp5 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            //if (Settings.Settings.TextBubble.IsEnabled)
            //{
            //    // PosX = (1080 - 750) / 2,
            //    // PosY = (1920 - 125) / 2 + 130,
            //    var s = Settings.Settings.TextBubble.Value;
            //    var b = new Plain(
            //        s.Position.Width,
            //        s.Position.Height);
            //    b.CreateGradientSolid(
            //        Color.FromArgb(
            //            s.Color1.r,
            //            s.Color1.g,
            //            s.Color1.b),
            //        Color.FromArgb(
            //            s.Color2.r,
            //            s.Color2.g,
            //            s.Color2.b)); //nice blue Color.FromArgb(0, 151, 255) Color.FromArgb(0, 173, 173)
            //    b.CreateTextBubble(s.Text);
            //    b.CreateRoundedCorners(s.CornerRadius);
            //    b.Save($"{tdm.TmpDirectoryList[0]}\\overlay.png");

            //    instructions.Add(new OverlayImage()
            //    {
            //        PosX = s.Position.PosX,
            //        PosY = s.Position.PosY,
            //        Ffmpeg = ffmpeg,
            //        Input = tmp4,
            //        Overlay = new Medium(
            //        tdm.TmpDirectoryList[0],
            //        "overlay",
            //        Format.Image.Png),
            //        Output = tmp5
            //    });
            //}
            #endregion

            #region Ffmpeg - Intro
            //Medium tmp6 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            //var intro = new Medium("Input", "Intro", Format.Video.Avi);

            //new ConcatVideos()
            //{
            //    Ffmpeg = ffmpeg,
            //    Input = new Medium[] { tmp5, intro },
            //    Output = tmp6,
            //}.Execute();
            #endregion

            #region Ffmpeg - Outro
            //var outro = new Medium("Input", "Intro", Format.Video.Avi);

            //new ConcatVideos()
            //{
            //    Ffmpeg = ffmpeg,
            //    Input = new Medium[] { outro, tmp6 },
            //    Output = Result,
            //}.Execute();
            #endregion


            #region Execution
            instructions[instructions.Count - 1].Output = result;
            foreach (var i in instructions)
            {
                AddLogEntry( i.GetType().ToString() );
                AddLogEntry( i.Output.Path );
                i.Execute();
            }
            AddLogEntry("Rendering finished!");
            tdm.Clear();
            #endregion
        }
    }
}


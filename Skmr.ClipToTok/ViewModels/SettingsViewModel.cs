using System.Windows.Input;
using Newtonsoft.Json;
using ReactiveUI;
using Skmr.Editor.Instructions;
using Skmr.Editor.Media;
using Skmr.Editor;
using static Skmr.Editor.Ffmpeg;
using Skmr.Editor.Instructions.Interfaces;
using System.IO;
using Skmr.ClipToTok.Utility;
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;

namespace Skmr.ClipToTok.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }

        public SettingsViewModel()
        {
            ScreenPosWebcam = new ScreenPosViewModel();
            ScreenPosGameplay = new ScreenPosViewModel();
            
            NewCommand = ReactiveCommand.Create(New);
            SaveCommand = ReactiveCommand.Create(SaveAsync);
            LoadCommand = ReactiveCommand.Create(LoadAsync);
            RenderCommand = ReactiveCommand.Create(RenderThreaded);
        }

        #region Video
        [Reactive]
        public int Resolution { get; set; } = 1080;
        
        [Reactive]
        public string ResultFolder { get; set; }

        [Reactive]
        public string VideoFile { get; set; }
        #endregion

        #region TimeFrame

        [Reactive]
        public bool HasTimeFrame { get; set; }

        [Reactive]
        public TimeSpan TimeFrameStart { get; set; }

        [Reactive]
        public TimeSpan TimeFrameDuration { get; set; }
        #endregion

        #region Background
        [Reactive]
        public bool HasBackground { get; set; }

        [Reactive]
        public string BackgroundImage { get; set; }
        #endregion

        #region Webcam
        [Reactive]
        public bool HasWebcam { get; set; }

        [Reactive]
        public ScreenPosViewModel ScreenPosWebcam { get; set; }
        #endregion

        #region ColorGrading
        [Reactive]
        public bool HasColorGrading { get; set; }




        [Reactive]
        public float Contrast { get; set; } = 1;

        [Reactive]
        public float Brighness { get; set; } = 0;

        [Reactive]
        public float Saturation { get; set; } = 1;


        [Reactive]
        public float Gamma { get; set; } = 1;

        [Reactive]
        public float GammaR { get; set; } = 1f;

        [Reactive]
        public float GammaG { get; set; } = 1f;

        [Reactive]
        public float GammaB { get; set; } = 1f;

        [Reactive]
        public float GammaWeight { get; set; } = 1f;
        #endregion

        #region ChangeSpeed
        [Reactive]
        public bool HasChangeSpeed { get; set; }

        [Reactive]
        public double Speed { get; set; } = 1;
        #endregion

        #region Gameplay
        private ScreenPosViewModel _ScreenPosGameplay;
        public ScreenPosViewModel ScreenPosGameplay
        {
            get { return _ScreenPosGameplay; }
            set { this.RaiseAndSetIfChanged(ref _ScreenPosGameplay, value); }
        }
        #endregion


        //Events
        public delegate void ScreenPosHandler(object sender, int posX, int posY, int width, int height);
        public delegate void RenderEventHandler(object sender, string e);
        public delegate void StringHandler(object sender, string str);

        public event RenderEventHandler OnRenderEvent = delegate { };


        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand NewCommand { get; set; }
        public ICommand RenderCommand { get; set; }


        public void New()
        {
            Resolution = 1080;
            ResultFolder = String.Empty;

            ScreenPosGameplay.PosX = 0;
            ScreenPosGameplay.PosY = 0;
            ScreenPosGameplay.Width = 0;
            ScreenPosGameplay.Height = 0;

            HasWebcam = false;
            ScreenPosWebcam.PosX = 0;
            ScreenPosWebcam.PosY = 0;
            ScreenPosWebcam.Width = 0;
            ScreenPosWebcam.Height = 0;

            HasChangeSpeed = false;
            Speed = 1;

            HasColorGrading = false;
            Contrast = 1;
            Brighness = 0;
            Saturation = 1;
            Gamma = 1;
            GammaR = 1;
            GammaG = 1;
            GammaB = 1;
            GammaWeight = 1;

            HasBackground = false;
            BackgroundImage = String.Empty;
        }
        public async Task SaveAsync()
        {
            var dialogResult = await Interactions.SaveFileDialog.Handle(String.Empty);

            using (StreamWriter sw = new StreamWriter(dialogResult))
            {
                var data = this.ToSettings();
                sw.Write(JsonConvert.SerializeObject(data));
            }
        }
        public async Task LoadAsync()
        {
            var dialogResult = await Interactions.OpenFileDialog.Handle(String.Empty);

            using (StreamReader sr = new StreamReader(dialogResult))
            {
                Settings loaded = JsonConvert.DeserializeObject<Settings>(sr.ReadToEnd());
                loaded.LoadInto(this);
            }
        }


        
        

        Thread renderThread;
        
        public void RenderThreaded()
        {
            if (renderThread != null)
            {
                if (renderThread.IsAlive)
                {
                    OnRenderEvent(this, "already Rendering!");
                    return;
                }
            }

            renderThread = new Thread(Render);
            renderThread.Start();
        }
        public void Render()
        {
            #region Setup
            OnRenderEvent(this, "Rendering started!");
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
                ResultFolder,
                DateTime.Now.Ticks.ToString(),
                Ffmpeg.Format.Video.Mp4);

            var videoFolder = Path.GetDirectoryName(VideoFile);
            var videoName = Path.GetFileName(VideoFile).Split('.')[0];

            Medium footage = new Medium(
                videoFolder,
                videoName,
                Ffmpeg.Format.Video.Mp4);
            #endregion

            #region Ffmpeg - Cutting
            Medium tmp0 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            if (HasTimeFrame)
            {
                instructions.Add(new CutVideo((TimeFrameStart, TimeFrameDuration))
                {
                    Ffmpeg = ffmpeg,
                    Input = footage,
                    Output = tmp0
                });
            }
            else
            {
                tmp0 = footage;
            }
            #endregion

            #region Ffmpeg - Normalizing
            Medium ClipNormalized = new Medium(
                tdm.TmpDirectoryList[0],
                "normalized",
                Ffmpeg.Format.Video.Mp4);

            int height1 = Resolution;
            if (height1 % 2 != 0) height1--;
            int width1 = (int)(((double)1920 / (double)1080) * (double)Resolution);
            instructions.Add(new ResizeVideo(width1, height1)
            {
                Ffmpeg = ffmpeg,
                Input = tmp0,
                Output = ClipNormalized
            });
            #endregion

            #region Ffmpeg - Speedup
            Medium tmp1 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            if (HasChangeSpeed)
            {
                instructions.Add(new ChangeSpeed(Speed)
                {
                    Ffmpeg = ffmpeg,
                    Input = ClipNormalized,
                    Output = tmp1
                });
            }
            else
            {
                tmp1 = ClipNormalized;
            }
            #endregion

            #region Ffmpeg - Webcam
            Medium Webcam = new Medium(
                tdm.TmpDirectoryList[0],
                "webcam",
                Ffmpeg.Format.Video.Mp4);

            Medium tmp2 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            if (HasWebcam)
            {
                instructions.Add(new CropVideo(
                    ScreenPosWebcam.Width,
                    ScreenPosWebcam.Height,
                    ScreenPosWebcam.PosX,
                    ScreenPosWebcam.PosY)
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp1,
                    Output = tmp2,
                });

                int h1 = (int)(Resolution * ((double)ScreenPosWebcam.Height / (double)ScreenPosWebcam.Width));
                if (h1 % 2 == 1) h1--;

                instructions.Add(new ResizeVideo(Resolution, h1)
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp2,
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

            Medium tmp3 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            instructions.Add(new CropVideo(
                ScreenPosGameplay.Width,
                ScreenPosGameplay.Height,
                ScreenPosGameplay.PosX,
                ScreenPosGameplay.PosY)
            {
                Ffmpeg = ffmpeg,
                Input = tmp1,
                Output = tmp3,
            });

            int h2 = (int)((double)Resolution * ((double)ScreenPosGameplay.Height / (double)ScreenPosGameplay.Width));
            if (h2 % 2 == 1) h2--;

            instructions.Add(new ResizeVideo(Resolution, h2)
            {
                Ffmpeg = ffmpeg,
                Input = tmp3,
                Output = GameplayArea
            });

            composition.Add(GameplayArea);
            #endregion

            #region Ffmpeg - VStack
            Medium tmp4 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            composition.Reverse();
            if (composition.Count > 1)
            {

                instructions.Add(new VStack()
                {
                    Input = composition.ToArray(),
                    Output = tmp4,
                    Ffmpeg = ffmpeg
                });
            }
            else
            {
                tmp4 = composition[0];
            }
            #endregion



            #region Ffmpeg - Color Grading
            Medium tmp5 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            if (HasColorGrading)
            {
                instructions.Add(new ColorGradeVideo()
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp4,
                    Output = tmp5,

                    Contrast = Contrast,
                    Brighness = Brighness,
                    Saturation = Saturation,
                    Gamma = Gamma,
                    GammaR = GammaR,
                    GammaG = GammaG,
                    GammaB = GammaB,
                    GammaWeight = GammaWeight,
                });
            }
            else
            {
                tmp5 = tmp4;
            }
            #endregion

            #region Ffmpeg - Background
            Medium tmp6 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            if (HasBackground)
            {
                var backgroundImgPath = Path.GetDirectoryName(BackgroundImage);
                var backgroundImgFile = Path.GetFileName(BackgroundImage).Split('.')[0];

                instructions.Add(new BackgroundImage()
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp5,
                    Image = new Medium(
                        backgroundImgPath,
                        backgroundImgFile,
                        Format.Image.Jpg),
                    Output = tmp6
                });
            }
            else
            {
                tmp6 = tmp5;
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
                OnRenderEvent(this, i.GetType().ToString());
                OnRenderEvent(this, i.Output.Path);
                i.Execute();
            }
             OnRenderEvent(this, "Rendering finished!");
            tdm.Clear();
            #endregion
        }
    }
}
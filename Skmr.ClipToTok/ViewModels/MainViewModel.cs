﻿using ReactiveUI;
using Skmr.Editor;
using Skmr.Editor.Instructions;
using Skmr.Editor.Instructions.Interfaces;
using Skmr.Editor.Media;
using System.Windows.Input;
using static Skmr.Editor.Ffmpeg;
using SkiaSharp;
using System.Reactive;
using Splat;

namespace Skmr.ClipToTok.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public RoutingState Router { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoSettings { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHighlighter { get; }


        public HighlighterViewModel Hvm { get; }
        public SettingsViewModel Svm { get; }

        public MainViewModel()
        {
            Svm = new SettingsViewModel();
            Hvm = new HighlighterViewModel();

            Router = new RoutingState();
            GoSettings = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(Svm));
            GoHighlighter = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(Hvm));
            Router.Navigate.Execute(Svm);

            RenderCommand = ReactiveCommand.Create(RenderThreaded);
        }


        public string _Log;
        public delegate void LogHandler(string line);
        public event LogHandler OnLog = delegate { };
        private void AddLogEntry(string line)
        {
            OnLog(line);
            Log += $"{DateTime.Now.ToString("hh:mm:ss.FFF")}    {line}\n";
        }
        public string Log
        {
            get { return _Log; }
            set { this.RaiseAndSetIfChanged(ref _Log, value); }
        }

        
        Thread renderThread;
        public ICommand RenderCommand { get; set; }
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
                Svm.ResultFolder,
                DateTime.Now.Ticks.ToString(),
                Ffmpeg.Format.Video.Mp4);

            var videoFolder = Path.GetDirectoryName(Svm.VideoFile);
            var videoName = Path.GetFileName(Svm.VideoFile).Split('.')[0];

            Medium footage = new Medium(
                videoFolder,
                videoName,
                Ffmpeg.Format.Video.Mp4);
            #endregion

            #region Ffmpeg - Cutting
            Medium tmp0 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            if (Svm.HasTimeFrame)
            {
                instructions.Add(new CutVideo((Svm.TimeFrameStart, Svm.TimeFrameDuration))
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

            int height1 = Svm.Resolution;
            if (height1 % 2 != 0) height1--;
            int width1 = (int)(((double)1920 / (double)1080) * (double)Svm.Resolution);
            instructions.Add(new ResizeVideo(width1, height1)
            {
                Ffmpeg = ffmpeg,
                Input = tmp0,
                Output = ClipNormalized
            });
            #endregion

            #region Ffmpeg - Speedup
            Medium tmp1 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            if (Svm.HasChangeSpeed)
            {
                instructions.Add(new ChangeSpeed(Svm.Speed)
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

            if (Svm.HasWebcam)
            {
                instructions.Add(new CropVideo(
                    Svm.ScreenPosWebcam.Width,
                    Svm.ScreenPosWebcam.Height,
                    Svm.ScreenPosWebcam.PosX,
                    Svm.ScreenPosWebcam.PosY)
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp1,
                    Output = tmp2,
                });

                int h1 = (int)(Svm.Resolution * ((double)Svm.ScreenPosWebcam.Height / (double)Svm.ScreenPosWebcam.Width));
                if (h1 % 2 == 1) h1--;

                instructions.Add(new ResizeVideo(Svm.Resolution, h1)
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
                Svm.ScreenPosGameplay.Width,
                Svm.ScreenPosGameplay.Height,
                Svm.ScreenPosGameplay.PosX,
                Svm.ScreenPosGameplay.PosY)
            {
                Ffmpeg = ffmpeg,
                Input = tmp1,
                Output = tmp3,
            });

            int h2 = (int)((double)Svm.Resolution * ((double)Svm.ScreenPosGameplay.Height / (double)Svm.ScreenPosGameplay.Width));
            if (h2 % 2 == 1) h2--;

            instructions.Add(new ResizeVideo(Svm.Resolution, h2)
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

            if (Svm.HasColorGrading)
            {
                instructions.Add(new ColorGradeVideo()
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp4,
                    Output = tmp5,

                    Contrast = Svm.Contrast,
                    Brighness = Svm.Brighness,
                    Saturation = Svm.Saturation,
                    Gamma = Svm.Gamma,
                    GammaR = Svm.GammaR,
                    GammaG = Svm.GammaG,
                    GammaB = Svm.GammaB,
                    GammaWeight = Svm.GammaWeight,
                });
            }
            else
            {
                tmp5 = tmp4;
            }
            #endregion

            #region Ffmpeg - Background
            Medium tmp6 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            if (Svm.HasBackground)
            {
                var backgroundImgPath = Path.GetDirectoryName(Svm.BackgroundImage);
                var backgroundImgFile = Path.GetFileName(Svm.BackgroundImage).Split('.')[0];

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


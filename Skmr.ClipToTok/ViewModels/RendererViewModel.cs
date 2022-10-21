using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skmr.ClipToTok.Utility;
using Skmr.Editor;
using Skmr.Editor.Instructions;
using Skmr.Editor.Instructions.Interfaces;
using Skmr.Editor.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Skmr.Editor.Ffmpeg;
using static Skmr.Editor.Ffmpeg.Format;

namespace Skmr.ClipToTok.ViewModels
{
    public class RendererViewModel : ReactiveObject
    {
        public RendererViewModel()
        {
            RenderCommand = ReactiveCommand.Create(RenderThreaded);
        }

        [Reactive]
        public int Resolution { get; set; } = 1080;

        [Reactive]
        public string ResultFolder { get; set; }

        [Reactive]
        public bool HasBackground { get; set; }

        [Reactive]
        public string BackgroundImage { get; set; }

        //Events
        public delegate void RenderEventHandler(object sender, string e);
        public event RenderEventHandler OnRenderEvent = delegate { };
        public ICommand RenderCommand { get; set; }
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
            var Video = ViewModelBus.VideoViewModel;
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


            List<IInstruction> instructions = new List<IInstruction>();


            Medium result = new Medium(
                ResultFolder,
                DateTime.Now.Ticks.ToString(),
                Ffmpeg.Format.Video.Mp4);

            var videoFolder = Path.GetDirectoryName(Video.VideoFile);
            var videoName = Path.GetFileName(Video.VideoFile).Split('.')[0];

            Medium footage = new Medium(videoFolder, videoName, Ffmpeg.Format.Video.Mp4);
            Medium v0 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            Medium v1 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            Medium v2 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            #endregion

            #region Video

            //Cut out wanted part
            if (Video.HasTimeFrame)
            {
                instructions.Add(new CutVideo((Video.TimeFrameStart, Video.TimeFrameDuration))
                {
                    Ffmpeg = ffmpeg,
                    Input = footage,
                    Output = v0
                });
            }
            else
            {
                v0 = footage;
            }


            //Resize to set resolution
            int height1 = Resolution;
            if (height1 % 2 != 0) height1--;
            int width1 = (int)(((double)1920 / (double)1080) * (double)Resolution);
            instructions.Add(new ResizeVideo(width1, height1)
            {
                Ffmpeg = ffmpeg,
                Input = v0,
                Output = v1,
            });


            // Change Speed
            if (Video.HasChangeSpeed)
            {
                instructions.Add(new ChangeSpeed(Video.Speed)
                {
                    Ffmpeg = ffmpeg,
                    Input = v1,
                    Output = v2
                });
            }
            else
            {
                v2 = v1;
            }


            #region Frames
            List<Medium> frames = new List<Medium>();
            for (int i = 0; i < Video.ScreenPositions.Count; i++)
            {
                Medium f0 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
                Medium f1 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
                Medium f2 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
                //Get Frame Section
                instructions.Add(new CropVideo(
                    Video.ScreenPositions[i].Width,
                    Video.ScreenPositions[i].Height,
                    Video.ScreenPositions[i].PosX,
                    Video.ScreenPositions[i].PosY)
                {
                    Ffmpeg = ffmpeg,
                    Input = v2,
                    Output = f0,
                });

                //Resize
                int h2 = (int)((double)Resolution * ((double)Video.ScreenPositions[i].Height / (double)Video.ScreenPositions[i].Width));
                if (h2 % 2 == 1) h2--;

                instructions.Add(new ResizeVideo(Resolution, h2)
                {
                    Ffmpeg = ffmpeg,
                    Input = f0,
                    Output = f1
                });


                //Color Grade


                if (Video.ScreenPositions[i].HasColorGrading)
                {
                    instructions.Add(new ColorGradeVideo()
                    {
                        Ffmpeg = ffmpeg,
                        Input = f1,
                        Output = f2,

                        Contrast = Video.ScreenPositions[i].Contrast,
                        Brighness = Video.ScreenPositions[i].Brighness,
                        Saturation = Video.ScreenPositions[i].Saturation,
                        Gamma = Video.ScreenPositions[i].Gamma,
                        GammaR = Video.ScreenPositions[i].GammaR,
                        GammaG = Video.ScreenPositions[i].GammaG,
                        GammaB = Video.ScreenPositions[i].GammaB,
                        GammaWeight = Video.ScreenPositions[i].GammaWeight,
                    });
                }
                else
                {
                    f2 = f1;
                }
                frames.Add(f2);
            }
            #endregion

            #endregion

            #region Ffmpeg - VStack
            Medium tmp0 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);
            Medium tmp1 = Medium.GenerateMedium(tdm.TmpDirectoryList[0], Ffmpeg.Format.Video.Mp4);

            frames.Reverse();
            if (frames.Count > 1)
            {

                instructions.Add(new VStack()
                {
                    Input = frames.ToArray(),
                    Output = tmp0,
                    Ffmpeg = ffmpeg
                });
            }
            else
            {
                tmp0 = frames[0];
            }
            #endregion

            #region Ffmpeg - Background


            if (HasBackground)
            {
                var backgroundImgPath = Path.GetDirectoryName(BackgroundImage);
                var backgroundImgFile = Path.GetFileName(BackgroundImage).Split('.')[0];

                instructions.Add(new BackgroundImage()
                {
                    Ffmpeg = ffmpeg,
                    Input = tmp0,
                    Image = new Medium(
                        backgroundImgPath,
                        backgroundImgFile,
                        Format.Image.Jpg),
                    Output = tmp1
                });
            }
            else
            {
                tmp1 = tmp0;
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

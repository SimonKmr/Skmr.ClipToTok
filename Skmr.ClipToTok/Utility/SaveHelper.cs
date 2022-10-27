using Skmr.ClipToTok.Model;
using Skmr.ClipToTok.ViewModels.ClipToTok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Utility
{
    public static class SaveHelper
    {
        public static Project Convert(this ProjectViewModel ViewModel)
        {
            return new Project()
            {
                Renderer = ViewModel.Renderer.Convert(),
                Video = ViewModel.Video.Convert(),
            };
        }
        public static Video Convert(this VideoViewModel ViewModel)
        {
            var f = ViewModel.ScreenPositions;

            Frame[] frames = new Frame[f.Count];
            for(int i = 0; i < f.Count; i++)
            {
                frames[i] = f[i].Convert();
            }

            return new Video()
            {
                VideoFile = ViewModel.VideoFile,
                Frames = frames,

                HasTimeFrame = ViewModel.HasTimeFrame,
                TimeFrameDuration = ViewModel.TimeFrameDuration,
                TimeFrameStart = ViewModel.TimeFrameStart,

                HasChangeSpeed = ViewModel.HasChangeSpeed,
                Speed = ViewModel.Speed,
            };
        }
        public static Frame Convert(this FrameViewModel ViewModel)
        {
            return new Frame() 
            { 
                Red = ViewModel.Red,
                Green = ViewModel.Green,
                Blue = ViewModel.Blue,
                Alpha = ViewModel.Alpha,

                PosX = ViewModel.PosX,
                PosY = ViewModel.PosY,
                Height = ViewModel.Height,
                Width = ViewModel.Width,
            };
        }
        public static Renderer Convert(this RendererViewModel ViewModel)
        {
            return new Renderer() 
            { 
                BackgroundImage = ViewModel.BackgroundImage,
                HasBackground = ViewModel.HasBackground,
                Resolution = ViewModel.Resolution,
                ResultFolder = ViewModel.ResultFolder,
            };
        }


        public static void LoadInto(this ProjectViewModel vm, Project project)
        {
            vm.Renderer.Resolution = project.Renderer.Resolution;
            vm.Renderer.BackgroundImage = project.Renderer.BackgroundImage;
            vm.Renderer.HasBackground = project.Renderer.HasBackground;
            vm.Renderer.ResultFolder = project.Renderer.ResultFolder;

            vm.Video.ClearFrames();
            foreach (var f in project.Video.Frames)
            {
                var frame = new FrameViewModel()
                {
                    Height = f.Height,
                    Width= f.Width,
                    PosX = f.PosX,
                    PosY= f.PosY,

                    Red = f.Red,
                    Green = f.Green,
                    Blue = f.Blue,
                    Alpha = f.Alpha,

                    Brighness = f.Brighness,
                    Contrast = f.Contrast,
                    Saturation = f.Saturation,
                    Gamma = f.Gamma,
                    GammaR = f.GammaR,
                    GammaG = f.GammaG,
                    GammaB = f.GammaB,
                    GammaWeight = f.GammaWeight,
                };
                
                vm.Video.AddFrame(frame);
            }
            vm.Video.VideoFile = project.Video.VideoFile;

            vm.Video.HasTimeFrame = project.Video.HasTimeFrame;
            vm.Video.TimeFrameStart = project.Video.TimeFrameStart;
            vm.Video.TimeFrameDuration = project.Video.TimeFrameDuration;

            vm.Video.HasChangeSpeed = project.Video.HasChangeSpeed;
            vm.Video.Speed = project.Video.Speed;
        }

        public static void Reset(this ProjectViewModel vm)
        {
            vm.Video.VideoFile = String.Empty;
            vm.Video.HasChangeSpeed = false;
            vm.Video.HasTimeFrame = false;
            vm.Video.Speed = 1;
            vm.Video.TimeFrameStart = new TimeSpan();
            vm.Video.TimeFrameDuration = new TimeSpan();
            vm.Video.ClearFrames();
            vm.Renderer.HasBackground = false;
            vm.Renderer.BackgroundImage = String.Empty;
            vm.Renderer.Resolution = 1080;
            vm.Renderer.ResultFolder = String.Empty;
        }
    }
}

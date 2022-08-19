using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Utility
{
    public static class Converter
    {
        public static Settings ToSettings(this ViewModels.SettingsViewModel vm)
        {
            var gameplay = new ScreenPos()
            {
                PosX = vm.ScreenPosGameplay.PosX,
                PosY = vm.ScreenPosGameplay.PosY,
                Width = vm.ScreenPosGameplay.Width,
                Height = vm.ScreenPosGameplay.Height,
            };

            var webcam = new Option<ScreenPos>(new ScreenPos()
            {
                PosX = vm.ScreenPosWebcam.PosX,
                PosY = vm.ScreenPosWebcam.PosY,
                Width = vm.ScreenPosWebcam.Width,
                Height = vm.ScreenPosWebcam.Height,
            }, vm.HasWebcam);

            var background = new Option<Background>(new Background()
            {
                Image = vm.BackgroundImage
            }, vm.HasBackground);

            var speed = new Option<double>(vm.Speed, vm.HasChangeSpeed);

            var colorGrading = new Option<ColorGrading>(new ColorGrading
            {
                Contrast = vm.Contrast,
                Brighness = vm.Brighness,
                Saturation = vm.Saturation,
                Gamma = vm.Gamma,
                GammaR = vm.GammaR,
                GammaG = vm.GammaG,
                GammaB = vm.GammaB,
                GammaWeight = vm.GammaWeight,
            }, vm.HasColorGrading);

            return new Settings()
            {
                Resolution = vm.Resolution,
                ResultFolder = vm.ResultFolder,
                Webcam = webcam,
                Gameplay = gameplay,
                Background = background,
                Speed = speed,
                ColorGrading = colorGrading,
            };
        }
        public static void LoadInto(this Settings settings,ViewModels.SettingsViewModel vm)
        {
            vm.Resolution = settings.Resolution;
            vm.ResultFolder = settings.ResultFolder;

            vm.ScreenPosGameplay.PosX = settings.Gameplay.PosX;
            vm.ScreenPosGameplay.PosY = settings.Gameplay.PosY;
            vm.ScreenPosGameplay.Width = settings.Gameplay.Width;
            vm.ScreenPosGameplay.Height = settings.Gameplay.Height;

            vm.HasWebcam = settings.Webcam.IsEnabled;
            vm.ScreenPosWebcam.PosX = settings.Webcam.Value.PosX;
            vm.ScreenPosWebcam.PosY = settings.Webcam.Value.PosY;
            vm.ScreenPosWebcam.Width = settings.Webcam.Value.Width;
            vm.ScreenPosWebcam.Height = settings.Webcam.Value.Height;

            vm.HasChangeSpeed = settings.Speed.IsEnabled;
            vm.Speed = settings.Speed.Value;

            vm.HasColorGrading = settings.ColorGrading.IsEnabled;
            vm.Contrast = settings.ColorGrading.Value.Contrast;
            vm.Brighness = settings.ColorGrading.Value.Brighness;
            vm.Saturation = settings.ColorGrading.Value.Brighness;
            vm.Gamma = settings.ColorGrading.Value.Gamma;
            vm.GammaR = settings.ColorGrading.Value.GammaR;
            vm.GammaG = settings.ColorGrading.Value.GammaG;
            vm.GammaB = settings.ColorGrading.Value.GammaB;
            vm.GammaWeight = settings.ColorGrading.Value.GammaWeight;

            vm.HasBackground = settings.Background.IsEnabled;
            vm.BackgroundImage = settings.Background.Value.Image;
        }

        public static Highlight ToHighlight(this ViewModels.HighlightViewModel vm)
        {
            return new Highlight()
            {
                Comment = vm.Comment,
                Duration = vm.Duration,
                Start = vm.Start,
            };
        }
        public static void LoadInto(this Highlight highlight, ViewModels.HighlightViewModel vm)
        {
            vm.Start = highlight.Start;
            vm.Duration = highlight.Duration;
            vm.Comment = highlight.Comment;
        }

        public static ViewModels.HighlightViewModel ToHighlightViewModel(this Highlight highlight)
        {
            return new ViewModels.HighlightViewModel()
            {
                Start = highlight.Start,
                Duration = highlight.Duration,
                Comment = highlight.Comment,
            };
        }
    }
}

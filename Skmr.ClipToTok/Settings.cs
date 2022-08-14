using Skmr.ClipToTok.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Skmr.Editor.Instructions.BackgroundImage;

namespace Skmr.ClipToTok
{
    public class Settings
    {
        public Settings() { }
        public Settings(SettingsViewModel svm)
        {
            Resolution = svm.Resolution;
            ResultFolder = svm.ResultFolder;

            Gameplay = new ScreenPos()
            {
                PosX = svm.ScreenPosGameplay.PosX,
                PosY = svm.ScreenPosGameplay.PosY,
                Width = svm.ScreenPosGameplay.Width,
                Height = svm.ScreenPosGameplay.Height,
            };

            Webcam = new Option<ScreenPos>(new ScreenPos()
            {
                PosX = svm.ScreenPosWebcam.PosX,
                PosY = svm.ScreenPosWebcam.PosY,
                Width = svm.ScreenPosWebcam.Width,
                Height = svm.ScreenPosWebcam.Height,
            }, svm.HasWebcam);

            Background = new Option<Background>(new Background()
            {
                Image = svm.BackgroundImage
            }, svm.HasBackground);

            Speed = new Option<double>(svm.Speed, svm.HasChangeSpeed);

            ColorGrading = new Option<ColorGrading>(new ColorGrading
            {
                Contrast = svm.Contrast,
                Brighness = svm.Brighness,
                Saturation = svm.Saturation,
                Gamma = svm.Gamma,
                GammaR = svm.GammaR,
                GammaG = svm.GammaG,
                GammaB = svm.GammaB,
                GammaWeight = svm.GammaWeight,
            }, svm.HasColorGrading);
        }

        public int Resolution { get; set; }
        public string ResultFolder { get; set; }
        public Option<Background> Background { get; set; } = new Option<Background>();
        public Option<ScreenPos> Webcam { get; set; } = new Option<ScreenPos>();
        public ScreenPos Gameplay { get; set; } = new ScreenPos();
        public Option<TextBubble> TextBubble { get; set; } = new Option<TextBubble>();
        public Option<double> Speed { get; set; } = new Option<double>();
        public Option<ColorGrading> ColorGrading { get; set; } = new Option<ColorGrading>();

    }

    public class Option<T> where T : new()
    {
        public Option()
        {
            Value = new T();
        }
        public Option(T value)
        {
            Value = value;
        }
        public Option(T value, bool isEnabled)
        {
            Value = value;
            IsEnabled = isEnabled;
        }

        public bool IsEnabled { get; set; }
        public T Value { get; set; }
    }

    public class TextBubble
    {
        public ScreenPos Position { get; set; }
        public string Text { get; set; }
        public int FontSize { get; set; }
        public (int r, int g, int b) Color1 { get; set; }
        public (int r, int g, int b) Color2 { get; set; }
        public int CornerRadius { get; set; }
    }

    public class Background
    {
        public string Image { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
    }

    public class ScreenPos
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class ColorGrading
    {
        public float Contrast { get; set; }
        public float Brighness { get; set; }
        public float Saturation { get; set; }
        public float Gamma { get; set; }
        public float GammaR { get; set; }
        public float GammaG { get; set; }
        public float GammaB { get; set; }
        public float GammaWeight { get; set; }
    }
}

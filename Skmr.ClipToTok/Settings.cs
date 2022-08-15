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

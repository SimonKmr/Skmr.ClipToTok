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
        public Settings(SettingsViewModel settingsViewModel)
        {
            Resolution = settingsViewModel.Resolution;
            ResultFolder = settingsViewModel.ResultFolder;

            Gameplay = new ScreenPos()
            {
                PosX = settingsViewModel.ScreenPosGameplay.PosX,
                PosY = settingsViewModel.ScreenPosGameplay.PosY,
                Width = settingsViewModel.ScreenPosGameplay.Width,
                Height = settingsViewModel.ScreenPosGameplay.Height,
            };

            Webcam = new Option<ScreenPos>(new ScreenPos()
            {
                PosX = settingsViewModel.ScreenPosWebcam.PosX,
                PosY = settingsViewModel.ScreenPosWebcam.PosY,
                Width = settingsViewModel.ScreenPosWebcam.Width,
                Height = settingsViewModel.ScreenPosWebcam.Height,
            })
            { IsEnabled = settingsViewModel.HasWebcam};

            Background = new Option<Background>(new Background()
            {
                Image = settingsViewModel.BackgroundImage
            })
            { IsEnabled = settingsViewModel.HasBackground };
        }

        public int Resolution { get; set; }
        public string ResultFolder { get; set; }
        public Option<Background> Background { get; set; } = new Option<Background>(new Background());
        public Option<ScreenPos> Webcam { get; set; } = new Option<ScreenPos>(new ScreenPos());
        public ScreenPos Gameplay { get; set; } = new ScreenPos();
        public Option<TextBubble> TextBubble { get; set; } = new Option<TextBubble>(new TextBubble());

    }

    public class Option<T>
    {
        public Option(T value)
        {
            Value = value;
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

}

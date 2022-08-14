using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using ReactiveUI;
using Skmr.Editor.Instructions;
using Microsoft.Win32;
using Splat;

namespace Skmr.ClipToTok.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }
        public SettingsViewModel(IScreen? screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }


        public string Path { get; } = "Settings.json";
        public SettingsViewModel()
        {
            ScreenPosWebcam = new ScreenPosViewModel();
            ScreenPosGameplay = new ScreenPosViewModel();
            
            SaveCommand = ReactiveCommand.Create(Save);
            LoadCommand = ReactiveCommand.Create(Load);
            //if (File.Exists(Path))
            //{
            //    LoadJson();
            //}
            //else
            //{
            //    DefaultSettings();
            //    CreateJson();
            //}
        }

        #region Base
        private string _Video;
        public string Video
        {
            get { return _Video; }
            set { this.RaiseAndSetIfChanged(ref _Video, value); }
        }
        #endregion

        #region Video
        private int _Resolution = 1080;
        public int Resolution
        {
            get { return _Resolution; }
            set { this.RaiseAndSetIfChanged(ref _Resolution, value); }
        }
        
        private string _ResultFolder;
        public string ResultFolder
        {
            get { return _ResultFolder; }
            set { this.RaiseAndSetIfChanged(ref _ResultFolder, value); }
        }

        private string _VideoFile;
        public string VideoFile
        {
            get { return _VideoFile; }
            set { this.RaiseAndSetIfChanged(ref _VideoFile, value); }
        }
        #endregion

        #region TimeFrame
        private bool _HasTimeFrame;
        public bool HasTimeFrame
        {
            get { return _HasTimeFrame; }
            set { this.RaiseAndSetIfChanged(ref _HasTimeFrame, value); }
        }

        private TimeSpan _TimeFrameStart;
        public TimeSpan TimeFrameStart
        {
            get { return _TimeFrameStart; }
            set { this.RaiseAndSetIfChanged(ref _TimeFrameStart, value); }
        }

        private TimeSpan _TimeFrameEnd;
        public TimeSpan TimeFrameDuration
        {
            get { return _TimeFrameEnd; }
            set { this.RaiseAndSetIfChanged(ref _TimeFrameEnd, value); }
        }
        #endregion

        #region Background
        private bool _HasBackground;
        public bool HasBackground
        {
            get { return _HasBackground; }
            set { this.RaiseAndSetIfChanged(ref _HasBackground, value); }
        }

        private string _BackgroundImage;
        public string BackgroundImage
        {
            get { return _BackgroundImage; }
            set { this.RaiseAndSetIfChanged(ref _BackgroundImage, value); }
        }
        #endregion

        #region Webcam
        private bool _HasWebcam;
        public bool HasWebcam
        {
            get { return _HasWebcam; }
            set { this.RaiseAndSetIfChanged(ref _HasWebcam, value); }
        }
        
        private ScreenPosViewModel _ScreenPosWebcam;
        public ScreenPosViewModel ScreenPosWebcam 
        { 
            get { return _ScreenPosWebcam; }
            set { this.RaiseAndSetIfChanged(ref _ScreenPosWebcam, value); }
        }
        #endregion

        #region ColorGrading
        private bool _HasColorGrading;
        public bool HasColorGrading
        {
            get { return _HasColorGrading; }
            set { this.RaiseAndSetIfChanged(ref _HasColorGrading, value); }
        }

        
        
        
        private float _Contrast = 1;
        public float Contrast
        {
            get { return _Contrast; }
            set { this.RaiseAndSetIfChanged(ref _Contrast, value); }
        }
        
        private float _Brighness = 0;
        public float Brighness
        {
            get { return _Brighness; }
            set { this.RaiseAndSetIfChanged(ref _Brighness, value); }
        }
        
        private float _Saturation = 1;
        public float Saturation
        {
            get { return _Saturation; }
            set { this.RaiseAndSetIfChanged(ref _Saturation, value); }
        }

        
        private float _Gamma = 1;
        public float Gamma
        {
            get { return _Gamma; }
            set { this.RaiseAndSetIfChanged(ref _Gamma, value); }
        }

        private float _GammaR = 1f;
        public float GammaR
        {
            get { return _GammaR; }
            set { this.RaiseAndSetIfChanged(ref _GammaR, value); }
        }

        private float _GammaG = 1f;
        public float GammaG
        {
            get { return _GammaG; }
            set { this.RaiseAndSetIfChanged(ref _GammaG, value); }
        }

        private float _GammaB = 1f;
        public float GammaB
        {
            get { return _GammaB; }
            set { this.RaiseAndSetIfChanged(ref _GammaB, value); }
        }
        
        private float _GammaWeight = 1f;
        public float GammaWeight
        {
            get { return _GammaWeight; }
            set { this.RaiseAndSetIfChanged(ref _GammaWeight, value); }
        }
        #endregion

        #region ChangeSpeed
        private bool _HasChangeSpeed;
        public bool HasChangeSpeed
        {
            get { return _HasChangeSpeed; }
            set { this.RaiseAndSetIfChanged(ref _HasChangeSpeed, value); }
        }




        private double _Speed = 1;
        public double Speed
        {
            get { return _Speed; }
            set { this.RaiseAndSetIfChanged(ref _Speed, value); }
        }
        #endregion

        #region Gameplay
        private ScreenPosViewModel _ScreenPosGameplay;
        public ScreenPosViewModel ScreenPosGameplay
        {
            get { return _ScreenPosGameplay; }
            set { this.RaiseAndSetIfChanged(ref _ScreenPosGameplay, value); }
        }
        #endregion

        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand NewCommand { get; set; }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter($"Save.json"))
            {
                var data = new Settings(this);
                sw.Write(JsonConvert.SerializeObject(data));
            }
        }
        public void Load()
        {
            using (StreamReader sr = new StreamReader($"Save.json"))
            {
                Settings loaded = JsonConvert.DeserializeObject<Settings>(sr.ReadToEnd());
                Resolution = loaded.Resolution;
                ResultFolder = loaded.ResultFolder;
                
                ScreenPosGameplay.PosX = loaded.Gameplay.PosX;
                ScreenPosGameplay.PosY = loaded.Gameplay.PosY;
                ScreenPosGameplay.Width = loaded.Gameplay.Width;
                ScreenPosGameplay.Height = loaded.Gameplay.Height;
                
                HasWebcam = loaded.Webcam.IsEnabled;
                ScreenPosWebcam.PosX = loaded.Webcam.Value.PosX;
                ScreenPosWebcam.PosY = loaded.Webcam.Value.PosY;
                ScreenPosWebcam.Width = loaded.Webcam.Value.Width;
                ScreenPosWebcam.Height = loaded.Webcam.Value.Height;

                HasChangeSpeed = loaded.Speed.IsEnabled;
                Speed = loaded.Speed.Value;

                HasColorGrading = loaded.ColorGrading.IsEnabled;
                Contrast = loaded.ColorGrading.Value.Contrast;
                Brighness = loaded.ColorGrading.Value.Brighness;
                Saturation = loaded.ColorGrading.Value.Brighness;
                Gamma = loaded.ColorGrading.Value.Gamma;
                GammaR = loaded.ColorGrading.Value.GammaR;
                GammaG = loaded.ColorGrading.Value.GammaG;
                GammaB = loaded.ColorGrading.Value.GammaB;
                GammaWeight = loaded.ColorGrading.Value.GammaWeight;

                HasBackground = loaded.Background.IsEnabled;
                BackgroundImage = loaded.Background.Value.Image;
            }
        }
    }
}
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

namespace Skmr.ClipToTok.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
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

                HasBackground = loaded.Background.IsEnabled;
                BackgroundImage = loaded.Background.Value.Image;
            }
        }
    }
}

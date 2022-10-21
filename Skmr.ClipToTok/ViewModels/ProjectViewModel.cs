﻿using System.Windows.Input;
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
using DynamicData;
using System.Collections.ObjectModel;
using Skmr.ClipToTok.Model;

namespace Skmr.ClipToTok.ViewModels
{
    public class ProjectViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }


        [Reactive]
        public VideoViewModel Video { get; set; }
        [Reactive]
        public RendererViewModel Renderer { get; set; }

        public ProjectViewModel()
        {
            NewCommand = ReactiveCommand.Create(New);
            SaveCommand = ReactiveCommand.Create(SaveAsync);
            LoadCommand = ReactiveCommand.Create(LoadAsync);

            Renderer = new RendererViewModel();
            Video = new VideoViewModel();
        }

        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand NewCommand { get; set; }

        public void New()
        {

        }
        public async Task SaveAsync()
        {
            var dialogResult = await Interactions.SaveFileDialog.Handle(String.Empty);

            using (StreamWriter sw = new StreamWriter(dialogResult))
            {
                sw.WriteLine(JsonConvert.SerializeObject(this.Convert()));
            }
        }
        public async Task LoadAsync()
        {
            var dialogResult = await Interactions.OpenFileDialog.Handle(String.Empty);

            using (StreamReader sr = new StreamReader(dialogResult))
            {
                var loaded = JsonConvert.DeserializeObject<Project>(sr.ReadToEnd());
                this.LoadInto(loaded);
            }
        }





    }
}
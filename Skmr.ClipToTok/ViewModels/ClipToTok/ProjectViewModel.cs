using System.Windows.Input;
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
using Skmr.ClipToTok.ViewModels.Video;

namespace Skmr.ClipToTok.ViewModels.ClipToTok
{
    public class ProjectViewModel : ReactiveObject
    {
        [Reactive]
        public VideoViewModel[] Videos { get; set; }
        [Reactive]
        public RenderableViewModel Renderer { get; set; }

        public ProjectViewModel()
        {
            ViewModelBus.VideoViewModel = new VideoViewModel();
        }

        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand NewCommand { get; set; }

    }
}
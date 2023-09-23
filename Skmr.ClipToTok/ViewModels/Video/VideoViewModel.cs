using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skmr.ClipToTok.ViewModels.Video.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Skmr.ClipToTok.ViewModels.Video
{
    public class VideoViewModel : ReactiveObject
    {
        [Reactive]
        public string Path { get; set; }
        
        private SourceList<IOptionViewModel> _optionSources = new SourceList<IOptionViewModel>();
        private SourceList<IInfoViewModel> _eventSources = new SourceList<IInfoViewModel>();

        public Interaction<VideoViewModel, object?> AttributesWindow { get; }
        public ICommand OpenAttributesCommand { get; set; }
        
        public VideoViewModel()
        {
            AttributesWindow = new Interaction<VideoViewModel, object?>();
            OpenAttributesCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await AttributesWindow.Handle(this);
            });
        }

        public ICommand AddFrameCommand { get; set; }
        public event EventHandler OnFramesChanged = delegate { };
    }
}

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skmr.ClipToTok.Utility;
using Skmr.Editor;
using Skmr.Editor.Instructions;
using Skmr.Editor.Instructions.Interfaces;
using Skmr.Editor.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Skmr.Editor.Ffmpeg;
using static Skmr.Editor.Ffmpeg.Format;

namespace Skmr.ClipToTok.ViewModels.Video
{
    public abstract class RenderableViewModel : ReactiveObject
    {
        [Reactive]
        public int Resolution { get; set; } = 1080;

        [Reactive]
        public string ResultFolder { get; set; }

        public RenderableViewModel()
        {
            RenderCommand = ReactiveCommand.Create(CreateRenderThread);
        }

        public ICommand RenderCommand { get; set; }

        Thread renderThread;
        public void CreateRenderThread()
        {
            if (renderThread != null)
                if (renderThread.IsAlive)
                    return;

            renderThread = new Thread(Render);
            renderThread.Start();
        }
        public abstract void Render();
    }
}

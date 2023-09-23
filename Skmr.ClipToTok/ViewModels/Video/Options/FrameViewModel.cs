using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Skmr.ClipToTok.ViewModels.Video.Options
{
    public class FrameViewModel : ReactiveObject, IOptionViewModel
    {
        [Reactive]
        public int PosX { get; set; }
        [Reactive]
        public int PosY { get; set; }
        [Reactive]
        public int Width { get; set; }
        [Reactive]
        public int Height { get; set; }

        public bool Render(string input, out string output)
        {
            throw new NotImplementedException();
        }
    }
}

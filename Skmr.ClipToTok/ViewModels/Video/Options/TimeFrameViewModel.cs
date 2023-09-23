using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.ViewModels.Video.Options
{
    public class TimeFrameViewModel : ReactiveObject, IOptionViewModel
    {
        [Reactive]
        public TimeSpan TimeFrameStart { get; set; }

        [Reactive]
        public TimeSpan TimeFrameDuration { get; set; }

        [Reactive]
        public TimeSpan TimeFrameEnd { get; set; }

        public bool Render(string input, out string output)
        {
            throw new NotImplementedException();
        }
    }
}

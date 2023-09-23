using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.ViewModels.Video.Options
{
    internal class ColorGradingViewModel : IOptionViewModel
    {
        [Reactive]
        public float Contrast { get; set; } = 1;

        [Reactive]
        public float Brighness { get; set; } = 0;

        [Reactive]
        public float Saturation { get; set; } = 1;


        [Reactive]
        public float Gamma { get; set; } = 1;

        [Reactive]
        public float GammaR { get; set; } = 1f;

        [Reactive]
        public float GammaG { get; set; } = 1f;

        [Reactive]
        public float GammaB { get; set; } = 1f;

        [Reactive]
        public float GammaWeight { get; set; } = 1f;

        public bool Render(string input, out string output)
        {
            throw new NotImplementedException();
        }
    }
}

using ReactiveUI.Fody.Helpers;
using Skmr.ClipToTok.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.ViewModels
{
    public class MainViewModel
    {
        [Reactive]
        public ClipToTokViewModel ClipToTok { get; set; }
        [Reactive]
        public AnalyzerViewModel Analyzer { get; set; }

        public MainViewModel()
        {
            ClipToTok = new ClipToTokViewModel();
        }
    }
}

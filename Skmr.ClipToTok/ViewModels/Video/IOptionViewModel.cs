using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.ViewModels.Video
{
    public interface IOptionViewModel
    {
        public bool Render(string input, out string output);
    }
}

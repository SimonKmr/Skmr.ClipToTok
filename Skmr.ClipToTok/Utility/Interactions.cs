using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Utility
{
    public class Interactions
    {
        public static readonly Interaction<string, string> OpenFileDialog = new Interaction<string, string>();
        public static readonly Interaction<string, string> SaveFileDialog = new Interaction<string, string>();
    }
}

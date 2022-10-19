using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Avalonia.Utils
{
    public class LoadedEventManager
    {
        public static event EventHandler Loaded = delegate { };
        public static void Fire() => Loaded(null, new EventArgs());
    }
}

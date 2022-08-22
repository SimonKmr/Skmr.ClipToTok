using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Utility
{
    public static class CttMath
    {
        public static TimeSpan Floor(this TimeSpan timeSpan)
            => new TimeSpan((timeSpan.Ticks / (TimeSpan.TicksPerSecond)) * (TimeSpan.TicksPerSecond));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok
{
    public class Highlight
    {
        public TimeSpan Start { get; set; }
        public TimeSpan Duration { get; set; }
        public string Comment { get; set; }
        public float Score { get; set; }


        public string Creator { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

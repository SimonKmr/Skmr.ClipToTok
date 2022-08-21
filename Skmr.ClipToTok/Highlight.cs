using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok
{
    public class Highlight
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Score { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan Duration { get; set; }
        public string Image { get; set; }

        public string Creator { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

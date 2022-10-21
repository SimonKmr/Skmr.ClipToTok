using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Model
{
    public class Project
    {
        [JsonProperty("video")]
        public Video Video { get; set; }

        [JsonProperty("renderer")]
        public Renderer Renderer { get; set; }
    }
}

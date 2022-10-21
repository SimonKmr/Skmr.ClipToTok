using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Model
{
    public class Renderer
    {
        [JsonProperty("resolution")]
        public int Resolution { get; set; }
        
        [JsonProperty("result_folder")]
        public string ResultFolder { get; set; }
        
        [JsonProperty("has_background")]
        public bool HasBackground { get; set; }
        
        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }
    }
}

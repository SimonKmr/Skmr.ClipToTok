using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Model
{
    public class Video
    {
        [JsonProperty("video")]
        public string VideoFile { get; set; }
        
        [JsonProperty("frames")]
        public Frame[] Frames { get; set; }

        [JsonProperty("has_time_frame")]
        public bool HasTimeFrame { get; set; }

        [JsonProperty("start")]
        public TimeSpan TimeFrameStart { get; set; }

        [JsonProperty("duration")]
        public TimeSpan TimeFrameDuration { get; set; }

        [JsonProperty("has_change_speed")]
        public bool HasChangeSpeed { get; set; }

        [JsonProperty("speed")]
        public double Speed { get; set; } = 1;
    }
}

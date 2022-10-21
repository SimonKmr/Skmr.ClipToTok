using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Model
{
    public class Frame
    {
        [JsonProperty("red")]
        public byte Red { get; set; }
        [JsonProperty("green")]
        public byte Green { get; set; }
        [JsonProperty("blue")]
        public byte Blue { get; set; }
        [JsonProperty("alpha")]
        public byte Alpha { get; set; }

        [JsonProperty("x")]
        public int PosX { get; set; }

        [JsonProperty("y")]
        public int PosY { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }



        [JsonProperty("has_color_grading")]
        public bool HasColorGrading { get; set; }

        [JsonProperty("contrast")]
        public float Contrast { get; set; }

        [JsonProperty("brightness")]
        public float Brighness { get; set; }

        [JsonProperty("saturation")]
        public float Saturation { get; set; }


        [JsonProperty("gamma")]
        public float Gamma { get; set; }

        [JsonProperty("gamma_r")]
        public float GammaR { get; set; }

        [JsonProperty("gamma_g")]
        public float GammaG { get; set; }

        [JsonProperty("gamma_b")]
        public float GammaB { get; set; }

        [JsonProperty("gamma_weight")]
        public float GammaWeight { get; set; }
    }
}

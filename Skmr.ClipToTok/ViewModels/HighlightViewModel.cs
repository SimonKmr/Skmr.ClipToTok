using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.ViewModels
{
    public class HighlightViewModel : ReactiveObject
    {
        public double Score { get; set; }
        public TimeSpan Start { get; set; } = TimeSpan.FromSeconds(0);
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0);
        public TimeSpan End { get => Start + Duration; }


        public string TimeFrameText { get => $"{Start} - {End}"; }
        public string DurationText { get => $"{Duration.TotalSeconds} sec"; }
        public string ScoreText { get => $"{Math.Round(Score * 100)}%"; }


        public delegate void ButtonHandler(object sender, TimeSpan start, TimeSpan duration, bool select);
        public event ButtonHandler OnButtonPressed = delegate { };
        public void Select()
        {
            OnButtonPressed(this, Start, Duration, true);
        }
        public void Play()
        {
            OnButtonPressed(this, Start, Duration, false);
        }
    }
}

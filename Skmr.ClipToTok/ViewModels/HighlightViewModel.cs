using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Skmr.ClipToTok.ViewModels
{
    public class HighlightViewModel : ReactiveObject
    {
        public HighlightViewModel()
        {
            PlayCommand = ReactiveCommand.Create(Play);
            SelectCommand = ReactiveCommand.Create(Select);
        }

        private TimeSpan _Duration;
        private TimeSpan _Start;
        
        
        public double Score { get; set; }
        public TimeSpan Start
        {
            get { return _Start; }
            set { this.RaiseAndSetIfChanged(ref _Start, value); }
        }
        public TimeSpan Duration 
        { 
            get { return _Duration; } 
            set { this.RaiseAndSetIfChanged(ref _Duration, value); } 
        }
        public TimeSpan End { get => Start + Duration; }


        public string DurationText { get => $"{Duration.TotalSeconds} sec"; }
        public string ScoreText { get => $"{Math.Round(Score * 100)}%"; }


        public delegate void ButtonHandler(object sender, TimeSpan start, TimeSpan duration, bool select);
        public event ButtonHandler OnSelectPressed = delegate { };
        public event ButtonHandler OnPlayPressed = delegate { };
        public void Select()
        {
            OnSelectPressed(this, Start, Duration, true);
        }
        public void Play()
        {
            OnPlayPressed(this, Start, Duration, false);
        }

        public ICommand PlayCommand { get; set; }
        public ICommand SelectCommand { get; set; }
    }
}

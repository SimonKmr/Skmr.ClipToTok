using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
            EditCommand = ReactiveCommand.Create(Edit);
            SelectCommand = ReactiveCommand.Create(Select);
            DeleteCommand = ReactiveCommand.Create(Delete);

            durationText = this.WhenAnyValue(x => x.Duration)
                .Select(x => $"{Duration.TotalSeconds} sec")
                .ToProperty(this, x => x.DurationText);

            end = this.WhenAnyValue(x => x.Start, x => x.Duration)
                .Select(x => Start + Duration)
                .ToProperty(this, x => x.End);
        }


        [Reactive]
        public string Title { get; set; }
        [Reactive]
        public string Description { get; set; }
        [Reactive]
        public float Score { get; set; }
        [Reactive]
        public string Image { get; set; }
        [Reactive]
        public string Creator { get; set; }
        [Reactive]
        public DateTime CreationTime { get; set; }
        [Reactive]
        public TimeSpan Start { get; set; }
        [Reactive]
        public TimeSpan Duration { get; set; }


        readonly ObservableAsPropertyHelper<TimeSpan> end;
        public TimeSpan End => end.Value;

        readonly ObservableAsPropertyHelper<string> durationText;
        public string DurationText => durationText.Value;


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
        public void Edit()
        {
            NewWindowRequest(this,new EventArgs());
        }
        public void Delete()
        {
            DeleteRequest(this, new EventArgs());
        }

        public ICommand PlayCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SelectCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public delegate void WindowHandler(object sender, EventArgs args);
        private event WindowHandler NewWindowRequest = delegate { };
        public event WindowHandler DeleteRequest = delegate { };

        private bool isWindowRegistered = false;
        public void RegisterWindow(WindowHandler window)
        {
            if(!isWindowRegistered && NewWindowRequest.GetInvocationList().Length > 0)
            {
                NewWindowRequest += window;
                isWindowRegistered = true;
            }
        }
    }
}

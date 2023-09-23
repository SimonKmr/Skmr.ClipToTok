using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Skmr.ClipToTok.ViewModels.Video.Options
{
    internal class SpeedViewModel : ReactiveObject, IOptionViewModel
    {
        [Reactive]
        public double Speed { get; set; } = 1;

        public bool Render(string input, out string output)
        {
            throw new NotImplementedException();
        }
    }
}

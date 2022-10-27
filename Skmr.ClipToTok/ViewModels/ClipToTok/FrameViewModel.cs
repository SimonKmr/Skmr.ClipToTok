using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Skmr.ClipToTok.ViewModels.ClipToTok
{
    public class FrameViewModel : ReactiveObject
    {
        public Interaction<FrameViewModel, object?> AttributesWindow { get; }
        public ICommand OpenAttributesCommand { get; }


        public FrameViewModel()
        {
            AttributesWindow = new Interaction<FrameViewModel, object?>();
            RemoveCommand = ReactiveCommand.Create(Remove);
            OpenAttributesCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await AttributesWindow.Handle(this);
            });
        }
        public ICommand RemoveCommand { get; set; }

        public void Remove()
        {
            OnRemoveRequest(this, new EventArgs());
        }

        [Reactive]
        public byte Red { get; set; } = 255;
        [Reactive]
        public byte Green { get; set; } = 255;
        [Reactive]
        public byte Blue { get; set; } = 255;
        [Reactive]
        public byte Alpha { get; set; } = 50;

        public delegate void PosChangedHandler(int x, int y, int width, int height);
        public event PosChangedHandler OnScreenPosChanged = delegate { };
        public event EventHandler OnRemoveRequest = delegate { };

        private int _PosX = 0;
        private int _PosY = 0;
        private int _Width = 0;
        private int _Height = 0;
        public int PosX
        {
            get => _PosX;
            set
            {
                this.RaiseAndSetIfChanged(ref _PosX, value);
                OnScreenPosChanged(PosX, PosY, Width, Height);
            }
        }
        public int PosY
        {
            get => _PosY;
            set
            {
                this.RaiseAndSetIfChanged(ref _PosY, value);
                OnScreenPosChanged(PosX, PosY, Width, Height);
            }
        }
        public int Width
        {
            get => _Width;
            set
            {
                this.RaiseAndSetIfChanged(ref _Width, value);
                OnScreenPosChanged(PosX, PosY, Width, Height);
            }
        }
        public int Height
        {
            get => _Height;
            set
            {
                this.RaiseAndSetIfChanged(ref _Height, value);
                OnScreenPosChanged(PosX, PosY, Width, Height);
            }
        }

        public void Set(int x, int y, int width, int height)
        {
            PosX = x;
            PosY = y;
            Width = width;
            Height = height;
        }

        #region ColorGrading
        [Reactive]
        public bool HasColorGrading { get; set; }

        [Reactive]
        public float Contrast { get; set; } = 1;

        [Reactive]
        public float Brighness { get; set; } = 0;

        [Reactive]
        public float Saturation { get; set; } = 1;


        [Reactive]
        public float Gamma { get; set; } = 1;

        [Reactive]
        public float GammaR { get; set; } = 1f;

        [Reactive]
        public float GammaG { get; set; } = 1f;

        [Reactive]
        public float GammaB { get; set; } = 1f;

        [Reactive]
        public float GammaWeight { get; set; } = 1f;
        #endregion

    }
}

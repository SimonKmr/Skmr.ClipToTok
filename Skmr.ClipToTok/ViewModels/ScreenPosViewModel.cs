using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.ViewModels
{
    public class ScreenPosViewModel : ReactiveObject
    {
        public delegate void PosChangedHandler(int x, int y, int width, int height);
        public event PosChangedHandler OnScreenPosChanged = delegate { };

        private int _PosX;
        private int _PosY;
        private int _Width;
        private int _Height;
        public int PosX 
        { 
            get => _PosX;
            set 
            { 
                OnScreenPosChanged(PosX, PosY, Width, Height); 
                this.RaiseAndSetIfChanged(ref _PosX, value);  
            } 
        }
        public int PosY 
        {
            get => _PosY;
            set { this.RaiseAndSetIfChanged(ref _PosY, value); OnScreenPosChanged(PosX, PosY, Width, Height); }
        }
        public int Width 
        {
            get => _Width;
            set { this.RaiseAndSetIfChanged(ref _Width, value); OnScreenPosChanged(PosX, PosY, Width, Height); }
        }
        public int Height 
        {
            get => _Height;
            set { this.RaiseAndSetIfChanged(ref _Height, value); OnScreenPosChanged(PosX, PosY, Width, Height); }
        } 
    }
}

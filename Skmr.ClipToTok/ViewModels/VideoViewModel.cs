using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Skmr.ClipToTok.ViewModels
{
    public class VideoViewModel : ReactiveObject
    {
        public Interaction<VideoViewModel, object?> AttributesWindow { get; }
        public ICommand OpenAttributesCommand { get; set; }
        public VideoViewModel()
        {
            AttributesWindow = new Interaction<VideoViewModel, object?>();
            var disposable = _screenPositionsSources.Connect().Bind(out _screenPositions).Subscribe();
            AddFrameCommand = ReactiveCommand.Create(AddFrame);
            OpenAttributesCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await AttributesWindow.Handle(this);
            });
        }



        #region Frames
        public ICommand AddFrameCommand { get; set; }
        public ReadOnlyObservableCollection<FrameViewModel> ScreenPositions => _screenPositions;
        private SourceList<FrameViewModel> _screenPositionsSources = new SourceList<FrameViewModel>();
        private ReadOnlyObservableCollection<FrameViewModel> _screenPositions;
        public event EventHandler OnFramesChanged = delegate { };

        public void AddFrame(FrameViewModel frame)
        {
            _screenPositionsSources.Add(frame);
            frame.OnScreenPosChanged += Spvm_OnScreenPosChanged;
            frame.OnRemoveRequest += Spvm_OnRemoveRequest;
        }
        public void AddFrame()
        {
            var spvm = new FrameViewModel();
            _screenPositionsSources.Add(spvm);
            spvm.OnScreenPosChanged += Spvm_OnScreenPosChanged;
            spvm.OnRemoveRequest += Spvm_OnRemoveRequest;
        }
        public void ClearFrames()
        {
            _screenPositionsSources.Clear();
        }

        private void Spvm_OnScreenPosChanged(int x, int y, int width, int height)
        {
            OnFramesChanged(this, new EventArgs());
        }

        private void Spvm_OnRemoveRequest(object? sender, EventArgs e)
        {
            var s = sender as FrameViewModel;
            _screenPositionsSources.Remove(s);
        }
        #endregion

        #region Video
        [Reactive]
        public string VideoFile { get; set; }
        #endregion

        #region TimeFrame

        [Reactive]
        public bool HasTimeFrame { get; set; }

        [Reactive]
        public TimeSpan TimeFrameStart { get; set; }

        [Reactive]
        public TimeSpan TimeFrameDuration { get; set; }
        #endregion

        #region ChangeSpeed
        [Reactive]
        public bool HasChangeSpeed { get; set; }

        [Reactive]
        public double Speed { get; set; } = 1;
        #endregion

    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;
using System.Globalization;

namespace Skmr.ClipToTok.Avalonia.Utils.UserControls
{
    /// <summary>
    /// A control to allow the user to select a time.
    /// </summary>
    [TemplatePart("PART_FirstColumnDivider", typeof(Rectangle))]
    [TemplatePart("PART_FirstPickerHost", typeof(Border))]
    [TemplatePart("PART_SecondColumnDivider", typeof(Rectangle))]
    [TemplatePart("PART_SecondPickerHost", typeof(Border))]
    [TemplatePart("PART_ThirdColumnDivider", typeof(Rectangle))]
    [TemplatePart("PART_ThirdPickerHost", typeof(Border))]

    [TemplatePart("PART_FlyoutButton", typeof(Button))]
    [TemplatePart("PART_FlyoutButtonContentGrid", typeof(Grid))]

    [TemplatePart("PART_HourTextBlock", typeof(TextBlock))]
    [TemplatePart("PART_MinuteTextBlock", typeof(TextBlock))]
    [TemplatePart("PART_SecondTextBlock", typeof(TextBlock))]

    [TemplatePart("PART_PickerPresenter", typeof(TimePickerPresenter))]
    [TemplatePart("PART_Popup", typeof(Popup))]

    [PseudoClasses(":hasnotime")]
    public class TimePicker : TemplatedControl
    {

        //https://github.com/AvaloniaUI/Avalonia/blob/6a3a2817cf731d6f40840bf3dca058664103fd07/src/Avalonia.Themes.Fluent/Controls/TimePicker.xaml

        public static readonly DirectProperty<TimePicker, TimeSpan?> SelectedTimeProperty =
            AvaloniaProperty.RegisterDirect<TimePicker, TimeSpan?>(nameof(SelectedTime),
                x => x.SelectedTime, (x, v) => x.SelectedTime = v,
                defaultBindingMode: BindingMode.TwoWay);

        // Template Items
        private TimePickerPresenter? _presenter;
        private Button? _flyoutButton;

        private Border? _firstPickerHost;
        private Border? _secondPickerHost;
        private Border? _thirdPickerHost;

        private TextBlock? _hourText;
        private TextBlock? _minuteText;
        private TextBlock? _secondText;

        private Rectangle? _firstSplitter;
        private Rectangle? _secondSplitter;

        private Popup? _popup;



        private TimeSpan? _selectedTime;


        public TimePicker()
        {
            PseudoClasses.Set(":hasnotime", true);
        }


        public TimeSpan? SelectedTime
        {
            get => _selectedTime;
            set
            {
                var old = _selectedTime;
                SetAndRaise(SelectedTimeProperty, ref _selectedTime, value);
                OnSelectedTimeChanged(old, value);
                SetSelectedTimeText();
            }
        }

        /// <summary>
        /// Raised when the <see cref="SelectedTime"/> property changes
        /// </summary>
        public event EventHandler<TimePickerSelectedValueChangedEventArgs>? SelectedTimeChanged;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            if (_flyoutButton != null)
                _flyoutButton.Click -= OnFlyoutButtonClicked;

            if (_presenter != null)
            {
                _presenter.Confirmed -= OnConfirmed;
                _presenter.Dismissed -= OnDismissPicker;
            }
            base.OnApplyTemplate(e);

            _flyoutButton = e.NameScope.Find<Button>("PART_FlyoutButton");

            _firstPickerHost = e.NameScope.Find<Border>("PART_FirstPickerHost");
            _secondPickerHost = e.NameScope.Find<Border>("PART_SecondPickerHost");
            _thirdPickerHost = e.NameScope.Find<Border>("PART_ThirdPickerHost");

            _hourText = e.NameScope.Find<TextBlock>("PART_HourTextBlock");
            _minuteText = e.NameScope.Find<TextBlock>("PART_MinuteTextBlock");
            _secondText = e.NameScope.Find<TextBlock>("PART_SecondTextBlock");

            _firstSplitter = e.NameScope.Find<Rectangle>("PART_FirstColumnDivider");
            _secondSplitter = e.NameScope.Find<Rectangle>("PART_SecondColumnDivider");

            _popup = e.NameScope.Find<Popup>("PART_Popup");
            _presenter = e.NameScope.Find<TimePickerPresenter>("PART_PickerPresenter");

            if (_flyoutButton != null)
                _flyoutButton.Click += OnFlyoutButtonClicked;

            SetSelectedTimeText();

            if (_presenter != null)
            {
                _presenter.Confirmed += OnConfirmed;
                _presenter.Dismissed += OnDismissPicker;
            }
        }


        private void SetSelectedTimeText()
        {
            if (_hourText == null || _minuteText == null || _secondText == null)
                return;

            var time = SelectedTime;
            if (time.HasValue)
            {
                var newTime = SelectedTime!.Value;

                _hourText.Text = newTime.ToString("%h");
                _minuteText.Text = newTime.ToString("mm");
                _secondText.Text = newTime.ToString("ss");
                PseudoClasses.Set(":hasnotime", false);
            }
            else
            {
                _hourText.Text = "hour";
                _minuteText.Text = "minute";
                _secondText.Text = "second";
                PseudoClasses.Set(":hasnotime", true);
            }
        }

        protected virtual void OnSelectedTimeChanged(TimeSpan? oldTime, TimeSpan? newTime)
        {
            SelectedTimeChanged?.Invoke(this, new TimePickerSelectedValueChangedEventArgs(oldTime, newTime));
        }

        private void OnFlyoutButtonClicked(object? sender, RoutedEventArgs e)
        {
            if (_presenter == null)
                throw new InvalidOperationException("No DatePickerPresenter found.");
            if (_popup == null)
                throw new InvalidOperationException("No Popup found.");

            _presenter.Time = SelectedTime ?? new TimeSpan();

            _popup.PlacementMode = PlacementMode.AnchorAndGravity;
            _popup.PlacementAnchor = PopupAnchor.Bottom;
            _popup.PlacementGravity = PopupGravity.Bottom;
            _popup.PlacementConstraintAdjustment = PopupPositionerConstraintAdjustment.SlideY;
            _popup.IsOpen = true;

            // Overlay popup hosts won't get measured until the next layout pass, but we need the
            // template to be applied to `_presenter` now. Detect this case and force a layout pass.
            if (!_presenter.IsMeasureValid)
                (VisualRoot as ILayoutRoot)?.LayoutManager?.ExecuteInitialLayoutPass();

            var deltaY = _presenter.GetOffsetForPopup();

            // The extra 5 px I think is related to default popup placement behavior
            _popup.VerticalOffset = 0;
        }

        private void OnDismissPicker(object? sender, EventArgs e)
        {
            _popup!.Close();
            Focus();
        }

        private void OnConfirmed(object? sender, EventArgs e)
        {
            _popup!.Close();
            SelectedTime = _presenter!.Time;
        }
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace Skmr.ClipToTok.Avalonia.Utils.UserControls
{
    /// <summary>
    /// Defines the presenter used for selecting a time. Intended for use with
    /// <see cref="TimePicker"/> but can be used independently
    /// </summary>
    [TemplatePart("PART_AcceptButton", typeof(Button))]
    [TemplatePart("PART_DismissButton", typeof(Button))]

    //[TemplatePart("PART_DayDownButton", typeof(RepeatButton))]
    //[TemplatePart("PART_DaySelector", typeof(DateTimePickerPanel))]
    //[TemplatePart("PART_DayUpButton", typeof(RepeatButton))]

    [TemplatePart("PART_HourDownButton", typeof(RepeatButton))]
    [TemplatePart("PART_HourSelector", typeof(DateTimePickerPanel))]
    [TemplatePart("PART_HourUpButton", typeof(RepeatButton))]
    
    [TemplatePart("PART_MinuteDownButton", typeof(RepeatButton))]
    [TemplatePart("PART_MinuteSelector", typeof(DateTimePickerPanel))]
    [TemplatePart("PART_MinuteUpButton", typeof(RepeatButton))]

    [TemplatePart("PART_SecondDownButton", typeof(RepeatButton))]
    [TemplatePart("PART_SecondSelector", typeof(DateTimePickerPanel))]
    [TemplatePart("PART_SecondUpButton", typeof(RepeatButton))]

    [TemplatePart("PART_PickerContainer", typeof(Grid))]
    public class TimePickerPresenter : PickerPresenterBase
    {
        /// <summary>
        /// Defines the <see cref="Time"/> property
        /// </summary>
        public static readonly DirectProperty<TimePickerPresenter, TimeSpan> TimeProperty =
            AvaloniaProperty.RegisterDirect<TimePickerPresenter, TimeSpan>(nameof(Time),
                x => x.Time, (x, v) => x.Time = v);

        public TimePickerPresenter()
        {
            Time = DateTime.Now.TimeOfDay;
            KeyboardNavigation.SetTabNavigation(this, KeyboardNavigationMode.Cycle);
        }

        // TemplateItems
        private Grid? _pickerContainer;
        private Button? _acceptButton;
        private Button? _dismissButton;
        //private DateTimePickerPanel? _daySelector;
        private DateTimePickerPanel? _hourSelector;
        private DateTimePickerPanel? _minuteSelector;
        private DateTimePickerPanel? _secondSelector;
        //private Button? _dayUpButton;
        private Button? _hourUpButton;
        private Button? _minuteUpButton;
        private Button? _secondUpButton;
        //private Button? _dayDownButton;
        private Button? _hourDownButton;
        private Button? _minuteDownButton;
        private Button? _secondDownButton;

        // Backing Fields
        private TimeSpan _time;

        /// <summary>
        /// Gets or sets the minute increment in the selector
        /// </summary>


        /// <summary>
        /// Gets or sets the current clock identifier, either 12HourClock or 24HourClock
        /// </summary>


        /// <summary>
        /// Gets or sets the current time
        /// </summary>
        public TimeSpan Time
        {
            get => _time;
            set
            {
                SetAndRaise(TimeProperty, ref _time, value);
                InitPicker();
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _pickerContainer = e.NameScope.Get<Grid>("PART_PickerContainer");

            //_daySelector = e.NameScope.Get<DateTimePickerPanel>("PART_DaySelector");
            _hourSelector = e.NameScope.Get<DateTimePickerPanel>("PART_HourSelector");
            _minuteSelector = e.NameScope.Get<DateTimePickerPanel>("PART_MinuteSelector");
            _secondSelector = e.NameScope.Get<DateTimePickerPanel>("PART_SecondSelector");

            _acceptButton = e.NameScope.Get<Button>("PART_AcceptButton");
            _acceptButton.Click += OnAcceptButtonClicked;

            //_dayUpButton = e.NameScope.Find<RepeatButton>("PART_PeriodUpButton");
            //if (_dayUpButton != null)
            //    _dayUpButton.Click += OnSelectorButtonClick;
            //_dayDownButton = e.NameScope.Find<RepeatButton>("PART_PeriodDownButton");
            //if (_dayDownButton != null)
            //    _dayDownButton.Click += OnSelectorButtonClick;

            _hourUpButton = e.NameScope.Find<RepeatButton>("PART_HourUpButton");
            if (_hourUpButton != null)
                _hourUpButton.Click += OnSelectorButtonClick;
            _hourDownButton = e.NameScope.Find<RepeatButton>("PART_HourDownButton");
            if (_hourDownButton != null)
                _hourDownButton.Click += OnSelectorButtonClick;

            _minuteUpButton = e.NameScope.Find<RepeatButton>("PART_MinuteUpButton");
            if (_minuteUpButton != null)
                _minuteUpButton.Click += OnSelectorButtonClick;
            _minuteDownButton = e.NameScope.Find<RepeatButton>("PART_MinuteDownButton");
            if (_minuteDownButton != null)
                _minuteDownButton.Click += OnSelectorButtonClick;

            _secondUpButton = e.NameScope.Find<RepeatButton>("PART_SecondUpButton");
            if (_secondUpButton != null)
                _secondUpButton.Click += OnSelectorButtonClick;
            _secondDownButton = e.NameScope.Find<RepeatButton>("PART_SecondDownButton");
            if (_secondDownButton != null)
                _secondDownButton.Click += OnSelectorButtonClick;

            _dismissButton = e.NameScope.Find<Button>("PART_DismissButton");
            if (_dismissButton != null)
                _dismissButton.Click += OnDismissButtonClicked;

            InitPicker();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    OnDismiss();
                    e.Handled = true;
                    break;
                case Key.Tab:
                    if (FocusManager.Instance?.Current is IInputElement focus)
                    {
                        var nextFocus = KeyboardNavigationHandler.GetNext(focus, NavigationDirection.Next);
                        KeyboardDevice.Instance?.SetFocusedElement(nextFocus, NavigationMethod.Tab, KeyModifiers.None);
                        e.Handled = true;
                    }
                    break;
                case Key.Enter:
                    OnConfirmed();
                    e.Handled = true;
                    break;
            }
            base.OnKeyDown(e);
        }

        protected override void OnConfirmed()
        {
            //var day = _daySelector!.SelectedValue;
            var hr = _hourSelector!.SelectedValue;
            var min = _minuteSelector!.SelectedValue;
            var snd = _secondSelector!.SelectedValue;

            Time = new TimeSpan(hr, min, snd);

            base.OnConfirmed();
        }

        private void InitPicker()
        {
            if (_pickerContainer == null)
                return;

            //_daySelector!.MaximumValue = 30;
            //_daySelector.MinimumValue = 0;
            //_daySelector.Increment = 1;
            //_daySelector.SelectedValue = Time.Days;
            //_daySelector.ItemFormat = "dd";

            _hourSelector!.MaximumValue = 23;
            _hourSelector.MinimumValue = 0;
            _hourSelector.Increment = 1;
            _hourSelector.SelectedValue = Time.Hours;
            _hourSelector.ItemFormat = "hh";

            _minuteSelector!.MaximumValue = 59;
            _minuteSelector.MinimumValue = 0;
            _minuteSelector.Increment = 1;
            _minuteSelector.SelectedValue = Time.Minutes;
            _minuteSelector.ItemFormat = "mm";

            _secondSelector!.MaximumValue = 59;
            _secondSelector.MinimumValue = 0;
            _secondSelector.Increment = 1;
            _secondSelector.SelectedValue = Time.Seconds;
            _secondSelector.ItemFormat = "ss";

            KeyboardDevice.Instance?.SetFocusedElement(_hourSelector, NavigationMethod.Pointer, KeyModifiers.None);
        }



        private void OnDismissButtonClicked(object? sender, RoutedEventArgs e)
        {
            OnDismiss();
        }

        private void OnAcceptButtonClicked(object? sender, RoutedEventArgs e)
        {
            OnConfirmed();
        }

        private void OnSelectorButtonClick(object? sender, RoutedEventArgs e)
        {
            //if (sender == _dayUpButton)
            //    _daySelector!.ScrollUp();
            //else if (sender == _dayDownButton)
            //    _daySelector!.ScrollDown();
            if (sender == _hourUpButton)
                _hourSelector!.ScrollUp();
            else if (sender == _hourDownButton)
                _hourSelector!.ScrollDown();
            else if (sender == _minuteUpButton)
                _minuteSelector!.ScrollUp();
            else if (sender == _minuteDownButton)
                _minuteSelector!.ScrollDown();
            else if (sender == _secondUpButton)
                _secondSelector!.ScrollUp();
            else if (sender == _secondDownButton)
                _secondSelector!.ScrollDown();
        }

        internal double GetOffsetForPopup()
        {
            if (_hourSelector is null)
                return 0;

            var acceptDismissButtonHeight = _acceptButton != null ? _acceptButton.Bounds.Height : 41;
            return -(MaxHeight - acceptDismissButtonHeight) / 2 - (_hourSelector.ItemHeight / 2);
        }
    }
}

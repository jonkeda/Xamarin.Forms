﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Xamarin.Forms.Platform.WPF
{
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
        }
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Time", typeof(TimeSpan), typeof(TimePicker),
        new UIPropertyMetadata(DateTime.Now.TimeOfDay, new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = obj as TimePicker;
            control.Hours = ((TimeSpan)e.NewValue).Hours;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.Seconds = ((TimeSpan)e.NewValue).Seconds;
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register("Hours", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register("Minutes", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public event EventHandler<EventArgs> TimeChanged;

        public static readonly DependencyProperty SecondsProperty =
        DependencyProperty.Register("Seconds", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));


        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = obj as TimePicker;
            control.Time = new TimeSpan(control.Hours, control.Minutes, control.Seconds);
            if (control.TimeChanged != null)
            {
                control.TimeChanged.Invoke(control, EventArgs.Empty);
            }
        }

        private void Down(object sender, KeyEventArgs args)
        {
            switch (((System.Windows.Controls.Grid)sender).Name)
            {
                case "sec":
                    if (args.Key == Key.Up)
                        this.Seconds++;
                    if (args.Key == Key.Down)
                        this.Seconds--;
                    break;

                case "min":
                    if (args.Key == Key.Up)
                        this.Minutes++;
                    if (args.Key == Key.Down)
                        this.Minutes--;
                    break;

                case "hour":
                    if (args.Key == Key.Up)
                        this.Hours++;
                    if (args.Key == Key.Down)
                        this.Hours--;
                    break;
            }
        }

    }

}

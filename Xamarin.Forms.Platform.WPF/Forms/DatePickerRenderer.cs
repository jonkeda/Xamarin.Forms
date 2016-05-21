using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xamarin.Forms.Platform.WPF
{
	public class DatePickerRenderer : ViewRenderer<Xamarin.Forms.DatePicker, Xceed.Wpf.Toolkit.DateTimePicker>
	{
		Brush _defaultBrush;

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
		{
			base.OnElementChanged(e);

			var datePicker = new Xceed.Wpf.Toolkit.DateTimePicker { Value = Element.Date };

			datePicker.Loaded += (sender, args) => {
				// The defaults from the control template won't be available
				// right away; we have to wait until after the template has been applied
				_defaultBrush = datePicker.Foreground;
				UpdateTextColor();
			};

			datePicker.ValueChanged += DatePickerOnValueChanged;
			SetNativeControl(datePicker);

			UpdateFormatString();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName)
				Control.Value = Element.Date;
			else if (e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
				UpdateFormatString();
			else if (e.PropertyName == Xamarin.Forms.DatePicker.TextColorProperty.PropertyName)
				UpdateTextColor();
		}

		internal override void OnModelFocusChangeRequested(object sender, VisualElement.FocusRequestArgs args)
		{
            Xceed.Wpf.Toolkit.DateTimePicker control = Control;
			if (control == null)
				return;

			if (args.Focus)
			{
				typeof(DateTimePickerBase).InvokeMember("OpenPickerPage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder, control, null);
				args.Result = true;
			}
			else
			{
				UnfocusControl(control);
				args.Result = true;
			}
		}

		void DatePickerOnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
		{
			if (Control.Value.HasValue)
				((IElementController)Element).SetValueFromRenderer(Xamarin.Forms.DatePicker.DateProperty, Control.Value.Value);
		}

		void UpdateFormatString()
		{
			Control.TimeFormatString = "{0:" + Element.Format + "}";
		}

		void UpdateTextColor()
		{
			Color color = Element.TextColor;
			Control.Foreground = color.IsDefault ? (_defaultBrush ?? color.ToBrush()) : color.ToBrush();
		}
	}
}
using System;
using System.Windows;

namespace Xamarin.Forms.Platform.WPF
{
	public class FormsListPicker : ListPicker
	{
		internal static readonly DependencyProperty ListPickerModeChangedProperty = DependencyProperty.Register("ListPickerMode", 
            typeof(ListPickerMode), typeof(FormsListPicker),
			new PropertyMetadata(ModeChanged));

		protected virtual void OnListPickerModeChanged(DependencyPropertyChangedEventArgs args)
		{
			ListPickerModeChanged?.Invoke(this, args);
		}

		internal event EventHandler<DependencyPropertyChangedEventArgs> ListPickerModeChanged;

		static void ModeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var listPicker = dependencyObject as FormsListPicker;
			listPicker?.OnListPickerModeChanged(dependencyPropertyChangedEventArgs);
		}
	}

    public enum ListPickerMode
    {
        Inline, Popup
    }
}
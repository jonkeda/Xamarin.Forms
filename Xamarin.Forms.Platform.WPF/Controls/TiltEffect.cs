using System.Windows;

namespace Xamarin.Forms.Platform.WPF
{
    public class TiltEffect
    {
        public static readonly DependencyProperty IsTiltEnabledProperty = DependencyProperty.RegisterAttached("IsTiltEnabled", typeof (bool), typeof (TiltEffect), new PropertyMetadata(default(bool)));

        public static bool GetIsTiltEnabled(UIElement element)
        {
            return (bool) element.GetValue(IsTiltEnabledProperty);
        }

        public static void SetIsTiltEnabled(UIElement element, bool value)
        {
            element.SetValue(IsTiltEnabledProperty, value);
        }
    }
}

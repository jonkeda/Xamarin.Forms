using System.Windows;
using System.Windows.Controls;

namespace Xamarin.Forms.Platform.WPF
{
    public class PhoneTextBox : TextBox
    
    
    {
        public PhoneTextBox()
        {
            Style = System.Windows.Application.Current.FindResource("PhoneTextBox") as System.Windows.Style;
        }

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(PhoneTextBox), new PropertyMetadata(null));
    }
}

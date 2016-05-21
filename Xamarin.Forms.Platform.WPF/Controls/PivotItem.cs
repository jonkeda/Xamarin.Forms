using System.Windows;
using System.Windows.Controls;

namespace Xamarin.Forms.Platform.WPF
{
    public class Pivot : TabControl {
       // public static DependencyProperty TitleProperty { get; set; }


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Pivot), new PropertyMetadata(null));

        
        public System.Windows.DataTemplate HeaderTemplate { get; set; }
    }

    public class PivotItem : TabItem
    {
    }
}

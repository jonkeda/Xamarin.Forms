using System.Windows;

namespace Xamarin.Forms.Platform.WPF
{
    public class PanoramaItem : DependencyObject
    {
        public DependencyObject Content { get; set; }
        public object DataContext { get; set; }
    }
}

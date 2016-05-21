using System.Collections.ObjectModel;

namespace Xamarin.Forms.Platform.WPF
{
    public class ApplicationBar
    {
        public ApplicationBar()
        {
            MenuItems = new ObservableCollection<ApplicationBarMenuItem>();
            Buttons = new ObservableCollection<ApplicationBarIconButton>();
        }

        public ObservableCollection<ApplicationBarIconButton> Buttons { get; set; }
        public bool IsVisible { get; set; }
        public ObservableCollection<ApplicationBarMenuItem> MenuItems { get; set; }
    }
}

using System.Windows.Controls;

namespace Xamarin.Forms.Platform.WPF
{
   public  class ListPicker : ListBox
    {
       public System.Windows.DataTemplate FullModeItemTemplate { get; set; }
       public string FullModeHeader { get; set; }
    }
}

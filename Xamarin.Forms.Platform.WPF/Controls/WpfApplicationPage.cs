using System;
using System.ComponentModel;
using System.Windows;

namespace Xamarin.Forms.Platform.WPF
{
    public class WpfApplicationPage : Window
    {
        public ApplicationBar ApplicationBar { get; set; }
        public event EventHandler<CancelEventArgs> BackKeyPress;


        public void BackClick(object sender, RoutedEventArgs e)
        {
            BackPage();
        }
        public bool BackPage()
        {
            if (BackKeyPress != null)
            {
                var cancel = new CancelEventArgs();
                BackKeyPress.Invoke(this, cancel);

                return !cancel.Cancel;
            }
            return false;
        }
    }
}

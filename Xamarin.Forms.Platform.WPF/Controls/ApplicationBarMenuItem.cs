using System;
using System.Windows.Controls;

namespace Xamarin.Forms.Platform.WPF
{
    public class ApplicationBarMenuItem : Control
    {
        public string Text { get; set; }

        public void Activate()
        {
                
        }

        public event EventHandler Click;
    }
}

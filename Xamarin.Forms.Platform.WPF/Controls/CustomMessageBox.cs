using System;
using System.Windows.Controls;

namespace Xamarin.Forms.Platform.WPF
{
    public class CustomMessageBox
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string LeftButtonContent { get; set; }
        public string RightButtonContent { get; set; }
        public ListBox Content { get; set; }

        public void Show()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<DismissedEventArgs> Dismissed;


        public void Dismiss()
        {
            

        }
    }
}
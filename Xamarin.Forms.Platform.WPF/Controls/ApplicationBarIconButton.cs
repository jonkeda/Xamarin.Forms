using System;
using System.Windows.Input;

namespace Xamarin.Forms.Platform.WPF
{
    public class ApplicationBarIconButton : Button
    {
        public Uri IconUri { get; set; }
        public string Text { get; set; }
        public event EventHandler Click;

        public ICommand ClickCommand
        {
            get
            {
                return new Command(DoClick);
            }
        }

        private void DoClick()
        {
            if (Click != null)
            {
                Click.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
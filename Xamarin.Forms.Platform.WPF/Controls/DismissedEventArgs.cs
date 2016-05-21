using System;

namespace Xamarin.Forms.Platform.WPF
{
    public class DismissedEventArgs : EventArgs
    {
        // Methods
        public DismissedEventArgs(CustomMessageBoxResult result)
        {
            this.Result = result;
        }

        // Properties
        public CustomMessageBoxResult Result { get; private set; }
    }
}
using System;

namespace Xamarin.Forms.Platform.WPF
{
    public interface ITransition
    {
        void Stop();
        event EventHandler<EventArgs> Completed;

        void Begin();
    }
}

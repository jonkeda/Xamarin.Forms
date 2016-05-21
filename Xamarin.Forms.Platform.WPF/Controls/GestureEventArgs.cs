using System;
using System.Windows;

namespace Xamarin.Forms.Platform.WPF
{




    public class GestureEventArgs : EventArgs
    {
        // Methods
        internal GestureEventArgs(System.Windows.Point gestureOrigin, System.Windows.Point position)
        {
            this.GestureOrigin = gestureOrigin;
            this.TouchPosition = position;
        }

        public System.Windows.Point GetPosition(UIElement relativeTo)
        {
            return GetPosition(relativeTo, this.TouchPosition);
        }

        protected static System.Windows.Point GetPosition(UIElement relativeTo, System.Windows.Point point)
        {
            if (relativeTo == null)
            {
                relativeTo = ApplicationEx.Current.RootVisual;
            }
            if (relativeTo != null)
            {
                return relativeTo.TranslatePoint(point, relativeTo);
                //return relativeTo.TransformToVisual(null).Inverse.Transform(point);
            }
            return point;
        }

        // Properties
        protected System.Windows.Point GestureOrigin { get; private set; }

        public bool Handled { get; set; }

        public object OriginalSource { get; internal set; }

        protected System.Windows.Point TouchPosition { get; private set; }
    }


}
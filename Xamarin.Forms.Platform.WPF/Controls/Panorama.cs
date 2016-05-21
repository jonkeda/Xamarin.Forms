using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Xamarin.Forms.Platform.WPF
{
    public class Panorama : Control
    {
        public static System.Windows.DependencyProperty TitleProperty { get; set; }
        public List<PanoramaItem> Items { get; set; }
        public static DependencyProperty SelectedItemProperty { get; set; }
        public PanoramaItem SelectedItem { get; set; }


        public void OnItemsChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            //throw new NotImplementedException();
        }

        protected event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    }
}

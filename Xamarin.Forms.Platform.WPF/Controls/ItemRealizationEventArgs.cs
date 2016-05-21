using System;

namespace Xamarin.Forms.Platform.WPF
{
    public sealed class ItemRealizationEventArgs : EventArgs
    {
        // Methods
        public ItemRealizationEventArgs(System.Windows.Controls.ContentPresenter container) //, LongListSelectorItemKind itemKind)
        {
            this.Container = container;
            //this.ItemKind = itemKind;
        }

        // Properties
        public System.Windows.Controls.ContentPresenter Container { get; private set; }

        //public LongListSelectorItemKind ItemKind { get; private set; }
    }
}
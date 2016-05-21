using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Xamarin.Forms.Platform.WPF
{
    public class LongListSelector : System.Windows.Controls.ListView
    {
        //public static DependencyProperty IsGroupingEnabledProperty { get; set; }


        public bool IsGroupingEnabled
        {
            get { return (bool)GetValue(IsGroupingEnabledProperty); }
            set { SetValue(IsGroupingEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGroupingEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGroupingEnabledProperty =
            DependencyProperty.Register("IsGroupingEnabled", typeof(bool), typeof(LongListSelector), new PropertyMetadata(false));



        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (Tap != null
                && _tapped)
            {
                Tap.Invoke(this,new GestureEventArgs(new System.Windows.Point(), e.GetPosition(this) )
                {
                    OriginalSource = e.OriginalSource
                } );
                _tapped = false;
            }
        }


        public event EventHandler<GestureEventArgs> Tap;
        public event EventHandler<GestureEventArgs> Hold;
        public event EventHandler<ItemRealizationEventArgs> ItemRealized;

        private bool _tapped;
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            
            base.OnSelectionChanged(e);

            if (SelectedIndex >= 0)
            {
                SelectedIndex = -1;
                _tapped = true;
            }
        }


        public System.Windows.Size GridCellSize
        {
            get { return (System.Windows.Size)GetValue(GridCellSizeProperty); }
            set { SetValue(GridCellSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridCellSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridCellSizeProperty =
            DependencyProperty.Register("GridCellSize", typeof(System.Windows.Size), typeof(LongListSelector));

        

        public LongListSelectorLayoutMode LayoutMode
        {
            get { return (LongListSelectorLayoutMode)GetValue(LayoutModeProperty); }
            set { SetValue(LayoutModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LayoutMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LayoutModeProperty =
            DependencyProperty.Register("LayoutMode", typeof(LongListSelectorLayoutMode), typeof(LongListSelector), new PropertyMetadata(LongListSelectorLayoutMode.Grid));

        
    }
}
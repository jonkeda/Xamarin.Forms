using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Xamarin.Forms.Platform.WPF
{
    public class HubTile : Control
    {
        public string Title { get; set; }
        public BitmapImage Source { get; set; }
        public TileSize Size { get; set; }

        public event Action<object, object> Tap;

    }
}
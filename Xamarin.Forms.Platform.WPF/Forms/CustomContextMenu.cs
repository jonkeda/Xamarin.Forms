using System.Windows;
using System.Windows.Controls;
using WApplication = System.Windows.Application;

namespace Xamarin.Forms.Platform.WPF
{
	public sealed class CustomContextMenu : ContextMenu
	{
		protected override DependencyObject GetContainerForItemOverride()
		{
			var item = new System.Windows.Controls.MenuItem();
			item.SetBinding(HeaderedItemsControl.HeaderProperty, new System.Windows.Data.Binding("Text") { Converter = (System.Windows.Data.IValueConverter)WApplication.Current.Resources["LowerConverter"] });

			item.Click += (sender, args) =>
			{
				IsOpen = false;

				var menuItem = item.DataContext as MenuItem;
				if (menuItem != null)
					menuItem.Activate();
			};
			return item;
		}
	}
}
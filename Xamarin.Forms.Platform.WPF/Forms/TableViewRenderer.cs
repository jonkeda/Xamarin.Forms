﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Xamarin.Forms.Platform.WPF
{
	public class TableViewRenderer : ViewRenderer<Xamarin.Forms.TableView, TableView>
	{
		TableView _view;

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SizeRequest result = base.GetDesiredSize(widthConstraint, heightConstraint);
			result.Minimum = new Size(40, 40);
			return result;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TableView> e)
		{
			base.OnElementChanged(e);

			Element.ModelChanged += OnModelChanged;

			_view = new TableView { DataContext = Element.Root };
			_view.Tap += OnTapTable;
			_view.Hold += OnLongPressTable;
			SetNativeControl(_view);
		}

		bool FindIndices(GestureEventArgs e, out int sectionIndex, out int cellIndex)
		{
			sectionIndex = 0;
			cellIndex = 0;

			TableSection section = null;
			Cell cell = null;

			System.Windows.Point pos = e.GetPosition(System.Windows.Application.Current.MainWindow);
			if (Device.Info.CurrentOrientation.IsLandscape())
			{
				double x = pos.Y;
				double y = System.Windows.Application.Current.MainWindow.RenderSize.Width - pos.X + (SystemTray.IsVisible ? 72 : 0);
				pos = new System.Windows.Point(x, y);
			}
			IEnumerable<UIElement> elements = VisualTreeHelperEx.FindElementsInHostCoordinates(pos, System.Windows.Application.Current.MainWindow);
			foreach (FrameworkElement element in elements.OfType<FrameworkElement>())
			{
				if (cell == null)
					cell = element.DataContext as Cell;
				else if (section == null)
					section = element.DataContext as TableSection;
				else
					break;
			}

			if (cell == null || section == null)
				return false;

			sectionIndex = Element.Root.IndexOf(section);
			cellIndex = section.IndexOf(cell);
			return true;
		}

		void OnLongPressTable(object sender, GestureEventArgs e)
		{
			int sectionIndex, cellIndex;
			if (!FindIndices(e, out sectionIndex, out cellIndex))
				return;

			Element.Model.RowLongPressed(sectionIndex, cellIndex);
		}

		void OnModelChanged(object sender, EventArgs eventArgs)
		{
			_view.DataContext = Element.Root;
		}

		void OnTapTable(object sender, GestureEventArgs e)
		{
			int sectionIndex, cellIndex;
			if (!FindIndices(e, out sectionIndex, out cellIndex))
				return;

			Element.Model.RowSelected(sectionIndex, cellIndex);
		}
	}
}
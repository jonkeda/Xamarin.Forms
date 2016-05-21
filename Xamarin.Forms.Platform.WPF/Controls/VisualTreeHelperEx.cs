using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Xamarin.Forms.Platform.WPF
{
    public class ApplicationEx
    {
        public static ApplicationEx Current { get; set; }
        public UIElement RootVisual { get; set; }
    }

    public class VisualTreeHelperEx
    {
        internal static IEnumerable<UIElement> FindElementsInHostCoordinates(System.Windows.Point point, FrameworkElement longListSelector)
        {
            List<UIElement> result = new List<UIElement>();
            VisualTreeHelper.HitTest(
                longListSelector,
                null,
                new HitTestResultCallback(
                    (HitTestResult hit) =>
                    {
                        result.Add((UIElement)hit.VisualHit);
                        return HitTestResultBehavior.Continue;
                    }),
                new PointHitTestParameters(point));

            return result;
        }

        public static IEnumerable<UIElement> FindElementsInHostCoordinates(Rect intersectingRect, LongListSelector longListSelector)
        {
            List<UIElement> result = new List<UIElement>();
            VisualTreeHelper.HitTest(
                longListSelector,
                null,
                new HitTestResultCallback(
                    (HitTestResult hit) =>
                    {
                        result.Add((UIElement)hit.VisualHit);
                        return HitTestResultBehavior.Continue;
                    }),
                new System.Windows.Media.GeometryHitTestParameters(new RectangleGeometry(intersectingRect)));

            return result;
        }
    }
}
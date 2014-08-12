using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OpenSprinklerApp
{
    public class CalendarDayPanel : Panel
    {
		protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
		{
			var fullHeight = availableSize.Height;
			foreach(var child in Children)
			{
				child.Measure(availableSize);
			}

			return base.MeasureOverride(availableSize);
		}

		protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
		{
			var fullHeight = finalSize.Height;
			foreach (var child in Children)
			{
				var item = child;
				if (IsItemsHost)
				{
					if (item is ContentPresenter)
						item = VisualTreeHelper.GetChild(item, 0) as UIElement;
				}
				if (item == null)
					continue;
				object startObj = item.GetValue(StartTimeProperty);
				object durObj = item.GetValue(DurationProperty);
				if (startObj == null || durObj == null)
				{
					item.Arrange(new Rect(0, 0, 0, 0));
				}
				else
				{
					double start = ((TimeSpan)startObj).TotalSeconds;
					double dur = ((TimeSpan)durObj).TotalSeconds;
					var h1 = fullHeight * (start / (24 * 60 * 60));
					var h2 = fullHeight * (dur / (24 * 60 * 60));
					item.Arrange(new Rect(0, h1, finalSize.Width, h2));
				}
			}

			return base.ArrangeOverride(finalSize);
		}



		public static TimeSpan GetStartTime(DependencyObject obj)
		{
			return (TimeSpan)obj.GetValue(StartTimeProperty);
		}

		public static void SetStartTime(DependencyObject obj, TimeSpan value)
		{
			obj.SetValue(StartTimeProperty, value);
		}

		public static readonly DependencyProperty StartTimeProperty =
			DependencyProperty.RegisterAttached("StartTime", typeof(TimeSpan), typeof(CalendarDayPanel), new PropertyMetadata(null));

		public static TimeSpan GetDuration(DependencyObject obj)
		{
			return (TimeSpan)obj.GetValue(DurationProperty);
		}

		public static void SetDuration(DependencyObject obj, TimeSpan value)
		{
			obj.SetValue(DurationProperty, value);
		}

		public static readonly DependencyProperty DurationProperty =
			DependencyProperty.RegisterAttached("Duration", typeof(TimeSpan), typeof(CalendarDayPanel), new PropertyMetadata(null));

		public class Entry
		{
			public string Title { get; set; }
			public Color Color { get; set; }
			public TimeSpan Start { get; set; }
			public TimeSpan Duration { get; set; }
		}
		
    }
}

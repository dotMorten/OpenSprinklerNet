using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OpenSprinklerApp
{
    public sealed partial class CalendarItem : UserControl
    {
        public CalendarItem()
        {
            this.InitializeComponent();			
        }

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(CalendarItem), new PropertyMetadata(null, OnTitlePropertyChanged));

		private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CalendarItem)d).title.Text = e.NewValue as string;
		}

		public Color Color
		{
			get { return (Color)GetValue(ColorProperty); }
			set { SetValue(ColorProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register("Color", typeof(Color), typeof(CalendarItem), new PropertyMetadata(Colors.Red, OnColorPropertyChanged));

		private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CalendarItem)d).color.Fill = new SolidColorBrush((Color)e.NewValue);
		}		
    }
}

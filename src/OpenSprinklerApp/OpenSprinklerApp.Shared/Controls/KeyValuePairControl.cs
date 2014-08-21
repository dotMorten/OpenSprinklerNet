using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace OpenSprinklerApp.Controls
{
	[Windows.UI.Xaml.Markup.ContentProperty(Name="Content")]
	public sealed class KeyValuePairControl : ContentControl
	{
		public KeyValuePairControl()
		{
			this.DefaultStyleKey = typeof(KeyValuePairControl);
		}

		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(string), typeof(KeyValuePairControl), new PropertyMetadata(null));
		
	}
}

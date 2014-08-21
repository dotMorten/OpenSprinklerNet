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
	public class TitleBasePage : ContentControl
	{
		public TitleBasePage()
		{
			this.DefaultStyleKey = typeof(TitleBasePage);
		}
#if !WINDOWS_PHONE_APP
		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			var btn = GetTemplateChild("backButton") as Windows.UI.Xaml.Controls.Primitives.ButtonBase;
			if(btn != null)
			{
				btn.Click += btn_Click;
			}
		}

		void btn_Click(object sender, RoutedEventArgs e)
		{
			FrameworkElement parent = this.Parent as FrameworkElement;
			while(parent != null && !(parent is Frame))
			{
				parent = parent.Parent as FrameworkElement;
			}
			if(parent is Frame)
			{
				var frame = (Frame)parent;
				if (frame.CanGoBack)
					frame.GoBack();
			}
		}
#endif


		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(TitleBasePage), new PropertyMetadata(""));

		
	}
}

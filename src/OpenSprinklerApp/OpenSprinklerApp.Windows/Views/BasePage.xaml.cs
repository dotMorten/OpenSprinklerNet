using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace OpenSprinklerApp.Views
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	[Windows.UI.Xaml.Markup.ContentProperty(Name="Contents")]
	public abstract partial class BasePage : Page
	{
		public BasePage()
		{
			this.InitializeComponent();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (!Frame.CanGoBack)
				backButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
		}

		private void backButton_Click(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack)
				Frame.GoBack();
		}

		public string PageTitle
		{
			get { return (string)GetValue(PageTitleProperty); }
			set { SetValue(PageTitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for PageTitle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty PageTitleProperty =
			DependencyProperty.Register("PageTitle", typeof(string), typeof(BasePage), new PropertyMetadata("", OnPageTitlePropertyChanged));

		private static void OnPageTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BasePage)d).pageTitle.Text = (string)e.NewValue;
		}

		public object Contents
		{
			get { return (object)GetValue(ContentsProperty); }
			set { SetValue(ContentsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ContentsProperty =
			DependencyProperty.Register("Contents", typeof(object), typeof(BasePage), new PropertyMetadata(null, OnContentsPropertyChanged));

		private static void OnContentsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BasePage)d).ContentArea.Content = e.NewValue;
		}
	}
}

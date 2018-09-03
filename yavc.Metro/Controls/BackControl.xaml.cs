using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace yavc.Metro.Controls {
	public sealed partial class BackControl : UserControl {
		public BackControl() {
			this.InitializeComponent();
		}

		private void Button_Back_Click(object sender, RoutedEventArgs e) {
			var page = GetPage(this as FrameworkElement);

			if (null == page) return;

			if (null != page.Frame && page.Frame.CanGoBack)
				page.Frame.GoBack();
		}

		private Page GetPage(FrameworkElement fe) {
			if (null == fe) return null;

			if (fe is Page)
				return fe as Page;


			return GetPage(fe.Parent as FrameworkElement);
		}
	}
}

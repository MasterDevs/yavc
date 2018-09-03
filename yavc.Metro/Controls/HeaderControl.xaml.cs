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

namespace yavc.Metro.Controls {
	public sealed partial class HeaderControl : UserControl {



		public double HeaderFontSize {
			get { return (double)GetValue(HeaderFontSizeProperty); }
			set { SetValue(HeaderFontSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for HeaderFontSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HeaderFontSizeProperty =
				DependencyProperty.Register("HeaderFontSize", typeof(double), typeof(HeaderControl), new PropertyMetadata(30));

		public double SubHeaderFontSize {
			get { return (double)GetValue(SubHeaderFontSizeProperty); }
			set { SetValue(SubHeaderFontSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SubHeaderFontSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SubHeaderFontSizeProperty =
				DependencyProperty.Register("SubHeaderFontSize", typeof(double), typeof(HeaderControl), new PropertyMetadata(26));



		public string HeaderText {
			get { return (string)GetValue(HeaderTextProperty); }
			set { SetValue(HeaderTextProperty, value); }
		}

		public static readonly DependencyProperty HeaderTextProperty =
				DependencyProperty.Register("HeaderText", typeof(string), typeof(HeaderControl), new PropertyMetadata(string.Empty));

		public string SubHeaderText {
			get { return (string)GetValue(SubHeaderTextProperty); }
			set { SetValue(SubHeaderTextProperty, value); }
		}
		public static readonly DependencyProperty SubHeaderTextProperty =
				DependencyProperty.Register("SubHeaderText", typeof(string), typeof(HeaderControl), new PropertyMetadata(null, SubHeaderTxtChanged));


		/// <summary>
		/// We need to hide the SubHeader control if no text is set.
		/// That way, there's not a huge gab between the header when no sub header is set.
		/// </summary>
		private static void SubHeaderTxtChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var str = e.NewValue as string;
			var ctrl = d as HeaderControl;
			ctrl.subHeader.Visibility = string.IsNullOrEmpty(str) ? Visibility.Collapsed : Visibility.Visible;
		}

		public HeaderControl() {
			this.InitializeComponent();
		}
	}
}

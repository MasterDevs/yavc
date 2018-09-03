using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace yavc.Phone.Controls {
	public partial class HeaderControl : UserControl {

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
			InitializeComponent();
		}
	}
}

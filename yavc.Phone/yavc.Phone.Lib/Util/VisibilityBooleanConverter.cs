using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace yavc.Phone.Lib.Util {
	public class VisibilityBooleanConverter : IValueConverter {

		public Visibility TrueVisibility { get; set; }
		public Visibility FalseVisibility { get; set; }
		public Visibility DefaultVisibility { get; set; }

		public VisibilityBooleanConverter() {
			TrueVisibility = Visibility.Visible;
			FalseVisibility = Visibility.Collapsed;
			DefaultVisibility = Visibility.Visible;
		}

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (true.Equals(value)) return TrueVisibility;
			else if (false.Equals(value)) return FalseVisibility;

			return DefaultVisibility;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException();
		}

		#endregion
	}
}

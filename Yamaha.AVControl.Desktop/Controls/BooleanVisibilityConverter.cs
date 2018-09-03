using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Yamaha.AVControl.Desktop.Controls {
	public class BooleanVisibilityConverter : IValueConverter{

		public Visibility FalseVisibility { get; set; }
		public Visibility TrueVisibility { get; set; }

		public BooleanVisibilityConverter() {
			FalseVisibility = Visibility.Collapsed;
			TrueVisibility = Visibility.Visible;
		}

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (value == null)
				return null;

			try {
				bool bVal = (bool)value;
				if (bVal)
					return TrueVisibility;
				else
					return FalseVisibility;
			} catch { return value; }
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException();
		}

		#endregion
	}
}

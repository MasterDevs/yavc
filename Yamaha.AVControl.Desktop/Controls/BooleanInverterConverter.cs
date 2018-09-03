using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Yamaha.AVControl.Desktop.Controls {
	public class BooleanInverterConverter : IValueConverter {
		
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (true.Equals(value))
				return false;
			else if (false.Equals(value))
				return true;
			else
				return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return Convert(value, targetType, parameter, culture);
		}

		#endregion
	}
}

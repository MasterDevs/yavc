using System;
using System.Windows.Data;
using System.Windows.Media;

namespace yavc.Phone.Lib.Util {
	public class ColorBooleanConverter : IValueConverter {

		public SolidColorBrush TrueColor { get; set; }
		public SolidColorBrush FalseColor { get; set; }
		public SolidColorBrush DefaultColor { get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (true.Equals(value)) return TrueColor;
			else if (false.Equals(value)) return FalseColor;
			return DefaultColor;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException();
		}

		#endregion
	}
}

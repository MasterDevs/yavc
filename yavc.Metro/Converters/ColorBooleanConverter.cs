using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace yavc.Metro.Converters {
	public class ColorBooleanConverter : IValueConverter {

		public SolidColorBrush TrueColor { get; set; }
		public SolidColorBrush FalseColor { get; set; }
		public SolidColorBrush DefaultColor { get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language) {
			if (true.Equals(value)) return TrueColor;
			else if (false.Equals(value)) return FalseColor;
			return DefaultColor;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotImplementedException();
		}

		#endregion
	}
}

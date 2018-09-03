using System;
using Windows.UI.Xaml.Data;

namespace yavc.Metro.Converters {
	public class InverterConverter : IValueConverter {
				
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language) {
			return !true.Equals(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotImplementedException();
		}

		#endregion
	}
}

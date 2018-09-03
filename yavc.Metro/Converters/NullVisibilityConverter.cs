using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace yavc.Metro.Converters {
	public class NullVisibilityConverter : IValueConverter {


		public Visibility NullVisibility { get; set; }
		public Visibility NotNullVisibility { get; set; }
		public Visibility DefaultVisibility { get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language) {
			if (null == value) return NullVisibility;
			else return NotNullVisibility;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotSupportedException();
		}

		#endregion
	}
}

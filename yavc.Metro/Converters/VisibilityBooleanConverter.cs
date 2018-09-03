using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace yavc.Metro.Converters {
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

		public object Convert(object value, Type targetType, object parameter, string language) {
			if (true.Equals(value)) return TrueVisibility;
			else if (false.Equals(value)) return FalseVisibility;

			return DefaultVisibility;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotImplementedException();
		}

		#endregion
	}
}

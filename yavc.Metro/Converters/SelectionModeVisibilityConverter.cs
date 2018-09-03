using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using yavc.Base.Models;

namespace yavc.Metro.Converters {
	public class SelectionModeVisibilityConverter : IValueConverter {

		public SelectionMode VisibleMode { get; set; }
		
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language) {
			if (null == value) return Visibility.Collapsed;

			try {
				var mode = (SelectionMode)value;

				if (mode == VisibleMode) return Visibility.Visible;

			} catch { }


			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotImplementedException();
		}

		#endregion
	}
}

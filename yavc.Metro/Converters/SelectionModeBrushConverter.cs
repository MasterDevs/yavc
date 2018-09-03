using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using yavc.Base.Models;

namespace yavc.Metro.Converters {
	public class SelectionModeBrushConverter : IValueConverter {

		public SelectionMode Mode { get; set; }
		public Brush Brush { get; set; }
		public Brush DefaultBrush { get; set; }
		
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language) {
			if (Mode.Equals(value))
				return Brush;

			return DefaultBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotImplementedException();
		}

		#endregion
	}
}

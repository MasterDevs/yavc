using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using yavc.Base.Util;

namespace yavc.Metro.Converters {
	public class ScaleConverter : IValueConverter {

		public double BaseValue { get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, string language) {
			try {
				var d = (double)value;

				var result = d * BaseValue * GetParamValue(parameter);

				if (targetType == typeof(Thickness))
					return new Thickness(result);

				return d * BaseValue * GetParamValue(parameter);
			} catch { return value; }
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			try {
				var d = (double)value;
				return d / BaseValue / GetParamValue(parameter);
			} catch { return value; }
		}

		#endregion

		public static double GetParamValue(object param) {
			if (param is double) return (double)param;

			var result = 1D;

			if (null != param)
				double.TryParse(param.ToString(), out result);
			
			return result;
		}
	}

	public class ScaleValue : DependencyObject {

		public double Value {
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
				DependencyProperty.Register("Value", typeof(double), typeof(ScaleValue), new PropertyMetadata(0D));

		public override string ToString() {
			return Value.ToString();
		}
	}

}

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using yavc.Base.Models;

namespace yavc.Metro.Controls {
	public sealed partial class ZoneControl : UserControl {

		public double ImageSize {
			get { return (double)GetValue(ImageSizeProperty); }
			set { SetValue(ImageSizeProperty, value); }
		}

		public static readonly DependencyProperty ImageSizeProperty =
				DependencyProperty.Register("ImageSize", typeof(double), typeof(ZoneControl), new PropertyMetadata(100));

		public double LargeFontSize {
			get { return (double)GetValue(ZoneNameFontSizeProperty); }
			set { SetValue(ZoneNameFontSizeProperty, value); }
		}
		public static readonly DependencyProperty ZoneNameFontSizeProperty =
				DependencyProperty.Register("LargeFontSize", typeof(double), typeof(ZoneControl), new PropertyMetadata(30));

		public double MediumFontSize {
			get { return (double)GetValue(MediumFontProperty); }
			set { SetValue(MediumFontProperty, value); }
		}

		public static readonly DependencyProperty MediumFontProperty =
				DependencyProperty.Register("MediumFontSize", typeof(double), typeof(ZoneControl), new PropertyMetadata(25));

		public ZoneControl() {
			this.InitializeComponent();
		}

		private void TogglePower_Click(object sender, RoutedEventArgs e) {
			var fe = sender as FrameworkElement;
			var zone = fe.DataContext as VMZone;
			zone.ToggelPower();
		}
	}
}

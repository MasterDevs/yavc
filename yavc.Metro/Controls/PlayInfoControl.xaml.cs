using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace yavc.Metro.Controls {
	public sealed partial class PlayInfoControl : UserControl {

		public double LargeFontSize {
			get { return (double)GetValue(LargeFontSizeProperty); }
			set { SetValue(LargeFontSizeProperty, value); }
		}

		public static readonly DependencyProperty LargeFontSizeProperty =
				DependencyProperty.Register("LargeFontSize", typeof(double), typeof(PlayInfoControl), new PropertyMetadata(30));

		public double MediumFontSize {
			get { return (double)GetValue(MediumFontSizeProperty); }
			set { SetValue(MediumFontSizeProperty, value); }
		}

		public static readonly DependencyProperty MediumFontSizeProperty =
				DependencyProperty.Register("MediumFontSize", typeof(double), typeof(PlayInfoControl), new PropertyMetadata(20));


		public PlayInfoControl() {
			this.InitializeComponent();
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using yavc.Base.Models;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace yavc.Metro.Controls {
	public sealed partial class VolumeControl : UserControl {
		public VolumeControl() {
			this.InitializeComponent();
		}

		private VMVolume GetVM() {
			return DataContext as VMVolume;
		}

		private void SetVolumeToSliderValue() {
			var vm = GetVM();
			if (null != vm) {
				vm.ChangeVolume(volSlder.Value);
			}
		}

		double previousValue = 0;
		private void volSlder_ValueChanged_1(object sender, RangeBaseValueChangedEventArgs e) {
			var val = volSlder.Value;
			if (Math.Abs(val - previousValue) > 1)
				SetVolumeToSliderValue();
			previousValue = val;
		}

		private void Button_ToggleMute(object sender, RoutedEventArgs e) {
			var vm = GetVM();
			vm.ToggleMute();
		}
	}
}

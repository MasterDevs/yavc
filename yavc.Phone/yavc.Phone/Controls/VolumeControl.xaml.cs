using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using yavc.Base.Models;

namespace yavc.Phone.Controls {
	public partial class VolumeControl : UserControl {

		public VolumeControl() {
			InitializeComponent();
		}

		private VMVolume GetVM() {
			return DataContext as VMVolume;
		}

		private void volSlder_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e) {
			var vm = GetVM();
			if (null != vm) {
				vm.ChangeVolume(volSlder.Value);
			}
		}

		private void VolumeDown_Click(object sender, RoutedEventArgs e) {
			var vm = GetVM();
			if (null == vm) return;

			vm.VolumeDown();
		}

		private void VolumeUp_Click(object sender, RoutedEventArgs e) {
			var vm = GetVM();
			if (null == vm) return;

			vm.VolumeUp();
		}
	}
}

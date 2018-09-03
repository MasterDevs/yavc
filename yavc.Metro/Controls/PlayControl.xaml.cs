using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
	public sealed partial class PlayControl : UserControl {

		public PlayControl() {
			this.InitializeComponent();
		}

		private void prevBtn_Click(object sender, RoutedEventArgs e) {
			InvokeCmd(pb => pb.Previous());
		}

		private void playBtn_Click(object sender, RoutedEventArgs e) {
			InvokeCmd(pb => pb.Play());
		}

		private void pauseBtn_Click(object sender, RoutedEventArgs e) {
			InvokeCmd(pb => pb.Pause());
		}

		private void stopBtn_Click(object sender, RoutedEventArgs e) {
			InvokeCmd(pb => pb.Stop());
		}

		private void nextBtn_Click(object sender, RoutedEventArgs e) {
			InvokeCmd(pb => pb.Next());
		}

		private void InvokeCmd(Action<VMPlayback> action) {
			var vm = DataContext as VMMain;
			if (null != vm)
				action(vm.SelectedZone.Playback);
		}
	}
}

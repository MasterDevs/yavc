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
	public partial class PlayControl : UserControl {

		public PlayControl() {
			InitializeComponent();
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
			var vm = DataContext as VMPlayback;
			if (null != vm)
				action(vm);
		}
	}
}

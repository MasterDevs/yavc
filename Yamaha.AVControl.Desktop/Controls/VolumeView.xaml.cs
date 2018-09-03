using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Yamaha.AVControl.Desktop.Models;

namespace Yamaha.AVControl.Desktop.Controls {
	/// <summary>
	/// Interaction logic for VolumeView.xaml
	/// </summary>
	public partial class VolumeView : UserControl {
		public VolumeView() {
			InitializeComponent();
		}

		private void VolumeDown_Click(object sender, RoutedEventArgs e) {
			VMVolume model = DataContext as VMVolume;
			if (model != null) {
				model.VolumeDown();
			}
		}

		private void VolumeUp_Click(object sender, RoutedEventArgs e) {
			VMVolume model = DataContext as VMVolume;
			if (model != null) {
				model.VolumeUp();
			}
		}

		private void ToggleMute_Click(object sender, RoutedEventArgs e) {
			VMVolume model = DataContext as VMVolume;
			if (model != null) {
				model.ToggleMute();
			}
		}

		double PreviousValue;
		private void Slider_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
			PreviousValue = volSlider.Value;
		}

		private void Slider_PreviewMouseUp(object sender, MouseButtonEventArgs e) {
			if (PreviousValue != volSlider.Value) {
				VMVolume model = DataContext as VMVolume;
				if (model != null) {
					model.ChangeVolume(volSlider.Value);
				}
			}
		}
	}
}

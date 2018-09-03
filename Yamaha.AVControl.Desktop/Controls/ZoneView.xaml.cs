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
	/// Interaction logic for ZoneView.xaml
	/// </summary>
	public partial class ZoneView : UserControl {
		public ZoneView() {
			InitializeComponent();	
		}

		private void Power_Click(object sender, RoutedEventArgs e) {
			VMZone model = DataContext as VMZone;
			if (model != null)
				model.ToggelPower();
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			VMZone model = DataContext as VMZone;
			if (model != null)
				model.Refresh();
		}
	}
}

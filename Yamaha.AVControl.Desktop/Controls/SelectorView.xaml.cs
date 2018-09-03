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
	/// Interaction logic for SelectorView.xaml
	/// </summary>
	public partial class SelectorView : UserControl {
		public SelectorView() {
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			Button b = sender as Button;
			VMSelectable selected = b.Tag as VMSelectable;

			if (selected != null) {
				selected.Select();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e) {
			MessageBox.Show("OK");
		}
	}
}

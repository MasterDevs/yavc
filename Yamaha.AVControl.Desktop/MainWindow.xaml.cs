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

namespace Yamaha.AVControl.Desktop {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private VMMain MainModel;

		public MainWindow() {
			InitializeComponent();
			MainModel = VMMain.Empty;
			DataContext = MainModel;
		}

		private void Connect_Click(object sender, RoutedEventArgs e) {
			MainModel.UpdateHostNameOrAddresss(tbHost.Text);
			if (!MainModel.IsConnected) {
				puDialog.IsOpen = true;
				puOkButton.Focus();
			}
		}

		private void puDialog_Click(object sender, RoutedEventArgs e) {
			puDialog.IsOpen = false;
		}
	}
}

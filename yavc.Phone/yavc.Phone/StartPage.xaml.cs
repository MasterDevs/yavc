using System;
using System.Windows;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using yavc.Base.Models;
using yavc.Phone.Controls;
using yavc.Base.Data;

namespace yavc.Phone {
	public partial class StartPage : TransitionPage {
		private VMStart StartVM;
		private ProgressIndicator statusIndicator;

		public StartPage() {
			InitializeComponent();

			SessionManager.GetVMStart(vm =>
			{
				UI.Invoke(() =>
				{
					DataContext = StartVM = vm;
					statusIndicator = new ProgressIndicator();
					statusIndicator.IsIndeterminate = true;
					statusIndicator.Text = StartVM.AddingDeviceStatus;
					statusIndicator.IsVisible = false;
					SystemTray.SetProgressIndicator(this, statusIndicator);

					//-- If we have no devices when this page loads, try to find some.
					if (StartVM._Devices.Count == 0)
						Search_Click(this, EventArgs.Empty);
				});
			});
		}

		private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e) {
			if (null == SessionManager.AddedDevice) return;

			StartVM.TryAdd(SessionManager.AddedDevice);
			SessionManager.AddedDevice = null; //-- Reset added device
		}

		private void OnAddFailure(string errorMessage) {
			UI.Invoke(() =>
			{
				statusIndicator.IsVisible = false;
				MessageBox.Show(errorMessage);
			});
		}

		private void OnAddProgressChanged() {
			statusIndicator.IsVisible = true;
			statusIndicator.IsIndeterminate = false;
			statusIndicator.Text = StartVM.AddingDeviceStatus;
			statusIndicator.Value = StartVM.AddingDevicePercentage;
		}

		#region Event Handlers
		private void AddReceiver_Click(object sender, EventArgs e) {
			NavigationService.Navigate(new Uri("/AddDevicePage.xaml", UriKind.Relative));
		}

		private void PinToStart_Click(object sender, RoutedEventArgs e) {
			var fe = sender as FrameworkElement;
			var d = fe.DataContext as VMDevice;

			StartVM.CreateDeviceTile(d.Device);
		}

		private void RemoveDevice_Click(object sender, RoutedEventArgs e) {
			var fe = sender as FrameworkElement;
			var d = fe.DataContext as VMDevice;

			var result = MessageBox.Show("Are you sure you want to remove the following device?\n\n" + d.FriendlyName, "Confirm Remove", MessageBoxButton.OKCancel);

			if (result != MessageBoxResult.OK) return;

			StartVM.Remove(d);
			DataContext = null;
			DataContext = StartVM;
		}

		private void Refresh_Click(object sender, EventArgs e) {
			DataContext = null;
			DataContext = StartVM;
			StartVM.Refresh();
		}

		private void Review_Click(object sender, EventArgs e) {
			var reviewTask = new MarketplaceReviewTask();
			reviewTask.Show();
		}

		private void Search_Click(object sender, EventArgs e) {
			statusIndicator.IsVisible = true;
			statusIndicator.IsIndeterminate = true;
			statusIndicator.Text = "Searching for devices . . .";
			StartVM.TryFind(UI.Wrap(() =>
			{
				statusIndicator.IsVisible = false;
			}));
		}

		private void ViewDevice_Click(object sender, RoutedEventArgs e) {
			var fe = sender as FrameworkElement;
			var d = fe.DataContext as VMDevice;
			if (d.IsLoaded) {
				//App.ViewModel = SessionManager.GetVMMain(d.Device);
				SessionManager.GetVMMain(d.Device, vm =>
				{
					UI.Invoke(() =>
					{
						App.ViewModel = vm;
						NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));						
					});
				});
			}
		}
		#endregion
	}
}

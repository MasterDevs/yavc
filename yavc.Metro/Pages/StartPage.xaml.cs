using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using yavc.Base;
using yavc.Base.Data;
using yavc.Base.Models;
using yavc.Metro.Imp;

namespace yavc.Metro.Pages {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StartPage : LayoutAwarePage {
		
		VMStart StartVM;

		public StartPage() {
			this.InitializeComponent();
			
			SessionManager.GetVMStart(vm => UI.Invoke(() => DataContext = StartVM = vm));
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e) {
			if (SessionManager.AddedDevice == null) {
				return;
			} else {
				SessionManager.GetVMStart(vm => UI.Invoke(() =>
				{
					DataContext = StartVM = vm;
					StartVM.TryAdd(SessionManager.AddedDevice);
					SessionManager.AddedDevice = null; //-- Reset added device
				}));
			}
		}

		private void Button_AddDevice_Click(object sender, RoutedEventArgs e) {
			Frame.Navigate(typeof(AddDevicePage));
		}

		private void Button_Refresh_Click(object sender, RoutedEventArgs e) {
			StartVM.Refresh();
		}

		private void ViewDevice_Click(object sender, RoutedEventArgs e) {
			var fe = sender as FrameworkElement;
			var d = fe.DataContext as VMDevice;
			if (d.IsLoaded) {
				SessionManager.GetVMMain(d.Device, main => {
					UI.Invoke(() =>
					{
						App.ViewModel = main;
						Frame.Navigate(typeof(ViewDevicePage), App.ViewModel);
					});
				});
			}
		}
	}
}

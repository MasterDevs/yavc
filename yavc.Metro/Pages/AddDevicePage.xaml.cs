using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using yavc.Base.Data;
using yavc.Base.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace yavc.Metro.Pages {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AddDevicePage : LayoutAwarePage {

		VMStart StartVM;

		public AddDevicePage() {
			this.InitializeComponent();
			tbIP.Focus(Windows.UI.Xaml.FocusState.Keyboard);

			SessionManager.GetVMStart(vm => UI.Invoke(() => DataContext = StartVM = vm));
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e) {
		}

		private void ButtonTryAdd_Click(object sender, RoutedEventArgs e) {
			SessionManager.AddedDevice = new Device(tbIP.Text, tbFriendlyName.Text);
			Frame.GoBack();
		}
	}
}

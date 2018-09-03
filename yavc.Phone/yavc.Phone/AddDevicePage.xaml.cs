using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Shell;
using yavc.Base.Data;
using yavc.Phone.Controls;

namespace yavc.Phone {
	public partial class AddDevicePage : TransitionPage {

		ApplicationBarIconButton abTryAdd;

		public AddDevicePage() {
			InitializeComponent();
			SetupAppBarButtons();
		}

		/// <summary>
		/// This is necessary because of the fact that you can't access an ApplicationBarIconButton
		/// that's defined in XAML.
		/// See http://blogs.msdn.com/b/ptorr/archive/2010/06/18/why-are-the-applicationbar-objects-not-frameworkelements.aspx?wa=wsignin1.0
		/// </summary>
		private void SetupAppBarButtons() {
			ApplicationBar appBar = new ApplicationBar();

			abTryAdd = new ApplicationBarIconButton(new Uri("Images/appbar.check.rest.png", UriKind.Relative));
			abTryAdd.Click += new EventHandler(TryAdd_Click);
			abTryAdd.Text = "Try Add";
			appBar.Buttons.Add(abTryAdd);

			ApplicationBarIconButton CancelEdit = new ApplicationBarIconButton(new Uri("Images/appbar.cancel.rest.png", UriKind.Relative));
			CancelEdit.Click += new EventHandler(Cancel_Click);
			CancelEdit.Text = "Cancel";
			appBar.Buttons.Add(CancelEdit);

			ApplicationBar = appBar;
		}

		private void AddDevicePage_Loaded(object sender, RoutedEventArgs e) {
			theAdder.Start();
		}

		private void TryAdd_Click(object sender, EventArgs e) {
			SessionManager.AddedDevice = new Device(theAdder.IP, theAdder.FriendlyName);
			NavigationService.GoBack();
		}

		private void Cancel_Click(object sender, EventArgs e) {
			SessionManager.AddedDevice = null;
			NavigationService.GoBack();
		}

		private void PhoneApplicationPage_KeyUp(object sender, KeyEventArgs e) {
			abTryAdd.IsEnabled = theAdder.IPEntered;
		}
	}
}
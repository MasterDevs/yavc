using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using yavc.Base;
using yavc.Base.Models;
using yavc.Metro.Converters;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace yavc.Metro.Pages {

	public static class FrameworkElementExtensions {
		public static object TryFindResource(this FrameworkElement element, object resourceKey) {
			var currentElement = element;

			while (currentElement != null) {
				try {
					return currentElement.Resources[resourceKey];
				} catch { }
				currentElement = currentElement.Parent as FrameworkElement;
			}

			return Application.Current.Resources[resourceKey];
		}
	}

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ViewDevicePage : LayoutAwarePage {

		VMMain MainVM;

		public ViewDevicePage() {
			this.InitializeComponent();
			this.Loaded += (s, e) => UpdateSelectionMode();
		}
		
		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e) {
			if (e.Parameter != null) {
				DataContext = MainVM = e.Parameter as VMMain;
				MainVM.PropertyChanged += (s, a) =>
				{
					if (a.PropertyName == "SelectionMode")
						UpdateSelectionMode();
				};
                MainVM.RefreshSelectedZone(false);
			}
		}

		private void UpdateSelectionMode() {
			switch (MainVM.SelectionMode) {
				case yavc.Base.Models.SelectionMode.Input:
					accInputs.Show();
					break;
				case yavc.Base.Models.SelectionMode.Scene:
					accScene.Show();
					break;
				case yavc.Base.Models.SelectionMode.DSP:
					accDSP.Show();
					break;
				case yavc.Base.Models.SelectionMode.Play:
					accPlayInfo.Show();
					break;
				case yavc.Base.Models.SelectionMode.Zone:
					accZone.Show();
					break;
				case yavc.Base.Models.SelectionMode.None:
				default:
					break;
			}
		}

		private void Home_Click(object sender, RoutedEventArgs e) {
			NavigationHelper.GoHome();
		}

		private void PinDevice_Click(object sender, RoutedEventArgs e) {
			Factory.TileService.CreateOrUpdateTile(MainVM.TheController.Device);
		}

		private void PinSelectedZone_Click(object sender, RoutedEventArgs e) {
			MainVM.SelectedZone.PinToStart();
		}

		private void Refresh_Click(object sender, RoutedEventArgs e) {
            MainVM.RefreshSelectedZone(true);
		}
		
		private void Select_Click(object sender, RoutedEventArgs e) {
			var fe = sender as FrameworkElement;
			var selectable = fe.DataContext as VMSelectable;
			if (selectable != null)
				selectable.Select();
		}
	}
}

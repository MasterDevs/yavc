using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using yavc.Base.Models;
using yavc.Phone.Controls;

namespace yavc.Phone {
	public partial class PickZonePage : TransitionPage {
		public PickZonePage() {
			if (App.ViewModel != null)
				DataContext = App.ViewModel;
			
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			var b = sender as Button;
			var z = b.Content as VMZone;

			z.Select();
			NavigationService.GoBack();
		}
	}
}
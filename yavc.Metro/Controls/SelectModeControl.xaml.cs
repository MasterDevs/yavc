using Windows.UI.Xaml;
using yavc.Base.Models;

namespace yavc.Metro.Controls {
	public sealed partial class SelectModeControl : Windows.UI.Xaml.Controls.UserControl {

		private VMMain MainVM { get { return DataContext as VMMain; } }
		
		public SelectModeControl() {
			this.InitializeComponent();
		}

        private void DSP_Click(object sender, RoutedEventArgs e) {
			MainVM.SelectionMode = SelectionMode.DSP;
		}

		private void Inputs_Click(object sender, RoutedEventArgs e) {
			MainVM.SelectionMode = SelectionMode.Input;
		}

		private void Scenes_Click(object sender, RoutedEventArgs e) {
			MainVM.SelectionMode = SelectionMode.Scene;
		}

		private void Zone_Click(object sender, RoutedEventArgs e) {
			MainVM.SelectionMode = SelectionMode.Zone;
		}

		private void Play_Click(object sender, RoutedEventArgs e) {
			MainVM.SelectionMode = SelectionMode.Play;
		}
	}
}

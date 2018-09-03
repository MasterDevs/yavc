using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace yavc.Metro.Pages {
	public class LayoutAwarePage : Page {

		public LayoutAwarePage() {
			this.Loaded += (s, e) =>
			{
				InitializeView();
				Window.Current.SizeChanged += Current_SizeChanged;
			};

			this.Unloaded += (s, e) =>
			{
				Window.Current.SizeChanged -= Current_SizeChanged;
			};
		}

		private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e) {
			InitializeView();
		}
		
		protected void InitializeView() {
			switch (ApplicationView.Value) {
				case ApplicationViewState.Filled:
				case ApplicationViewState.FullScreenLandscape:
				case ApplicationViewState.FullScreenPortrait:
					VisualStateManager.GoToState(this, "FullScreenLandscape", false);
					break;
				case ApplicationViewState.Snapped:
					VisualStateManager.GoToState(this, "Snapped", false);
					break;
			}
		}
	}
}

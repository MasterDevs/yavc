using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using yavc.Base.Data;
using yavc.Base.Models;
using yavc.Phone.Controls;
using System.Windows.Threading;

namespace yavc.Phone {
	public partial class MainPage : TransitionPage {

		private ApplicationBarIconButton muteButton;
		private ApplicationBarIconButton listButton;
		private ApplicationBarMenuItem powerBtn;
		private ApplicationBarMenuItem reviewBtn; 
		private DispatcherTimer autoRefreshTimer;
		private ProgressIndicator refreshIndicator;
		private readonly Uri MuteOnImg = new Uri("Images/appbar.muted.rest.png", UriKind.Relative);
		private readonly Uri MuteOffImg = new Uri("Images/appbar.volume.rest.png", UriKind.Relative);
		private readonly Uri RefreshImg = new Uri("Images/appbar.sync.rest.png", UriKind.Relative);
		private readonly Uri ListImg = new Uri("Images/appbar.lines.horizontal.4.png", UriKind.Relative);

		public MainPage() {
			InitializeComponent();

			InitializeAppBarButtons();

			AttachToVMEvents();
			InitializeRefresh();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			base.OnNavigatedTo(e);

			if (App.ViewModel != null) {
				RefreshLocal();
				App.ViewModel.RefreshSelectedZone(true);
                autoRefreshTimer.Start();
				return;
			}

			var uri = e.Uri.ToString();
			var qs = e.Uri.QueryStrings();
			if (qs.Count == 0 || !qs.ContainsKey(Device.PageUriKey)) return;

			var ip = qs[Device.PageUriKey];

			SessionManager.GetVMMain(ip, vm =>
			{
				UI.Invoke(() =>
				{
					App.ViewModel = vm;
					
					AttachToVMEvents();

					//-- Check if we were also passed a zone
					if (qs.ContainsKey(Zone.PageUriKey)) {
						var z = qs[Zone.PageUriKey];
						App.ViewModel.SelectZone(z);
					}
					App.ViewModel.RefreshSelectedZone(true);
					RefreshLocal();
                    autoRefreshTimer.Start();
				});
			});
		}

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            autoRefreshTimer.Stop();
        }

		#region Event Handlers
        private void listButton_Click(object sender, EventArgs e) {
			if (App.ViewModel.SelectedZone.List.CanList) {
				NavigationService.Navigate(new Uri("/BrowsePage.xaml", UriKind.Relative));
			}
		}

		private void PickZoneButton_Click(object sender, RoutedEventArgs e) {
			var b = sender as Button;
			var z = b.Content as VMZone;

			NavigationService.Navigate(new Uri("/PickZonePage.xaml", UriKind.Relative));
		}

		private void Refresh_Click(object sender, EventArgs e) {
			App.ViewModel.RefreshSelectedZone(true);
		}

		private void Review_Click(object sender, EventArgs e) {
			var reviewTask = new MarketplaceReviewTask();
			reviewTask.Show();
		}

		private void Select_Click(object sender, RoutedEventArgs e) {
			var b = sender as Button;
			var s = b.Tag as VMSelectable;

			if (s != null)
				s.Select();
		}

		private void ToggleMute_Click(object sender, EventArgs e) {
			var z = App.ViewModel.SelectedZone;
			z.Volume.ToggleMute();
			RefreshLocal();
		}

		private void TogglePower_Click(object sender, EventArgs e) {
			if (App.ViewModel == null || App.ViewModel.SelectedZone == null) return;
			App.ViewModel.SelectedZone.ToggelPower();
			RefreshLocal();
		}

		private void ViewModel_NetworkTrafficFinished(object sender, EventArgs e) {
			refreshIndicator.IsVisible = false;
			RefreshLocal();
		}

		private void ViewModel_NetworkTrafficStarted(object sender, EventArgs e) {
			refreshIndicator.IsVisible = true;
		}
		#endregion

		#region Helpers
		private void AttachToVMEvents() {
			if (App.ViewModel != null) {
				DataContext = App.ViewModel;
				App.ViewModel.NetworkTrafficStarted += new EventHandler(ViewModel_NetworkTrafficStarted);
				App.ViewModel.NetworkTrafficFinished += new EventHandler(ViewModel_NetworkTrafficFinished);
			}
		}

        private void InitializeAppBarButtons() {
			var appBar = new ApplicationBar();

			powerBtn = new ApplicationBarMenuItem();
			powerBtn.Click += new EventHandler(TogglePower_Click);
			powerBtn.Text = "On";
			appBar.MenuItems.Add(powerBtn);

			reviewBtn = new ApplicationBarMenuItem();
			reviewBtn.Click += new EventHandler(Review_Click);
			reviewBtn.Text = "Review this app";
			appBar.MenuItems.Add(reviewBtn);

			muteButton = new ApplicationBarIconButton(MuteOffImg);
			muteButton.Click += new EventHandler(ToggleMute_Click);
			muteButton.Text = "Mute";
			appBar.Buttons.Add(muteButton);
			
			var refreshBtn = new ApplicationBarIconButton(RefreshImg);
			refreshBtn.Click += new EventHandler(Refresh_Click);
			refreshBtn.Text = "Refresh";
			appBar.Buttons.Add(refreshBtn);

			listButton = new ApplicationBarIconButton(ListImg);
			listButton.Click += new EventHandler(listButton_Click);
			listButton.Text = "list";
			appBar.Buttons.Add(listButton);

			ApplicationBar = appBar;
		}

		private void InitializeRefresh() {
			//-- Refresh indicator
			// This indicator will notify the user when the first browse to this page
			// as well as whenever they manually press the refresh button.
			refreshIndicator = new ProgressIndicator();
			refreshIndicator.IsIndeterminate = true;
			refreshIndicator.Text = "Refreshing";
			refreshIndicator.IsVisible = false;
			SystemTray.SetProgressIndicator(this, refreshIndicator);

			//-- Auto Refresh Timer
			autoRefreshTimer = new DispatcherTimer();
			autoRefreshTimer.Interval = TimeSpan.FromSeconds(5D);
            autoRefreshTimer.Tick += (s, e) =>
            {
                if (App.ViewModel != null)
                    App.ViewModel.RefreshSelectedZone(false);
            };
			autoRefreshTimer.Start();
		}

		/// <summary>
		/// Unfortunately, Application Bar Items can not 
		/// have properties bound. As a result, when something
		/// in our View Model changes, we need to ensure our app
		/// bar is up to date as well.
		/// </summary>
		private void RefreshLocal() {
			var z = App.ViewModel.SelectedZone;
			if (null == z) return;
			if (z.Volume.Muted) {
				muteButton.IconUri = MuteOnImg;
				muteButton.Text = "mute off";
			} else {
				muteButton.IconUri = MuteOffImg;
				muteButton.Text = "mute";
			}
			powerBtn.Text = z.IsOn ? "power off" : "power on";
			
			muteButton.IsEnabled = z.IsOn;
						
			//-- The only way to hide a Pivot Item is by removing them.
			UpdatePivot(z.CanSelectDSP, dspPivot);
			UpdatePivot(z.CanSelectScene, scenesPivot);
			UpdatePivot(z.CanViewPlayback, playPivot);
			if (z.CanList) {
				if (!ApplicationBar.Buttons.Contains(listButton))
					ApplicationBar.Buttons.Add(listButton);
			} else {
				ApplicationBar.Buttons.Remove(listButton);
			}
		}
		
		private void UpdatePivot(bool shouldExist, PivotItem item) {
			if (shouldExist) {
				if (!thePivot.Items.Contains(item))
					thePivot.Items.Add(item);
			} else {
				thePivot.Items.Remove(item);
			}
		}
		#endregion
	}
}
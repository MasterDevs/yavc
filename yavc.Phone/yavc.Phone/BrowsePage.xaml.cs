using System;
using Microsoft.Phone.Shell;
using yavc.Base.Models;
using yavc.Phone.Controls;

namespace yavc.Phone {
	public partial class BrowsePage : TransitionPage {

		private ApplicationBarIconButton upLevelButton;
		private ApplicationBarIconButton refreshButton;
		private VMBrowse Browse;

		public BrowsePage() {
			InitializeComponent();
			InitializeApplicationBar();
			DataContext = Browse = new VMBrowse(App.ViewModel, App.ViewModel.SelectedZone.SelectedInput, App.ViewModel.SelectedZone.TheZone);
			Browse.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(List_PropertyChanged);
			Browse.OnSelectedItem += new EventHandler(List_OnSelectedItem);
			Browse.Refresh();
		}

		private void List_OnSelectedItem(object sender, EventArgs e) {
            NavigationService.GoBack();
		}

		private void List_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			UI.Invoke(() => upLevelButton.IsEnabled = Browse.CanGoBack);
		}

		private void InitializeApplicationBar() {
			var appBar = new ApplicationBar();

			upLevelButton = new ApplicationBarIconButton(new Uri("Images/appbar.arrow.up.png", UriKind.Relative));
			upLevelButton.Click += new EventHandler(upLevelButton_Click);
			upLevelButton.Text = "up a level";
			appBar.Buttons.Add(upLevelButton);

			refreshButton = new ApplicationBarIconButton(new Uri("Images/appbar.sync.rest.png", UriKind.Relative));
			refreshButton.Click += new EventHandler(refreshButton_Click);
			refreshButton.Text = "refresh";
			appBar.Buttons.Add(refreshButton);

			ApplicationBar = appBar;
		}

		private void refreshButton_Click(object sender, EventArgs e) {
			Browse.Refresh();
		}

		private void upLevelButton_Click(object sender, EventArgs e) {
			Browse.Back();
		}
	}
}
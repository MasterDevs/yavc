using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using yavc.Base.Data;

namespace yavc.Metro.Pages {
	public static class NavigationHelper {

		/// <summary>
		/// Gets a value indicating what character is used to split a key value pair. Below is the value:<para>
		/// =</para>
		/// </summary>
		public static char KeyValueSplit { get { return '='; } }
		/// <summary>
		/// Gets a value indicating what character is used to split groups of key value pairs. Below is the value:<para>
		/// &amp;</para>
		/// </summary>
		public static char KeyValueGroupSplit { get { return '&'; } }

		public static void GoHome() {
			Navigate(CurrentFrame, typeof(StartPage), null);
		}

		/// <summary>
		/// Navigates the currently running application based on the specified
		/// args. Used to navigate to a particular tile.
		/// </summary>
		public static void Navigate(LaunchActivatedEventArgs args) {
			Navigate(CurrentFrame, args);
		}

		public static void Launch(LaunchActivatedEventArgs args) {
			Navigate(new Frame(), args);
		}

		#region Helper Methods
		private static Frame CurrentFrame { get { return Window.Current.Content as Frame; } }

		private static Type GetPage(Dictionary<string, string> startupInfo) {
			var page = typeof(StartPage);

			if (startupInfo.ContainsKey(Device.PageUriKey))
				page = typeof(ViewDevicePage);

			return page;
		}

		private static Dictionary<string, string> GetStartupInfo(string args) {
			var startupInfo = new Dictionary<string, string>();

			if (string.IsNullOrEmpty(args)) return startupInfo;

			var groups = args.Split(KeyValueGroupSplit);
			foreach (var i in groups) {
				var keyval = i.Split(KeyValueSplit);
				startupInfo[keyval[0]] = keyval[1];
			}

			return startupInfo;
		}

		private static void Navigate(Frame f, LaunchActivatedEventArgs args) {
			var startupInfo = GetStartupInfo(args.Arguments);
			var page = GetPage(startupInfo);

			if (startupInfo.ContainsKey(Device.PageUriKey)) {
				SessionManager.GetVMMain(startupInfo[Device.PageUriKey], main =>
				{
					UI.Invoke(() =>
					{
						App.ViewModel = main;
						
						if (startupInfo.ContainsKey(Zone.PageUriKey))
							main.SelectZone(startupInfo[Zone.PageUriKey]);

						Navigate(f, page, main);
					});
				});
			} else {
				Navigate(f, page, null);
			}
		}

		private static void Navigate(Frame f, Type page, object param) {
			if (!f.Navigate(page, param))
				throw new Exception("Failed to navigate to page");

			// Place the frame in the current Window and ensure that it is active
			Window.Current.Content = f;
			Window.Current.Activate();
		} 
		#endregion
	}
}

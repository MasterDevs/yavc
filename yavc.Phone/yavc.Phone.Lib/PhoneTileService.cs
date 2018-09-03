using System;
using System.Windows;
using Microsoft.Phone.Shell;
using yavc.Base.Data;
using yavc.Base;

namespace yavc.Phone.Lib {
	public class PhoneTileService : ITileService {

		#region Public
		public void CreateOrUpdateTile(Device d) {
			CreateOrUpdateTile(d.FriendlyName, GetMainPageUri(d, null));
		}
		public void CreateOrUpdateTile(Device d, Zone zone) {
			CreateOrUpdateTile(string.Format("{0} - {1}", d.FriendlyName, zone.DisplayName), GetMainPageUri(d, zone));
		}

		/// <summary>
		/// This method will remove all secondary tiles that correspond to this device.
		/// This includes Zone Specific tiles.
		/// </summary>
		public void DeleteAllTilesForDevice(Device d) {
			var uri = GetMainPageUri(d, null);
			foreach (var st in ShellTile.ActiveTiles) {
				var keys = st.NavigationUri.QueryStrings();
				if(keys.ContainsKey(Device.PageUriKey) && keys[Device.PageUriKey] == d.HostnameOrIp) {
					st.Delete();
				}
			}
		}

		public void UpdateTileIfExists(Device d) {
			var uri = GetMainPageUri(d, null);
			foreach (var tile in ShellTile.ActiveTiles) {
				if (tile.NavigationUri == uri) {
					CreateOrUpdateTile(d);
					return;
				}
			}
		}
		#endregion

		#region Private
		private static Uri GetMainPageUri(Device d, Zone z) {
			var uri = string.Format("/MainPage.xaml?{0}={1}", Device.PageUriKey, d.HostnameOrIp);
			if (null != z)
				uri = uri + string.Format("&{0}={1}", Zone.PageUriKey, z.Name);

			return new Uri(Uri.EscapeUriString(uri), UriKind.Relative);
		}
		private static void CreateOrUpdateTile(string title, Uri uri) {
			var tiledata = new StandardTileData()
			{
				BackgroundImage = new Uri("Background.png", UriKind.Relative),
				Title = title
			};

			//-- If we already have a Shell Tile for this pageUri, we can just
			// update the tile.
			foreach (var st in ShellTile.ActiveTiles) {
				if (st.NavigationUri == uri) {
					st.Update(tiledata);

					//-- Notify the user the tile was created.
					MessageBox.Show(string.Format("Successfully updated {0} tile.", title));
					return;
				}
			}

			//-- If this is a new Shell Tile, then lets go create one!
			ShellTile.Create(uri, tiledata);
		}
		#endregion
	}
}

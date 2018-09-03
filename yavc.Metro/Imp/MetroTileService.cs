using System;
using Windows.Foundation;
using Windows.UI.StartScreen;
using yavc.Base;
using yavc.Base.Data;
using yavc.Metro.Pages;

namespace yavc.Metro.Imp {
	public class MetroTileService : ITileService {
		#region ITileService Members

		public void CreateOrUpdateTile(Device device) {
			CreateOrUpdateTile(GetId(device, null), device.FriendlyName, GetArguments(device, null));
		}

		public void CreateOrUpdateTile(Device device, Zone zone) {
			if (null == device) throw new ArgumentNullException("device");
			if (null == zone) throw new ArgumentNullException("zone");
			CreateOrUpdateTile(GetId(device, zone), string.Format(@"{0} - {1}", device.FriendlyName, zone.DisplayName), GetArguments(device, zone));
		}

		public async void DeleteAllTilesForDevice(Device device) {
			foreach (var tile in await SecondaryTile.FindAllForPackageAsync()) {
				if(tile.TileId.Contains(device.HostnameOrIp))
					await tile.RequestDeleteAsync();
			}
		}

		public async void UpdateTileIfExists(Device d) {
			var id = GetId(d, null);
			foreach (var tile in await SecondaryTile.FindAllForPackageAsync()) {
				if (id == tile.TileId)
					CreateOrUpdateTile(d);
			}
		}

		#endregion

		private string GetId(Device device, Zone zone) {
			if (zone == null)
				return device.HostnameOrIp;
			else
				return string.Format("{0}_{1}", device.HostnameOrIp, zone.Name);
		}

		private static async void CreateOrUpdateTile(string id, string name, string args) {
			var tile = new SecondaryTile(id, name, name, args, TileOptions.CopyOnDeployment | TileOptions.ShowNameOnLogo, logo);
			await tile.RequestCreateAsync();
		}

		private static string GetArguments(Device d, Zone z) {
			var args = string.Format(@"{0}{1}{2}", Device.PageUriKey, NavigationHelper.KeyValueSplit, d.HostnameOrIp);

			if (null != z)
				args = args + string.Format("{0}{1}{2}{3}", NavigationHelper.KeyValueGroupSplit, Zone.PageUriKey, NavigationHelper.KeyValueSplit, z.Name);

			return args;
		}

		private static Uri logo { get { return new Uri(@"ms-appx:///assets/Logo_NoText.png"); } }
	}
}

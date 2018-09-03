using System;
using System.Collections.Generic;
using System.Linq;
using yavc.Base.Data;
using yavc.Base.Util;
using System.Collections.ObjectModel;

namespace yavc.Base.Models {
	public class VMStart : AVM {

		private static object TRY_ADD = new object();

		public VMStart() : this(new Device[0]) { }

		public VMStart(Device[] devices)
			: base(null) {
			_Devices = new List<VMDevice>(devices.Select(d => new VMDevice(this, d, true)));
		}

		#region Dependency Properties
		public List<VMDevice> _Devices { get; private set; }
		public VMDevice[] Devices { get { return _Devices.ToArray(); } }
		private string _AddingDeviceStatus;
        public string AddingDeviceStatus
        {
            get { return GetValue(() => AddingDeviceStatus); }
            set { SetValue(value, () => AddingDeviceStatus); }
        }
        public double AddingDevicePercentage
        {
            get { return GetValue(() => AddingDevicePercentage); }
            set { SetValue(value, () => AddingDevicePercentage); }
        }
        #endregion

		#region Public Methods
		public void CreateDeviceTile(Device d) {
			Factory.TileService.CreateOrUpdateTile(d);
		}

		public void Remove(VMDevice device) {
			if (device == null || string.IsNullOrEmpty(device.HostnameOrIp)) return;
			string hostname = device.HostnameOrIp.ToUpper();
			var foundDevice = _Devices.FirstOrDefault(d => d.HostnameOrIp.ToUpper() == device.HostnameOrIp);

			if (foundDevice != null) {
				_Devices.Remove(foundDevice);
				Factory.TileService.DeleteAllTilesForDevice(foundDevice.Device);
				SessionManager.SaveState(this, null);
			}
		}

		public void TryAdd(Device device) {
			TryAdd(device, true);
		}

		public void TryFind(Action onFinished) {
			var found_hostnames = new Dictionary<string, bool>(); 
			Factory.DeviceFinder.FindAllAsync(5, d =>
			{
				lock (TRY_ADD) {
					if (!found_hostnames.ContainsKey(d.HostnameOrIp)) {
						found_hostnames.Add(d.HostnameOrIp, true);
						TryAdd(d, false);
					}
				}
			}, onFinished);
		}

		public void Refresh() {
			foreach (var d in _Devices) {
				d.Refresh();
			}
		}
		#endregion

		private void TryAdd(Device device, bool updateIfFound) {
			if (device == null || device.HostnameOrIp == null || device.FriendlyName == null)
				return;

			var dvm = _Devices.FirstOrDefault(d => d.HostnameOrIp.ToUpper() == device.HostnameOrIp);

			UI.Invoke(() =>
			{
				if (null != dvm) {
					if (updateIfFound)
						dvm.RefreshDevice(device);
					else
						dvm.Refresh();
				} else {
					dvm = new VMDevice(this, device);
					_Devices.Add(dvm);
					Notify(() => Devices);
					dvm.Refresh();
				}
			});
		}
	}
}

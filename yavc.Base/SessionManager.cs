using System.Linq;
using yavc.Base;
using yavc.Base.Models;
using System.Xml.Serialization;
using System;

namespace yavc.Base.Data {
	public class SessionManager {

		public static Device AddedDevice { get; set; }

		#region Getters
		public static void GetVMMain(string hostnameOrIp, Action<VMMain> OnGetVMMain) {
			GetVMMain(new Device(hostnameOrIp), OnGetVMMain);
		}
		public static void GetVMMain(Device device, Action<VMMain> OnGetVMMain) {
			Read<Session>(device.HostnameOrIp, s => {
				//-- If we don't have any data, just return a new VM
				if (null == s || null == s.Device) {
					OnGetVMMain.NullableInvoke(new VMMain(new Controller(device)));
					return;
				}

				var vm = new VMMain(new Controller(s.Device, true));
				vm.SetSelectedZone(s.SelectedZone);

				OnGetVMMain.NullableInvoke(vm);
			});
		}

		private const string VMStartKey = "VMStart";
		public static void GetVMStart(Action<VMStart> OnGetStart) {
			Read<Device[]>(VMStartKey, devices => {
				if (devices == null)
					OnGetStart.NullableInvoke(new VMStart());
				else
					OnGetStart.NullableInvoke(new VMStart(devices));
			});
		}
		#endregion

		#region Setters
		public static void SaveState(VMMain model, Action OnSaveStateFinished) {
			if (
				null == model ||
				null == model.Zones ||
				null == model.TheController ||
				string.IsNullOrEmpty(model.TheController.HostNameorAddress)) return;

			WriteFile(new Session(model), model.TheController.HostNameorAddress, OnSaveStateFinished);
		}
		public static void SaveState(VMStart model, Action OnSaveFinished) {
			if (null == model) return;
			//-- Do not save any invalid devices (i.e. any that are invalid or are currently loading)
			var devices = model._Devices.Select(d => d.Device).Where(d => !d.InvalidDevice).ToArray();
			WriteFile(devices, VMStartKey, OnSaveFinished);
		}
		#endregion

		#region Helper Methods
		private static void WriteFile<T>(T obj, string filename, Action OnWriteFinished) {
			try {
				Factory.FileStore.WriteFile(obj, filename, OnWriteFinished);
			} catch { }
		}

		private static void Read<T>(string filename, Action<T> OnReadFinished) {
			try {
				Factory.FileStore.Read<T>(filename, readResult => {
					if (null == readResult) 
						OnReadFinished.NullableInvoke(default(T));
					else
						OnReadFinished.NullableInvoke(readResult);
				});
			} catch { }
		}
		#endregion
	}

	public class Session {

		public Session() { }
		public Session(VMMain model) {
			Device = model.TheController.Device;
			if (model.SelectedZone != null)
				SelectedZone = model.SelectedZone.TheZone.Name;
		}

		[XmlElement]
		public Device Device { get; set; }

		[XmlElement]
		public string SelectedZone { get; set; }
	}
}

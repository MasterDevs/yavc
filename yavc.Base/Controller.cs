using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using yavc.Base.Commands;
using System.Threading;
using yavc.Base.Data;
using yavc.Base.Parsers;

namespace yavc.Base {
	public class Controller : IController {

		#region Private Variables
		private IParseSystemInfo _sysInfoParser;
		private GetPartyMode _PartyMode;
		#endregion

		#region Public Properties
		public Device Device { get; set; }

		public bool IsConnected { get; protected set; }

		public bool IsPartyModeOn { get { return _PartyMode.IsOn; } }

		public string HostNameorAddress { get { return Device.HostnameOrIp; } }

		public string RecieverName { get { return Device.FriendlyName; } }
		public IProcessRequest RequestProccessor { get; private set; }

		public Zone[] Zones { get { return Device.Zones; } set { Device.Zones = value; } }

		public Source[] Sources { get { return Device.Sources; } }
		#endregion


		public Controller(Device d) : this(d, false) { }

		public Controller(Device d, bool isConnected) {
			Device = d;
			RequestProccessor = Factory.RequestProcessor;
			_sysInfoParser = new SystemInfoParser(RequestProccessor, new ZoneParser());
			_PartyMode = new GetPartyMode(null, RequestProccessor);
			IsConnected = isConnected;
		}

		public void UpdateStatus(Action<SendResult> OnComplete) {
			try {
				var commands = new Queue<ACommand>();
				foreach (var z in Zones) {
					commands.Enqueue(new RefreshZone(z, _sysInfoParser.ZoneParser));
				}
				//commands.Enqueue(_PartyMode);
				SendCommands(commands, OnComplete);
			} catch (Exception exp) {
				OnComplete(SendResult.Error(exp));
			}
		}

		public void RefreshZone(Zone z, Action<SendResult> OnComplete) {
			SendCommand(new RefreshZone(z, _sysInfoParser.ZoneParser), false, OnComplete);
		}

		public void SetPartyMode(bool partyModeOn, Action<SendResult> OnComplete) {
			var spm = new SetPartyMode(partyModeOn, null);
			SendCommand(spm, true, OnComplete);
		}

		public void SetPureDirect(Zone zone, bool pureDirectOn, Action<SendResult> OnComplete) {
			var spd = new SetPureDirect(zone, pureDirectOn);
			SendCommand(spd, true, OnComplete);
		}

		#region Volume
		public void VolumeUp(Zone z, Action<SendResult> OnComplete) {
			z.Volume.Raise();
			ChangeVolume(z, OnComplete);
		}

		public void VolumeDown(Zone z, Action<SendResult> OnComplete) {
			z.Volume.Lower();
			ChangeVolume(z, OnComplete);
		}

		public void ChangeVolume(Zone z, Action<SendResult> OnComplete) {
			if (z != null) {
				SendCommand(new UpdateVolume(z), false, OnComplete);
			}
		}
		#endregion

		#region Selects
		public void SelectDSP(Zone z) {
			SelectSurround sur = new SelectSurround(z, RequestProccessor);
			SendCommand(sur, false, null);
		}

		public void SelectInput(Zone zone, Input input, Action<SendResult> OnComplete) {
			SelectInput si = new SelectInput(zone, input);
			SendCommand(si, false, OnComplete);
		}

		public void SelectScene(Zone zone, Scene scene, Action<SendResult> OnComplete) {
			SelectScene si = new SelectScene(zone, scene);
			SendCommand(si, false, OnComplete);
		}
		#endregion

		#region Listing
		public void ListMenuUp(Input i, Action<SendResult> OnComplete) {
			SendCommand(new ListMenuUp(i), false, OnComplete);
		}
		public void SelectCurrentListItem(Input input, Action<SendResult> OnComplete) {
			var cmd = new SelectCurrentListItem(input);
			SendCommand(cmd, false, OnComplete);
		}
		#endregion

		public void ToggleMute(Zone zone, Action<SendResult> OnComplete) {
			ToggleMute cmd = new ToggleMute(zone);
			SendCommand(cmd, false, OnComplete);
		}

		public void TogglePower(Zone zone, Action<SendResult> OnComplete) {
			SendCommand(new TogglePower(zone), false, OnComplete);
		}

		public void TrySetDevice(Device device, Action<SendResult> onComplete) {
			var oldDevice = Device;
			Device = device;
			try {
				SystemInfo info = new SystemInfo(_sysInfoParser);
				SendCommand(info, false, sr =>
				{
					IsConnected = sr.Success;

					if (!IsConnected) {
						Device = oldDevice;
						onComplete.NullableInvoke(sr);
					} else {
						Device.Zones = info.Zones.Select(z => z.TheZone).ToArray();
						Device.Sources = info.Sources.Values.ToArray();
						SendCommands(new Queue<ACommand>(info.Zones), onComplete);
					}
				});
			} catch (Exception exp) {
				IsConnected = false;
				onComplete.NullableInvoke(SendResult.Error(exp));
			}
		}

		#region Helper Methods
		protected void SendCommands(Queue<ACommand> cmds, Action<SendResult> OnComplete) {
			if (cmds.Count == 0) {
				OnComplete.NullableInvoke(SendResult.Succcess);
				return;
			}

			var cmd = cmds.Dequeue();
			cmd.Send(this, sr =>
			{
				if (sr.Success) {
					SendCommands(cmds, OnComplete);
				} else {
					OnComplete.NullableInvoke(sr);
				}
			});
		}

		protected void SendCommand(ACommand cmd, bool update, Action<SendResult> OnComplete) {
			if (update) {
				cmd.Send(this, sr =>
				{
					if (sr.Success)
						UpdateStatus(OnComplete);
					else {
						OnComplete.NullableInvoke(sr);
					}
				});
			} else {
				cmd.Send(this, sr => OnComplete.NullableInvoke(sr));
			}
		}
		#endregion
	}
}

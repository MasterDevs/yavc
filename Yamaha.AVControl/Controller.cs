using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Xml.Linq;
using Yamaha.AVControl.Commands;
using Yamaha.AVControl.Commands.Data;
using System.Threading;

namespace Yamaha.AVControl {
	public class Controller {

		#region Private Variables
		private string _HostNameorAddress;
		private string RequestUri {
			get { return string.Format("http://{0}/YamahaRemoteControl/ctrl", _HostNameorAddress); }
		}
		private ZoneStatus[] _Zones;
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the IP Address of the connected reciever.
		/// </summary>
		public IPAddress Address { get; protected set; }

		public Zone[] Zones { get { return _Zones.Select(z => z.Zone).ToArray(); } }

		public bool IsConnected { get; protected set; }
		#endregion

		public event EventHandler StatusUpdated = delegate { };

		public Controller(string hostNameorAddress) {
			_HostNameorAddress = hostNameorAddress;
			_Zones = new ZoneStatus[] {
				new ZoneStatus("Main_Zone"),
				new ZoneStatus("Zone_2"),
			};
		}

		public bool TryConnect() {
			try {
				Ping p = new Ping();
				PingReply pr = p.Send(_HostNameorAddress);

				if (pr.Status != IPStatus.Success)
					return false;

				Address = pr.Address;

				InitializeController();

				UpdateStatus();

				IsConnected = true;
				return true;
			} catch {
				IsConnected = false;
				return false; }
		}

		private void InitializeController() {
			SystemInfo info = new SystemInfo();
			SendCommand(info, false);
			_Zones = info.Zones;
		}

		public void UpdateStatus() {
			foreach (var z in _Zones) {
				SendCommand(z, false);
			}

			StatusUpdated(this, EventArgs.Empty);
		}

		public void SendCommand(ACommand cmd, bool update) {
			cmd.Send(this);
			if (update)
				UpdateStatus();
		}

		public HttpWebRequest GenerateRequest() {
			return (HttpWebRequest)WebRequest.Create(RequestUri);
		}

		public HttpWebRequest GenerateRequest(string relativeUri) {
			return (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/{1}", _HostNameorAddress, relativeUri));
		}

		public void VolumeUp(Zone z) {
			z.Volume.Raise();
			ChangeVolume(z);
		}

		public void VolumeDown(Zone z) {
			z.Volume.Lower();
			ChangeVolume(z);
		}

		public void ChangeVolume(Zone z) {
			if (z != null) {
				SendCommand(new UpdateVolume(z), true);
			}
		}

		public void SelectInput(Zone zone, Input input) {
			SelectInput si = new SelectInput(zone, input);
			SendCommand(si, true);
		}

		public void SelectScene(Zone zone, Scene scene) {
			SelectScene si = new SelectScene(zone, scene);
			SendCommand(si, true);
		}

		public void ToggleMute(Zone zone) {
			ToggleMute cmd = new ToggleMute(zone);
			SendCommand(cmd, true);
		}

		public void TogglePower(Zone zone) {
			TogglePower cmd = new TogglePower(zone);
			SendCommand(cmd, true);
		}

		public bool TrySetAddress(string hostOrAddress) {
			string oldHost = _HostNameorAddress;
			_HostNameorAddress = hostOrAddress;
			if (!TryConnect())
				_HostNameorAddress = oldHost;
			return IsConnected;
		}
	}
}

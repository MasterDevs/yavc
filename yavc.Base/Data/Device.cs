using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace yavc.Base.Data {
	public class Device {

		[XmlElement]
		public string FriendlyName { get; set; }
		[XmlElement]
		public string HostnameOrIp { get; set; }
		public string ImageUri {
			get {
				if (string.IsNullOrEmpty(HostnameOrIp)) return null;

				return string.Format("http://{0}:49154/Icons/48x48.png", HostnameOrIp);
			}
		}
		[XmlElement]
		public bool IsPartyModeyOn { get; set; }
		[XmlElement]
		public bool InvalidDevice { get; set; }
		[XmlElement]
		public string LoadingMesssage { get; set; }
		[XmlElement]
		public Source[] Sources { get; set; }
		[XmlElement]
		public Zone[] Zones { get; set; }

		public Device() { LoadingMesssage = " "; }
		public Device(string hostnameOrIp) : this() {
			HostnameOrIp = FriendlyName = hostnameOrIp;
		}
		public Device(string hostnameOrIp, string friendlyName)
			: this(hostnameOrIp) {
			FriendlyName = friendlyName;
		}

		public static Device Empty { get { return new Device(string.Empty, string.Empty); } }
		/// <summary>
		/// Gets a value indicating the Query String Key used in the Navigation Uri
		/// when passing in a Device
		/// </summary>
		public static string PageUriKey { get { return "Device"; } }
	}
}

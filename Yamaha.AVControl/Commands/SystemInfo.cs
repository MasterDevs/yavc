using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Util;

namespace Yamaha.AVControl.Commands {
	public class SystemInfo : ACommand {

		public SystemInfo() : base(null) { }

		public string RecieverName { get; protected set; }
		public ZoneStatus[] Zones { get; protected set; }

		protected override SendResult ParseResponseImp(XElement xml) {

			try {
				List<ZoneStatus> zones = new List<ZoneStatus>();

				RecieverName = xml.Attribute("Unit_Name").Value;

				foreach (var menu in xml.Descendants("Menu")) {
					if (menu.CompareAttributeVal("Func", "Subunit"))
						zones.Add(new ZoneStatus(menu.Attribute("YNC_Tag").Value));
				}

				Zones = zones.ToArray();
			} catch (Exception exp) {
				return SendResult.Error(exp);
			}

			return SendResult.Succcess;
		}

		protected override RequestInfo[] GetRequests() {
			return new RequestInfo[] {
				RequestInfo.GenRequest("GET", string.Empty, "YamahaRemoteControl/desc.xml"),
			};
		}
	}

}


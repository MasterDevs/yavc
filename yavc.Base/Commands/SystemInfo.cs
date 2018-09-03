using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;

namespace yavc.Base.Commands {

	public interface IParseSystemInfo {
		void Load(string xml);
		string RecieverName { get; }
		IEnumerable<ZoneStatus> Zones { get; }
		Dictionary<string, Source> Sources { get; }
		IParseZone ZoneParser { get; }
	}

	public class SystemInfo : ACommand {

		private IParseSystemInfo Parser;

		public SystemInfo(IParseSystemInfo parser) {
			Parser = parser;
		}

		public string RecieverName { get; protected set; }
		public ZoneStatus[] Zones { get; protected set; }
		public Dictionary<string, Source> Sources { get; protected set; }

		protected override SendResult ParseResponseImp(string xml) {

			try {
				Parser.Load(xml);

				RecieverName = Parser.RecieverName;
				Zones = Parser.Zones.ToArray();
				Sources = Parser.Sources;

			} catch (Exception exp) {
				return SendResult.Error(exp);
			}

			return SendResult.Succcess;
		}

		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Get, null, "YamahaRemoteControl/desc.xml", 80),
			};
		}
	}

}


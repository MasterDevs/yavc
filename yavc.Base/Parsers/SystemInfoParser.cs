using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Commands;
using yavc.Base.Util;
using yavc.Base.Data;

namespace yavc.Base.Parsers {

	public class SystemInfoParser : IParseSystemInfo {

		private IProcessRequest _reqProcessor;

		public SystemInfoParser(IProcessRequest reqProccessor, IParseZone zoneParser) {
			_reqProcessor = reqProccessor;
			ZoneParser = zoneParser;
		}

		#region IParseSystemInfo Members

		public void Load(string xmlS) {
			try {
				var xml = XElement.Parse(xmlS);

				var zones = new List<ZoneStatus>();
				Sources = new Dictionary<string, Source>();

				RecieverName = xml.Attribute("Unit_Name").Value;

				foreach (var menu in xml.Descendants("Menu")) {
					if (menu.CompareAttributeVal("Func", "Subunit"))
						zones.Add(new ZoneStatus(menu.Attribute("YNC_Tag").Value, ZoneParser));
					else if (menu.CompareAttributeVal("Func", "Source_Device")) {
						var s = ParseSource(menu);
						Sources.Add(s.SourceName, s);
					}
				}
				Zones = zones;
			} catch { }
		}

		private Source ParseSource(XElement menu) {
			Source s = new Source();
			s.SourceName = menu.Attribute("YNC_Tag").Value;
			foreach (var m in menu.Elements("Menu")) {
				var func = m.Attribute("Func").Value;
				switch (func) {
					case "List_Browse":
						s.CanList = true;
						break;
					case "Play_Info":
						s.PlayInfo = true;
						break;
					case "Play_Control":
						s.Control = ParseControl(m);
						break;
					default:
						break;
				}
			}
			return s;
			//s.CanList = menu.Descendants().Where(m => m.CompareAttributeVal("Func", "List_Browse")).Count() == 1
		}

		private PlayControl ParseControl(XElement m) {
			var control = new PlayControl();

			foreach (var el in m.Elements()) {
				if (el.CompareAttributeVal("Func", "Playback")) {
					foreach (var child in el.Elements("Put_1")) {
						if (child.CompareAttributeVal("Func", "Play"))
							control.CanPlay = true;
						else if (child.CompareAttributeVal("Func", "Pause"))
							control.CanPause = true;
						else if (child.CompareAttributeVal("Func", "Stop"))
							control.CanStop = true;
					}
				} else if (el.CompareAttributeVal("Func", "Plus_Minus") && el.CompareAttributeVal("Title_1", "Skip")) {
					foreach (var child in el.Elements("Put_1")) {
						if (child.CompareAttributeVal("Func", "Plus_1"))
							control.CanNext = true;
						else if (child.CompareAttributeVal("Func", "Minus_1"))
							control.CanPrev = true;
					}
				}
			}

			return control;
		}

		public bool IsPartyModeOn { get; private set; }
		public string RecieverName { get; private set; }
		public IEnumerable<ZoneStatus> Zones { get; private set; }
		public Dictionary<string, Source> Sources { get; private set; }
		public IParseZone ZoneParser { get; private set; }

		#endregion
	}
}

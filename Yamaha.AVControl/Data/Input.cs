using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Util;

namespace Yamaha.AVControl.Commands.Data {
	public class Input {

		public static Input[] Parse(XElement responseXML) {
			List<Input> inputs = new List<Input>();
			var xml_items = responseXML.Descendants("Input_Sel_Item").FirstOrDefault();
			foreach (var item in xml_items.Elements()) {
				Input i = new Input();
				i.Name = item.Name.ToString();
				i.Param = item.GetStrFromEV("Param", string.Empty);
				i.Title = item.GetStrFromEV("Title", string.Empty);
				i.IconOff = item.GetStrFromEV("Off", string.Empty);
				i.IconOn = item.GetStrFromEV("On", string.Empty);
				i.Src_Name = item.GetStrFromEV("Src_Name", string.Empty);
				i.Src_Number = item.GetStrFromEV("Src_Number", string.Empty);
				inputs.Add(i);
			}
			return inputs.ToArray();
		}

		public string Name { get; set; }
		public string Param { get; set; }
		public string Title { get; set; }
		public string IconOn { get; set; }
		public string IconOff { get; set; }
		public string Src_Number { get; set; }
		public string Src_Name { get; set; }

		public override string ToString() {
			return Title;
		}
	}
}

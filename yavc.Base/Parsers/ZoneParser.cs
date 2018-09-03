using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Commands;
using yavc.Base.Data;
using yavc.Base.Util;

namespace yavc.Base.Parsers {
	public class ZoneParser : IParseZone {

		#region IParseZone Members

		public SendResult ParseResponse(string xml, Zone z) {
			var responseXML = XElement.Parse(xml);
			
			if (null != responseXML.Descendants("Basic_Status").FirstOrDefault()) {
				z.Volume = ParseVolume(responseXML.Descendants("Volume").FirstOrDefault());
				z.PowerOn = responseXML.CompareElementVal("Power", "On");
				z.Sleeping = responseXML.CompareElementVal("Sleep", "On");
				z.SelectedInput = responseXML.GetStrFromEV("Input_Sel", string.Empty);
				z.Surround = ParseSurround(responseXML.Descendants("Surround").FirstOrDefault());
				return SendResult.Succcess;
			} else if (null != responseXML.Descendants("Config").FirstOrDefault()) {
				z.HasVolume = responseXML.CompareElementVal("Volume_Existence", "Exist");
				z.DisplayName = responseXML.GetStrFromEV("Zone", z.Name);
				return SendResult.Succcess;
			} else if (null != responseXML.Descendants("Input_Sel_Item").FirstOrDefault()) {
				z.Inputs = ParseInputs(responseXML);
				return SendResult.Succcess;
			} else if (null != responseXML.Descendants("Scene_Sel_Item").FirstOrDefault()) {
				z.Scenes = ParseScenes(responseXML);
				return SendResult.Succcess;
			} else if (null != responseXML.Descendants("Pure_Direct").FirstOrDefault()) {
				z.PureDirectOn = ParsePureDirect(responseXML);
				return SendResult.Succcess;
			}
			return SendResult.Error(new Exception("Unknown response returned."));
		}

		#endregion

		#region Zone Parsing
		private Input[] ParseInputs(XElement xml) {
			List<Input> inputs = new List<Input>();
			var xml_items = xml.Descendants("Input_Sel_Item").FirstOrDefault();
			foreach (var item in xml_items.Elements()) {
				Input i = new Input();
				i.Name = item.Name.ToString();
				i.Param = item.GetStrFromEV("Param", string.Empty);
				i.Title = item.GetStrFromEV("Title", string.Empty);
				//-- Edge Case - Docks
				//For some reason the default Title for Bluetooth, iPod and USB are all Dock.
				//This makes it confusing for users. So to make it easy for 99.9% of users
				//if 'DOCK' is the title, we'll use Param as the title
				if((i.Title.IsNullOrEmpty() || i.Title == "DOCK") && !i.Param.IsNullOrEmpty())
					i.Title = i.Param;
				i.IconOff = item.GetStrFromEV("Off", string.Empty);
				i.IconOn = item.GetStrFromEV("On", string.Empty);
				i.Src_Name = item.GetStrFromEV("Src_Name", string.Empty);
				i.Src_Number = item.GetStrFromEV("Src_Number", string.Empty);
				inputs.Add(i);
			}
			return inputs.ToArray();

		}

		private bool ParsePureDirect(XElement responseXML) {
			var pureDirect = responseXML.Descendants("Pure_Direct").FirstOrDefault();
			if (null != pureDirect) {
				return pureDirect.CompareElementVal("Mode", "On");
			}
			return false;
		}

		private Scene[] ParseScenes(XElement xml) {
			List<Scene> scenes = new List<Scene>();

			foreach (var s in xml.Descendants("Scene_Sel_Item").Elements()) {
				//-- Scenes are defined as any Writeable Input
				if (s.CompareElementVal("RW", "W")) {
					string name = s.GetStrFromEV("Param", null);
					string display = s.GetStrFromEV("Title", null);
					string icon = s.GetStrFromEV("On", null);

					if (null != display && null != icon && null != name)
						scenes.Add(new Scene(name, display, icon));
				}
			}
			return scenes.ToArray();
		}

		private Surround ParseSurround(XElement xml) {
			if (xml == null) return null;
			return new Surround(
				xml.CompareElementVal("Straight", "On"),
				xml.CompareElementVal("Enhancer", "On"),
				xml.GetStrFromEV("Sound_Program", string.Empty));
		}

		private Volume ParseVolume(XElement data) {
			return new Volume(
				(double)data.GetIntFromEV("Val", 0) / 10D,
				data.GetStrFromEV("Exp", string.Empty),
				data.GetStrFromEV("Unit", string.Empty),
				data.GetStrFromEV("Mute", "Off") != "Off");
		}
		#endregion
	}
}

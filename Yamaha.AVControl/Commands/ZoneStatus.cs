using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Util;
using Yamaha.AVControl.Commands.Data;

namespace Yamaha.AVControl.Commands {
	public class ZoneStatus : ACommand {

		public ZoneStatus(string name)
			: base(new Zone(name)) {
		}

		#region ACommand
		protected override RequestInfo[] GetRequests() {
			return new RequestInfo[] { 
				RequestInfo.GenRequest("POST", string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Basic_Status>GetParam</Basic_Status></{0}></YAMAHA_AV>", Zone.Name)),
				RequestInfo.GenRequest("POST", string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Config>GetParam</Config></{0}></YAMAHA_AV>", Zone.Name)),
				RequestInfo.GenRequest("POST", string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Input><Input_Sel_Item>GetParam</Input_Sel_Item></Input></{0}></YAMAHA_AV>", Zone.Name)),
			};
		}

		protected override SendResult ParseResponseImp(XElement responseXML) {
			LastResponse = responseXML;

			if (null != responseXML.Descendants("Basic_Status").FirstOrDefault()) {

				Zone.Volume = new Volume(responseXML.Descendants("Volume").FirstOrDefault());
				Zone.PowerOn = responseXML.CompareElementVal("Power", "On");
				Zone.Sleeping = responseXML.CompareElementVal("Sleep", "On");
				Zone.SelectedInput = responseXML.GetStrFromEV("Input_Sel", string.Empty);
				Zone.Surround = new Surround(responseXML.Descendants("Surround").FirstOrDefault());
			} else if (null != responseXML.Descendants("Config").FirstOrDefault()) {
				Zone.HasVolume = responseXML.CompareElementVal("Volume_Existence", "Exist");
				Zone.DisplayName = responseXML.GetStrFromEV("Zone", Zone.Name);

				Zone.Scenes = Scene.Parse(responseXML);

			} else if (null != responseXML.Descendants("Input_Sel_Item").FirstOrDefault()) {
				Zone.Inputs = Input.Parse(responseXML);
			}
			return SendResult.Succcess;
		}
		#endregion
	}
}

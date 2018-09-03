using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;

namespace yavc.Base.Commands {
	public class SelectInput : AZoneCommand {
		
		public Input Input { get; set; }

		public SelectInput(Zone zone, Input input)
			: base(zone) {
			Input = input;
		}


		protected override SendResult ParseResponseImp(string xml) {
			if(xml.Contains("Input"))
				return SendResult.Succcess;
			else
				return SendResult.Empty;
		}

		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(@"<YAMAHA_AV cmd=""PUT""><{0}><Input><Input_Sel>{1}</Input_Sel></Input></{0}></YAMAHA_AV>", TheZone.Name, Input.Param)),
			};
		}
	}
}

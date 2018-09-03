using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Util;
using Yamaha.AVControl.Commands.Data;

namespace Yamaha.AVControl.Commands {
	public class SelectInput : ACommand {
		
		public Input Input { get; set; }
		
		public SelectInput(Zone zone, Input input) : base(zone) {
			Input = input;
		}


		protected override SendResult ParseResponseImp(XElement xml) {
			if (null != xml.Descendants("Input").FirstOrDefault())
				return SendResult.Succcess;
			else
				return SendResult.Empty;
		}

		protected override RequestInfo[] GetRequests() {
			return new RequestInfo[] {
				RequestInfo.GenRequest("POST", string.Format(@"<YAMAHA_AV cmd=""PUT""><{0}><Input><Input_Sel>{1}</Input_Sel></Input></{0}></YAMAHA_AV>", Zone.Name, Input.Param)),
			};
		}
	}
}

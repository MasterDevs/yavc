using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;

namespace yavc.Base.Commands {
	public class TogglePower : AZoneCommand {

		public TogglePower(Zone z) : base(z) { }

		protected override SendResult ParseResponseImp(string xml) {
			if(xml.Contains("Power_Control")) {
				TheZone.PowerOn = !TheZone.PowerOn;
				return SendResult.Succcess;
			} else
				return SendResult.Empty;
		}

		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] {
		      RequestInfo.GenRequest(yavcMethod.Post, RequestString),
		    };
		}

		private string RequestString {
			get {
				return string.Format(@"<YAMAHA_AV cmd=""PUT""><{0}><Power_Control><Power>{1}</Power></Power_Control></{0}></YAMAHA_AV>",
					TheZone.Name,
					TheZone.PowerOn ? "Standby" : "On");
			}
		}
	}
}

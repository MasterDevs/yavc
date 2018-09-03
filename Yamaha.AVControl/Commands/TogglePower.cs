using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yamaha.AVControl.Commands.Data;

namespace Yamaha.AVControl.Commands {
	public class TogglePower : ACommand {

		public TogglePower(Zone z) : base(z) { }

		protected override SendResult ParseResponseImp(System.Xml.Linq.XElement xml) {
			if (null != xml.Descendants("Power_Control").FirstOrDefault()) {
				return SendResult.Succcess;
			} else
				return SendResult.Empty;
		}

		protected override RequestInfo[] GetRequests() {
			return new RequestInfo[] {
		      RequestInfo.GenRequest("POST", RequestString),
		    };
		}

		private string RequestString {
			get {
				return string.Format(@"<YAMAHA_AV cmd=""PUT""><{0}><Power_Control><Power>{1}</Power></Power_Control></{0}></YAMAHA_AV>",
					Zone.Name,
					Zone.PowerOn ? "Standby" : "On");
			}
		}
	}
}

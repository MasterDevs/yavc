using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Commands.Data;

namespace Yamaha.AVControl.Commands {
	public class ToggleMute : ACommand {

		public ToggleMute(Zone zone)
			: base(zone) {
		}

		protected override SendResult ParseResponseImp(System.Xml.Linq.XElement xml) {
			if (null != xml.Descendants("Mute").FirstOrDefault()) {
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
				return string.Format(@"<YAMAHA_AV cmd=""PUT""><{0}><Volume><Mute>{1}</Mute></Volume></{0}></YAMAHA_AV>",
						Zone.Name,
						Zone.Volume.MuteOn ? "Off" : "On");
			}
		}
	}
}

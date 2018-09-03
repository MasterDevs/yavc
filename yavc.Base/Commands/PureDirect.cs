using System;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Data;
using yavc.Base.Util;

namespace yavc.Base.Commands {
	public class SetPureDirect : AZoneCommand {

		private bool IsOn;

		private const string REQ = @"<YAMAHA_AV cmd=""PUT""><{0}><Sound_Video><Pure_Direct><Mode>{1}</Mode></Pure_Direct></Sound_Video></{0}></YAMAHA_AV>";
		private const string RESPONSE_SUCCESS = @"<YAMAHA_AV rsp=""PUT"" RC=""0""><{0}><Sound_Video><Pure_Direct><Mode></Mode></Pure_Direct></Sound_Video></{0}></YAMAHA_AV>";
		
		public SetPureDirect(Zone z, bool pureDirectOn)
			: base(z) {
				IsOn = pureDirectOn;
		}

		protected override SendResult ParseResponseImp(string xml) {
			if (string.Format(RESPONSE_SUCCESS, this.TheZone.Name).Equals(xml, StringComparison.CurrentCultureIgnoreCase))
				return SendResult.Succcess;
			else
				return SendResult.Error(new Exception("Invalid Response returned: " + xml));
		}

		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(REQ, TheZone.Name, IsOn ? "On" : "Off"))
			};
		}
	}
}

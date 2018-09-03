using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;

namespace yavc.Base.Commands {
	public class SelectSurround : AZoneCommand {
		private const string SuccessXML = @"<YAMAHA_AV rsp=""PUT"" RC=""0""><Main_Zone><Surround><Program_Sel><Current></Current></Program_Sel></Surround></Main_Zone></YAMAHA_AV>";
		private const string SET_STRAIT = @"<YAMAHA_AV cmd=""PUT""><{0}><Surround><Program_Sel><Current><Straight>{1}</Straight></Current></Program_Sel></Surround></{0}></YAMAHA_AV>";
		private const string SET_DSP = @"<YAMAHA_AV cmd=""PUT""><{0}><Surround><Program_Sel><Current><Sound_Program>{1}</Sound_Program></Current></Program_Sel></Surround></{0}></YAMAHA_AV>";

		public SelectSurround(Zone z, IProcessRequest processor) : base(z) { }

		protected override SendResult ParseResponseImp(string xml) {
			if (SuccessXML == xml)
				return SendResult.Succcess;

			return SendResult.Error(null,
				string.Format("Respone Does not match success fragment.\n\n\nResponse:{0}\nSuccessXML:{1}",
					xml, SuccessXML));
		}

		protected override RequestInfo[] GetRequestInfo() {
			if (TheZone.Surround.StraitOn)
				return GenStraitOn();
			else
				return GenDSDP();
		}

		/// <summary>
		/// In order to set DSP, we need to ensure strait is off.
		/// </summary>
		/// <returns></returns>
		private RequestInfo[] GenDSDP() {
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post,
				string.Format(SET_STRAIT, TheZone.Name, "Off")),
				RequestInfo.GenRequest(yavcMethod.Post, 
					string.Format(SET_DSP, TheZone.Name, TheZone.Surround.DSP)),
			};
		}

		private RequestInfo[] GenStraitOn() {
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post,
				string.Format(SET_STRAIT, TheZone.Name, "On")),
			};
		}
	}
}

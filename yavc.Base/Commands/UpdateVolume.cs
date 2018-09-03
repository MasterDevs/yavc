using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using yavc.Base.Data;

namespace yavc.Base.Commands {
	public class UpdateVolume : AZoneCommand {

		private const string REQUEST_FS = @"<YAMAHA_AV cmd=""PUT""><{0}><Volume><Lvl><Val>{1}</Val><Exp>{2}</Exp><Unit>{3}</Unit></Lvl></Volume></{0}></YAMAHA_AV>";

		public UpdateVolume(Zone z) : base(z) { }

		protected override SendResult ParseResponseImp(string xml) {
			return SendResult.Succcess;
		}

		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] { 
				RequestInfo.GenRequest(yavcMethod.Post,  
				string.Format(REQUEST_FS, TheZone.Name, TheZone.Volume.RawValue, TheZone.Volume.Exp, TheZone.Volume.Unit) )
			};
		}
	}
}

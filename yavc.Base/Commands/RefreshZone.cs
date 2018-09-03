using System.Linq;
using System.Xml.Linq;
using yavc.Base.Data;
using yavc.Base.Util;
using System;

namespace yavc.Base.Commands {

	public class RefreshZone : AZoneCommand {

		private IParseZone ZoneParser;

		public RefreshZone(string name, IParseZone zp)
			: this(new Zone(name), zp) { }

		public RefreshZone(Zone z, IParseZone zp) : base(z) {
			ZoneParser = zp;
		}

		#region ACommand
		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] { 
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Basic_Status>GetParam</Basic_Status></{0}></YAMAHA_AV>", TheZone.Name)),
			};
		}

		protected override SendResult ParseResponseImp(string xml) {
			ZoneParser.ParseResponse(xml, TheZone);
			return SendResult.Succcess;
		}
		#endregion
	}
}

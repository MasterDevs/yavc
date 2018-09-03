using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;

namespace yavc.Base.Commands {

	public interface IParseZone {
		SendResult ParseResponse(string xml, Zone z);
	}

	public class ZoneStatus : AZoneCommand {

		private IParseZone Parser;

		public ZoneStatus(string name, IParseZone parser)
			: this(new Zone(name), parser) {
				
		}

		public ZoneStatus(Zone z, IParseZone parser)
			: base(z) {
				Parser = parser;
		}

		#region ACommand
		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] { 
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Basic_Status>GetParam</Basic_Status></{0}></YAMAHA_AV>", TheZone.Name)),
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Config>GetParam</Config></{0}></YAMAHA_AV>", TheZone.Name)),
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Scene><Scene_Sel_Item>GetParam</Scene_Sel_Item></Scene></{0}></YAMAHA_AV>", TheZone.Name)),
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Input><Input_Sel_Item>GetParam</Input_Sel_Item></Input></{0}></YAMAHA_AV>", TheZone.Name)),
				//RequestInfo.GenRequest(yavcMethod.Post, string.Format(@"<YAMAHA_AV cmd=""GET""><{0}><Sound_Video><Pure_Direct><Mode>GetParam</Mode></Pure_Direct></Sound_Video></{0}></YAMAHA_AV>", Zone.Name)),
			};
		}

		protected override SendResult ParseResponseImp(string responseXML) {
			return Parser.ParseResponse(responseXML, TheZone);
		}
		#endregion
	}
}

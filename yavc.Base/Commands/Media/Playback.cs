using System;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Util;
using yavc.Base.Data;
using System.Collections.Generic;

namespace yavc.Base.Commands {
	public class Playback : ACommand {

		private const string REQ = @"<YAMAHA_AV cmd=""PUT""><{0}><Play_Control><Playback>{1}</Playback></Play_Control></{0}></YAMAHA_AV>";

		public Input Input { get; private set; }
		public PlaybackInfo Mode { get; private set; }

		public Playback(Input i, PlaybackInfo mode) { 
			Input = i;
			Mode = mode;
		}

		protected override SendResult ParseResponseImp(string xml) {
			try {
				var xel = XElement.Parse(xml);

				var Play_Control = xel.Descendants("Play_Control").FirstOrDefault();

				if (null == Play_Control) return SendResult.Error(new Exception("Invalid response returned"));

				return SendResult.Succcess;
			} catch (Exception exp) {
				return SendResult.Error(exp);
			}
		}

		protected override RequestInfo[] GetRequestInfo() {
			if (null == Input || Mode == PlaybackInfo.Unknown) return new RequestInfo[0];

			
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(REQ, Input.Src_Name, Mode))
			};
		}
	}
}

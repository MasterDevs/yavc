using System;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Util;
using yavc.Base.Data;
using System.Collections.Generic;

namespace yavc.Base.Commands {
	public class Skip : ACommand {

		private const string REQ = @"<YAMAHA_AV cmd=""PUT""><{0}><Play_Control><Playback>{1}</Playback></Play_Control></{0}></YAMAHA_AV>";

		public Input Input { get; private set; }
		public SkipDirection Direction { get; private set; }

		public Skip(Input i, SkipDirection direction) {
			Direction = direction;
			Input = i; 
		}

		protected override SendResult ParseResponseImp(string xml) {
			try {
				var xel = XElement.Parse(xml);

				var r = xel.Descendants("Play_Control").FirstOrDefault();

				if (null == r) return SendResult.Error(new Exception("Invalid response returned"));
				
				return SendResult.Succcess;
			} catch (Exception exp) {
				return SendResult.Error(exp);
			}
		}

		protected override RequestInfo[] GetRequestInfo() {
			if (null == Input) return new RequestInfo[0];
			
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(REQ, Input.Src_Name, GetDirectionString(Direction)))
			};
		}

		private static string GetDirectionString(SkipDirection direction) {
			switch (direction) {
				case SkipDirection.Forward:
					return @"Skip Fwd";
				case SkipDirection.Backward:
					return @"Skip Rev";
				default:
					return string.Empty;
			}
		}
	}

	public enum SkipDirection {
		Forward,
		Backward
	}
}

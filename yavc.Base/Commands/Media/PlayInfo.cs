using System;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Util;
using yavc.Base.Data;
using System.Collections.Generic;

namespace yavc.Base.Commands {
	public class PlayInfo : ACommand {

		private const string REQ = @"<YAMAHA_AV cmd=""GET""><{0}><Play_Info>GetParam</Play_Info></{0}></YAMAHA_AV>";

		public Input Input { get; private set; }

		public PlaybackInfo Playback { get; private set; }
		public MetaInfo[] MetaInfo { get; private set; }
		
		public PlayInfo(Input i) { Input = i; }

		protected override SendResult ParseResponseImp(string xml) {
			try {
				var xel = XElement.Parse(xml);

				var playinfo = xel.Descendants("Play_Info").FirstOrDefault();

				if (null == playinfo) return SendResult.Error(new Exception("Invalid response returned"));

				Playback = ParsePlayBack(playinfo.Element("Playback_Info"));
				MetaInfo = ParseMeta(playinfo.Element("Meta_Info"));				

				return SendResult.Succcess;
			} catch (Exception exp) {
				return SendResult.Error(exp);
			}
		}

		protected override RequestInfo[] GetRequestInfo() {
			if (null == Input) return new RequestInfo[0];
			
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(REQ, Input.Src_Name))
			};
		}

		private static MetaInfo[] ParseMeta(XElement xml) {
			var infos = new List<MetaInfo>();
			foreach (var el in xml.Elements()) {
				infos.Add(new MetaInfo(el.Name.ToString().HtmlDecode(), el.Value.HtmlDecode()));
			}
			return infos.ToArray();
		}

		private static PlaybackInfo ParsePlayBack(XElement xml) {
			switch (xml.Value.ToUpper()) {
				case "PLAY":
					return PlaybackInfo.Play;
				case "PAUSE":
					return PlaybackInfo.Pause;
				case "STOP":
					return PlaybackInfo.Stop;
				default:
					return PlaybackInfo.Unknown;
			}
		}
	}

	public enum PlaybackInfo {
		Unknown = 0,
		Play,
		Pause,
		Stop
	}

	public class MetaInfo {
		public string Key { get; set; }
		public string Value { get; set; }
		public MetaInfo() { }
		public MetaInfo(string key, string value) {
			Key = key;
			Value = value;
		}
	}
}

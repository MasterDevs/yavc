using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Commands.Data;

namespace Yamaha.AVControl.Commands {
	public class SelectScene : ACommand {

		public Scene Scene { get; protected set; }
		public SelectScene(Zone zone, Scene scene) : base(zone) {
			Scene = scene;
		}
		
		protected override SendResult ParseResponseImp(XElement xml) {
			return SendResult.Empty;
		}

		protected override RequestInfo[] GetRequests() {
			return new RequestInfo[] {
				RequestInfo.GenRequest("POST", string.Format(@"<YAMAHA_AV cmd=""PUT""><{0}><Scene><Scene_Sel>{1}</Scene_Sel></Scene></{0}></YAMAHA_AV>", Zone.Name, Scene.Name)),
			};
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Commands.Data;
using System.Net;

namespace Yamaha.AVControl.Commands {
	public class UpdateVolume : ACommand {

		private const string REQUEST_FS = @"<?xml version=""1.0"" encoding=""utf-8""?><YAMAHA_AV cmd=""PUT""><{0}><Volume><Lvl><Val>{1}</Val><Exp>{2}</Exp><Unit>{3}</Unit></Lvl></Volume></{0}></YAMAHA_AV>";
		
		public UpdateVolume(Zone z) : base(z) { }

		protected override SendResult ParseResponseImp(XElement xml) {
			return new SendResult();
		}

		protected override RequestInfo[] GetRequests() {
			return new RequestInfo[] { 
				RequestInfo.GenRequest("POST",  
				string.Format(REQUEST_FS, Zone.Name, Zone.Volume.RawValue, Zone.Volume.Exp, Zone.Volume.Unit) )
			};
		}
	}
}

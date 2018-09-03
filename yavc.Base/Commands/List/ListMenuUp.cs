using System;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Util;
using System.Collections.Generic;
using yavc.Base.Data;
using System.Net;
using System.ComponentModel;

namespace yavc.Base.Commands {
	public class ListMenuUp : ACommand {

		private string InputName;
		public ListMenuUp(Input i) {
			InputName = i.Src_Name;
		}

		protected override SendResult ParseResponseImp(string xml) {
			try {
				var root = XElement.Parse(xml);
				var list_control = root.Descendants("List_Control").FirstOrDefault();
				
				if (null != list_control)
					return SendResult.Succcess;

				return SendResult.Empty;
			} catch (Exception exp) {
				return SendResult.Error(exp);
			}
		}

		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, ListRequests.GetBackRequest(InputName))
			};
		}
	}
}

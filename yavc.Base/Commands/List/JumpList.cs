using System;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Data;
using yavc.Base.Util;
using System.Collections.Generic;

namespace yavc.Base.Commands {
	public class JumpList : GetList {
		
        public int JumpLine { get; private set; }
		
        public JumpList(int line, Input i) : base(i) {
				JumpLine = line;
		}

		protected override SendResult ParseResponseImp(string xml) {
			try {
				var root = XElement.Parse(xml);
				var List_Control = root.Descendants("List_Control").FirstOrDefault();
				
				if (null != List_Control) return SendResult.Succcess;

                return TryParseList(root);
			} catch (Exception exp) {
				return SendResult.Error(exp);
			}
		}

		protected override RequestInfo[] GetRequestInfo() {
            Items = new Dictionary<int, ListItem>();
            return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, ListRequests.GetJumpRequest(InputName, JumpLine)),
				RequestInfo.GenRequest(yavcMethod.Post, ListRequests.GetListRequest(InputName)),
			};
		}
	}
}

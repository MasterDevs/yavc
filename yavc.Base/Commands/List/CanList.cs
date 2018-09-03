using System;
using System.Linq;
using System.Xml.Linq;
using yavc.Base.Util;

namespace yavc.Base.Commands {
	public class CanList : ACommand {

		public bool FeatureExists { get; private set; }
		public bool FeatureAvailable { get; private set; }
		public string InputName { get; private set; }

		public CanList(string inputName) {
			InputName = inputName;
		}

		protected override SendResult ParseResponseImp(string xml) {
			try {
				var xel = XElement.Parse(xml);

				var config = xel.Descendants("Config").FirstOrDefault();

				if (null != config) {
					FeatureExists = config.CompareElementVal("Feature_Existence", "1");
					FeatureAvailable = config.CompareElementVal("Feature_Availability", "Ready");
				}

				return SendResult.Succcess;
			} catch (Exception exp) {
				return SendResult.Error(exp);
			}
		}

		protected override RequestInfo[] GetRequestInfo() {
			return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, ListRequests.GetCanRequest(InputName))
			};
		}
	}
}

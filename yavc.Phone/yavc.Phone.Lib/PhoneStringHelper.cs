using System.Net;
using yavc.Base;

namespace yavc.Phone.Lib {
	public class PhoneStringHelper : IStringHelper {

		#region IStringHelper Members

		public string HtmlDecode(string value) {
			return HttpUtility.HtmlDecode(value);
		}

		#endregion
	}
}

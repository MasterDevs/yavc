using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using yavc.Base;

namespace yavc.Metro.Imp {
	public class MetroStringHelper : IStringHelper {
		#region IStringHelper Members

		public string HtmlDecode(string value) {
			return WebUtility.HtmlDecode(value);
		}

		#endregion
	}
}

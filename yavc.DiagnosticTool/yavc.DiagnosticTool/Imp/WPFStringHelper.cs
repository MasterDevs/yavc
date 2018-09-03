using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using yavc.Base;

namespace yavc.WPF.Imp {
	public class WPFStringHelper : IStringHelper {
		#region IStringHelper Members

		public string HtmlDecode(string value) {
            return HttpUtility.HtmlDecode(value);
		}

		#endregion
	}
}

using System.Collections.Generic;

namespace System {
	public static class UriExtensions {
		public static Dictionary<string, string> QueryStrings(this Uri uri) {
			var qs = new Dictionary<string, string>();
			
			if (null == uri) return qs;

			var uri_string = uri.ToString();
			int idx = uri_string.IndexOf('?');
			if (idx > 0) {
				foreach (var key_n_value in uri_string.Substring(++idx).Split('&')) {
					var kv = key_n_value.Split('=');
					qs.Add(kv[0], kv[1]);
				}
			}

			return qs;
		}
	}
}

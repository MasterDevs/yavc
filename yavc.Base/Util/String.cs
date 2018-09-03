namespace yavc.Base.Util {
	public static class StringExtensions {
		public static string HtmlDecode(this string html) {
			if (string.IsNullOrEmpty(html)) return html;

			return Factory.StringHelper.HtmlDecode(html);
		}
	}
}
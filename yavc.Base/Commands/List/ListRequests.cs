namespace yavc.Base.Commands {
	public class ListRequests {
		private const string BACK_REQ = @"<YAMAHA_AV cmd=""PUT""><{0}><List_Control><Cursor>Back</Cursor></List_Control></{0}></YAMAHA_AV>";
		private const string CAN_REQ = @"<YAMAHA_AV cmd=""GET""><{0}><Config>GetParam</Config></{0}></YAMAHA_AV>";
		private const string JUMP_REQ = @"<YAMAHA_AV cmd=""PUT""><{0}><List_Control><Jump_Line>{1}</Jump_Line></List_Control></{0}></YAMAHA_AV>";
		private const string LIST_REQ = @"<YAMAHA_AV cmd=""GET""><{0}><List_Info>GetParam</List_Info></{0}></YAMAHA_AV>";
		private const string SELECT_REQ = @"<YAMAHA_AV cmd=""PUT""><{0}><List_Control><Cursor>Sel</Cursor></List_Control></{0}></YAMAHA_AV>";

		public static string GetBackRequest(string input) {
			return string.Format(BACK_REQ, input);
		}
		public static string GetCanRequest(string input) {
			return string.Format(CAN_REQ, input);
		}
		public static string GetJumpRequest(string input, int line) {
			return string.Format(JUMP_REQ, input, line);
		}
		public static string GetListRequest(string input) {
			return string.Format(LIST_REQ, input);
		}
		public static string GetSelectRequest(string input) {
			return string.Format(SELECT_REQ, input);
		}
	}
}

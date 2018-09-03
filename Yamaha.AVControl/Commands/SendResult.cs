using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yamaha.AVControl.Commands {
	public class SendResult {
		
		public bool Success { get; private set; }
		public Exception Exception { get; private set; }
		public string Message { get; private set; }

		public static SendResult Succcess { get { return new SendResult() { Success = true }; }}

		public static SendResult Empty { get { return new SendResult(); } }

		public static SendResult Error(Exception exp) {
			return Error(exp, exp.Message);
		}

		public static SendResult Error(Exception exp, string msg) {
			SendResult sr = new SendResult();
			sr.Success = false;
			sr.Exception = exp;
			sr.Message = msg;
			return sr;
		}
	}
}

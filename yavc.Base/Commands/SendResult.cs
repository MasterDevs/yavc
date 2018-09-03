using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yavc.Base.Commands {

	public delegate void SendResultHandler(object sender, SendResult sr);
	public delegate void SendResultHandler<T>(object sender, SendResult<T> sr);

	public class SendResult {

		public bool Success { get; protected set; }
		public Exception Exception { get; protected set; }
		public string Message { get; protected set; }

		public static SendResult Succcess { get { return new SendResult() { Success = true }; } }

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

	public class SendResult<T> : SendResult {

		public T Result { get; set; }

		public SendResult(SendResult sr) {
			Exception = sr.Exception;
			Success = sr.Success;
			Message = sr.Message;
		}

		public static SendResult<T> GetSuccess(T result) {
			var sr = new SendResult<T>(SendResult.Succcess);
			sr.Result = result;
			return sr;
		}

		public static new SendResult<T> Error(Exception exp) {
			return new SendResult<T>(SendResult.Error(exp));
		}
	}
}

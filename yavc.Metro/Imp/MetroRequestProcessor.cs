using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using yavc.Base.Commands;
using yavc.Phone.Lib;

namespace yavc.Metro.Imp {
	public class MetroRequestProcessor : IProcessRequest {
		private static Encoding CommandEncoding { get { return Encoding.UTF8; } }

		private void Process(RequestState state) {
			Process(state.Infos, state.HostName, state.OnResponse, state.OnCompleted);
		}

		#region IProcessRequest Members
		public void Process(Queue<RequestInfo> infos, string hostname, Action<string> onResult, Action<SendResult> onCompleted) {
			if (infos.Count == 0) {
				onCompleted(SendResult.Succcess);
				return;
			}
			ProcessImp(infos, hostname, onResult, onCompleted);
		}
		#endregion

		protected virtual void ProcessImp(Queue<RequestInfo> infos, string hostname, Action<string> onResult, Action<SendResult> onCompleted) {
			var info = infos.Dequeue();

			var uri = string.Format("http://{0}:{1}/{2}", hostname, info.Port, info.RelativeUri);
			var req = (HttpWebRequest)HttpWebRequest.CreateHttp(uri);
			//req.AllowReadStreamBuffering = true;

			//foreach (var header in info.Headers) {
			//	req.Headers[header.Key] = header.Value;
			//	TODO Check User Agent -> Might not need special workaround here.
			//	if (header.Key == "User-Agent") //-- Special header
			//		req.UserAgent = header.Value;
			//}

			req.Method = info.Method == yavcMethod.Get ? "GET" : "POST";

			try {
				if (info.Method == yavcMethod.Post) {
					byte[] body = CommandEncoding.GetBytes(info.Body);
					req.BeginGetRequestStream(RequestCallback, new RequestState(hostname, body, req, infos, onResult, onCompleted));
					return;
				} else {
					req.BeginGetResponse(ResponseCallback, new RequestState(hostname, req, infos, onResult, onCompleted));
				}
			} catch { }
		}

		protected void RequestCallback(IAsyncResult ar) {
			var state = (RequestState)ar.AsyncState;
			try {
				using (var stream = state.Request.EndGetRequestStream(ar)) {
					stream.Write(state.Body, 0, state.Body.Length);
				}
				state.Request.BeginGetResponse(ResponseCallback, state);
			} catch (Exception exp) {
				state.OnCompleted.NullableInvoke(SendResult.Error(exp));
			}
		}

		protected void ResponseCallback(IAsyncResult ar) {
			var state = (RequestState)ar.AsyncState;
			try {
				var response = state.Request.EndGetResponse(ar);
				using (var sr = new StreamReader(response.GetResponseStream())) {
					state.OnResponse(sr.ReadToEnd());
				}
				Process(state);
			} catch (Exception exp) {
				state.OnCompleted.NullableInvoke(SendResult.Error(exp));
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Net;
using yavc.Base.Commands;

namespace yavc.Phone.Lib {
	public class RequestState {
		public RequestState(string hostname, HttpWebRequest request, Queue<RequestInfo> infos, Action<string> responseAction, Action<SendResult> onCompleted) : this(hostname, null, request, infos, responseAction, onCompleted) { }
		public RequestState(string hostname, byte[] body, HttpWebRequest request, Queue<RequestInfo> infos, Action<string> responseAction, Action<SendResult> onCompleted) {
			Body = body;
			HostName = hostname;
			Infos = infos;
			OnCompleted = onCompleted;
			OnResponse = responseAction;
			Request = request;
		}
		public HttpWebRequest Request { get; set; }
		public byte[] Body { get; set; }
		public Action<string> OnResponse { get; set; }
		public Queue<RequestInfo> Infos { get; set; }
		public string HostName { get; set; }
		public Action<SendResult> OnCompleted { get; set; }
	}
}

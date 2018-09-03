using System;
using System.Collections.Generic;

namespace yavc.Base.Commands {

	public abstract class ACommand : ICommand {

		public SendResult LastSendResult { get; protected set; }
		
		protected virtual string UserAgent { get { return @"AVcontrol/1.03 CFNetwork/485.12.30 Darwin/10.4.0"; } }
		protected virtual IEnumerable<KeyValuePair<string, string>> AdditionalHeaders { get { return new List<KeyValuePair<string, string>>(); } }

		protected abstract SendResult ParseResponseImp(string xml);
		protected abstract RequestInfo[] GetRequestInfo();
		

		public void ParseResponse(string responseXML) {
			LastSendResult = ParseResponseImp(responseXML);
		}

		public void Send(IController c, Action<SendResult> onCompleted) {
			var requests = GetRequestInfo();
			var infos = new Queue<RequestInfo>(requests.Length);

			foreach (var req in requests) {
				//-- Setup the common Request properties
				req.AddHeaderIfNotExists("User-Agent", UserAgent);
				foreach (var header in AdditionalHeaders) {
					req.AddHeaderIfNotExists(header.Key, header.Value);
				}

				infos.Enqueue(req);
			}

			c.RequestProccessor.Process(infos, c.HostNameorAddress, ParseResponse,
				sr => onCompleted.NullableInvoke(sr));
		}
	}
}

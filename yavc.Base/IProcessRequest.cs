using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yavc.Base.Commands {
	public interface IProcessRequest {
		void Process(Queue<RequestInfo> requests, string hostname, Action<string> onResult, Action<SendResult> onCompleted);
	}
}

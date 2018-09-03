using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using yavc.Base.Commands;
using yavc.Base.Util;
using System.Threading;

namespace yavc.Phone.Lib {
	public class BackgroundRequestProcessor : RequestProcessor {

		protected override void ProcessImp(Queue<RequestInfo> infos, string hostname, Action<string> onResult, Action<SendResult> onCompleted) {
			ThreadPool.QueueUserWorkItem(state => base.ProcessImp(infos, hostname, onResult, onCompleted));
		}
	}
}

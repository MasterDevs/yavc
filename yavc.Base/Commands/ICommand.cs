using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace yavc.Base.Commands {
	public interface ICommand {
		void Send(IController c, Action<SendResult> onCompleted);
	}
}

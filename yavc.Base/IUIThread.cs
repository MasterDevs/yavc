using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yavc.Base {
	public interface IUIThread {
		void Invoke(Action a);
		Action Wrap(Action a);
	}
}

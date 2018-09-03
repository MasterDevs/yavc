using System;
using yavc.Base;

namespace yavc.Phone.Lib {
	public class PhoneUIThread : IUIThread {

		#region IUIThread Members

		public void Invoke(Action a) {
			if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
				a();
			else
				System.Windows.Deployment.Current.Dispatcher.BeginInvoke(a);
		}

		public Action Wrap(Action a) {
			return () => Invoke(a);
		}

		#endregion
	}
}

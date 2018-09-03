using System;
using System.Windows;
using yavc.Base;

namespace yavc.WPF.Imp {
	public class WPFThread : IUIThread {
		

		public WPFThread() {
            
		}
		#region IUIThread Members

		public void Invoke(Action a) {
            var d = Application.Current.Dispatcher;
            d.Invoke(a);
		}

		public Action Wrap(Action a) {
			return () => Invoke(a);
		}

		#endregion
	}
}

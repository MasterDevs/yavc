using System;
using Windows.UI.Core;
using yavc.Base;

namespace yavc.Metro.Imp {
	public class MetroUIThread : IUIThread {
		private CoreDispatcher Dispatcher;

		public MetroUIThread(CoreDispatcher dispatcher) {
			this.Dispatcher = dispatcher;
		}
		#region IUIThread Members

		public async void Invoke(Action a) {

			if (Dispatcher.HasThreadAccess)
				a.NullableInvoke();
			else
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a.NullableInvoke());
		}

		public Action Wrap(Action a) {
			return () => Invoke(a);
		}

		#endregion
	}
}

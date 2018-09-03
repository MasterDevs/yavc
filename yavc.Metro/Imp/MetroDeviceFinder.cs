using System;
using yavc.Base;
using yavc.Base.Data;

namespace yavc.Metro.Imp {
	public class MetroDeviceFinder : ADeviceFinder {
		protected override void FindAsync(string whatToFind, int seconds, Action<Device> FoundCallback, Action FinishedSearching) {
			FinishedSearching.NullableInvoke();
		}
	}
}

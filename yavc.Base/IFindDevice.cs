using System;
using yavc.Base.Data;

namespace yavc.Base {
	public interface IFindDevice {
		void FindAllAsync(int seconds, Action<Device> FoundCallback, Action FinishedSearching);
	}
}

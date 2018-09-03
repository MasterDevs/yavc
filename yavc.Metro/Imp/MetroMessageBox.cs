using System;
using yavc.Base;
namespace yavc.Metro.Imp {
	public class MetroMessageBox : IMessageBox {
		
		#region IMessageBox Members

		public void Show(string message) {
			UI.Invoke(async () =>
			{
				var diag = new Windows.UI.Popups.MessageDialog(message);
				await diag.ShowAsync();
			});
		}

		#endregion
	}
}

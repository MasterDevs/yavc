using System;
using System.Windows;
using yavc.Base;
namespace yavc.WPF.Imp {
	public class WPFMessageBox : IMessageBox {
		
		#region IMessageBox Members

		public void Show(string message) {
			UI.Invoke(() =>
			{
                MessageBox.Show(message);
			});
		}

		#endregion
	}
}

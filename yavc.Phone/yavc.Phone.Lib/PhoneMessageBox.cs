using System;
using yavc.Base;
using System.Windows;


namespace yavc.Phone.Lib {
	public class PhoneMessageBox : IMessageBox{

		#region IMessageBox Members

		public void Show(string message) { MessageBox.Show(message); }

		#endregion
	}
}

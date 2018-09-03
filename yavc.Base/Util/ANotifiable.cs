using System;
using System.ComponentModel;

namespace yavc.Base.Util {
	
	public abstract class ANotifiable : INotifyPropertyChanged {
		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyChanged(string propName) {
            UI.Invoke(() =>
            {
                var handler = PropertyChanged;
                if (handler != null && !string.IsNullOrEmpty(propName))
                {

                    PropertyChanged(this, new PropertyChangedEventArgs(propName));
                }
            });
		}
		#endregion
	}
}

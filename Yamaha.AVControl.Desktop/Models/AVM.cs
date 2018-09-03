using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yamaha.AVControl.Desktop.Models {
	public class AVM : DependencyObject {
		protected Controller TheController;

		protected AVM(Controller controller) {
			TheController = controller;
		}
	}
}

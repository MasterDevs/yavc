using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Yamaha.AVControl.Commands;
using Yamaha.AVControl.Commands.Data;
using System.Reflection;

namespace Yamaha.AVControl.Desktop.Models {
	public class VMMain : AVM {

		private string MainZoneImage { get { return @"pack://application:,,,/Images/MainZone.png"; } }
		private string Zone2Image { get { return @"pack://application:,,,/Images/Zone2.png"; } }

		public static VMMain Empty {
			get {
				return new VMMain(string.Empty);
			}
		}

		public VMMain(string RecieverHostNameorAddress)
			: base(new Controller(RecieverHostNameorAddress)) {
			TheController.StatusUpdated += TheController_StatusUpdated;
			SelectedZone = new VMZone(this, Zone.Empty, TheController, null);
			var v = Assembly.GetEntryAssembly().GetName().Version;
			Version = string.Format("v {0}.{1}.{2}", v.Major, v.Minor, v.Build);
		}

		public void UpdateHostNameOrAddresss(string hostOrAddress) {
			IsConnected = TheController.TrySetAddress(hostOrAddress);
		}

		#region Dependency Properties
		public VMSelectable[] Inputs {
			get { return (VMSelectable[])GetValue(InputsProperty); }
			set { SetValue(InputsProperty, value); }
		}
		public static readonly DependencyProperty InputsProperty =
				DependencyProperty.Register("Inputs", typeof(VMSelectable[]), typeof(VMMain));

		public bool IsConnected {
			get { return (bool)GetValue(IsConnectedProperty); }
			set { SetValue(IsConnectedProperty, value); }
		}
		public static readonly DependencyProperty IsConnectedProperty =
				DependencyProperty.Register("IsConnected", typeof(bool), typeof(VMMain));

		public VMSelectable[] Scenes {
			get { return (VMSelectable[])GetValue(ScenesProperty); }
			set { SetValue(ScenesProperty, value); }
		}
		public static readonly DependencyProperty ScenesProperty =
				DependencyProperty.Register("Scenes", typeof(VMSelectable[]), typeof(VMMain));

		public VMZone SelectedZone {
			get { return (VMZone)GetValue(SelectedZoneProperty); }
			set { SetValue(SelectedZoneProperty, value); }
		}
		public static readonly DependencyProperty SelectedZoneProperty =
				DependencyProperty.Register("SelectedZone", typeof(VMZone), typeof(VMMain));

		public SelectionMode SelectionMode {
			get { return (SelectionMode)GetValue(SelectionModeProperty); }
			set { SetValue(SelectionModeProperty, value); }
		}
		public static readonly DependencyProperty SelectionModeProperty =
				DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(VMMain));

		public string Version {
			get { return (string)GetValue(VersionProperty); }
			set { SetValue(VersionProperty, value); }
		}
		public static readonly DependencyProperty VersionProperty =
				DependencyProperty.Register("Version", typeof(string), typeof(VMMain));

		public VMSelectable[] Zones {
			get { return (VMSelectable[])GetValue(ZonesProperty); }
			set { SetValue(ZonesProperty, value); }
		}
		public static readonly DependencyProperty ZonesProperty =
				DependencyProperty.Register("Zones", typeof(VMSelectable[]), typeof(VMMain));
		#endregion

		#region Public Methods
		public void SelectZone(string name) {
			foreach (var z in TheController.Zones) {
				if (z.Name == name) {
					TheController.UpdateStatus();
					SetSelectedZone(z);
				}
			}

			SelectionMode = SelectionMode.None;
		}

		public void SelectScene(string name) {
			var scene = SelectedZone.TheZone.Scenes.Where(s => s.Name == name).FirstOrDefault();
			if (scene != null) {
				TheController.SelectScene(SelectedZone.TheZone, scene);
				TheController.UpdateStatus();
			}
		}

		public void SelectDSP(string name) {
			throw new NotImplementedException();
		}

		public void SelectInput(string name) {
			var input = SelectedZone.TheZone.Inputs.Where(i => i.Param == name).FirstOrDefault();
			if (input != null) {
				TheController.SelectInput(SelectedZone.TheZone, input);
				TheController.UpdateStatus();
			}

			SelectionMode = SelectionMode.None;
		}
		#endregion

		#region Helper Methods
		private void TheController_StatusUpdated(object sender, EventArgs e) {
			if (null == SelectedZone || SelectedZone.TheZone.Name == string.Empty)
				SetSelectedZone(TheController.Zones.FirstOrDefault());

			else {
				foreach (var z in TheController.Zones) {
					if (SelectedZone.TheZone.Name == z.Name) {
						SelectedZone = new VMZone(this, z, TheController, GetZoneImage(z));
						break;
					}
				}
			}

			Zones = TheController.Zones.Select(z => new VMSelectable(TheController, this, z, GetZoneImage(z))).ToArray();
		}

		private string GetZoneImage(Zone z) {
			if (z == null)
				return MainZoneImage;
			else
				return z.Name.ToLower().Contains("main") ? MainZoneImage : Zone2Image;
		}

		private void SetSelectedZone(Zone zone) {
			if (zone == null) return;
			SelectedZone = new VMZone(this, zone, TheController, GetZoneImage(zone));

			Inputs = zone.Inputs.Select(i => new VMSelectable(TheController, this, i)).ToArray();
			Scenes = zone.Scenes.Select(s => new VMSelectable(TheController, this, s)).ToArray();
		}
		#endregion
	}

	public enum SelectionMode {
		None,
		DSP,
		Input,
		Scene,
		Zone,
	}

	public class SelectionModeBoolenConverter : IValueConverter {

		public SelectionMode VisibleMode { get; set; }
		public SelectionMode InvisibleMode { get; set; }
		public SelectionModeBoolenConverter() {
			VisibleMode = SelectionMode.None;
			InvisibleMode = SelectionMode.None;
		}

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (value == null)
				return false;
			SelectionMode mode = (SelectionMode)value;

			if (mode == VisibleMode)
				return true;
			else
				return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (value == null)
				return VisibleMode;

			bool mode = (bool)value;
			if (mode)
				return VisibleMode;
			else
				return InvisibleMode;
		}

		#endregion
	}
}

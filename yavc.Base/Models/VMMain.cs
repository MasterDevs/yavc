using System;
using System.Linq;
using yavc.Base.Data;
using yavc.Base;
using System.Collections.Generic;
using yavc.Base.Util;

namespace yavc.Base.Models {
	public class VMMain : AVM {

		public event EventHandler NetworkTrafficStarted;
		public event EventHandler NetworkTrafficFinished;

		public VMMain(IController c)
			: base(c) {
			SetZones();
			SelectionMode = Models.SelectionMode.Input;
		}

		#region Dependency Properties

        public bool IsConnected
        {
            get { return GetValue(() => IsConnected); }
            set { SetValue(value, () => IsConnected); }
        }

        public bool IsRefreshing
        {
            get { return GetValue(() => IsRefreshing); }
            set { SetValue(value, () => IsRefreshing); }
        }

        public SelectionMode SelectionMode
        {
            get { return GetValue(() => SelectionMode); }
            set { SetValue(value, () => SelectionMode); }
        }

        public string Version
        {
            get { return GetValue(() => Version); }
            set { SetValue(value, () => Version); }
        }

        public VMZone SelectedZone
        {
            get { return GetValue(() => SelectedZone); }
            set {
                if (SetValue(value, () => SelectedZone))
                {
                    foreach (var z in Zones)
                    {
                        z.RefreshLocal();
                    }
                }

            }
        }

        public VMZone[] Zones
        {
            get { return GetValue(() => Zones); }
            set { SetValue(value, () => Zones); }
        }

        
        public string HostNameOrAddress { get { return TheController.Device.HostnameOrIp; } }
		public string FriendlyName { get { return TheController.Device.FriendlyName; } }

        public bool IsPartyModeOn
        {
            get { return GetValue(() => IsPartyModeOn); }
            set { SetValue(value, () => IsPartyModeOn); }
        }
		#endregion

		#region Public Methods
		public void Reconnect() {
			if (string.IsNullOrEmpty(TheController.HostNameorAddress)) {
				Factory.MessageBox.Show("Invalid device address.");
				return;
			}
			UI.Invoke(NotifyStart);
			TheController.TrySetDevice(TheController.Device, sr =>
			{
				UI.Invoke(() =>
				{
					IsConnected = sr.Success;
					if (IsConnected) {
						SetZones();
						RefreshVM();
						return; //-- Note that RefreshVM will call NotifyFinish.
					}
					NotifyFinish();
				});
			});
		}

		public void RefreshSelectedZone(bool notify) {
			if (notify) {
				UI.Invoke(NotifyStart);
				SelectedZone.Refresh(UI.Wrap(NotifyFinish));
			} else {
				SelectedZone.Refresh(null);
			}
		}

		public void RefreshVM() {
			UI.Invoke(NotifyStart);
			TheController.UpdateStatus(sr =>
			{
				UI.Invoke(() =>
				{
					if (sr.Success) {
						NotifyAll();
					}
					NotifyFinish();
				});
			});
		}

		public VMZone SelectZone(string name) {
			foreach (var z in Zones) {
				if (z.TheZone.Name == name) {
					return SelectedZone = z;
				}
			}
            return null;
		}
		public void SelectScene(string name) {
			var scene = SelectedZone.TheZone.Scenes.FirstOrDefault(s => s.Name == name);
			if (scene != null) {
				SelectedZone.SelectScene(scene, UI.Wrap(NotifyFinish));
			}
		}
		public void SelectDSP(string name) {
			if (null == SelectedZone) return;

			SelectedZone.SelectDSP(name);
		}
		public void SelectInput(string name) {
			var input = SelectedZone.TheZone.Inputs.FirstOrDefault(i => i.Param == name);
			if (input != null) {
				SelectedZone.SelectInput(input, UI.Wrap(NotifyFinish));
			}
		}

		public void SetPartyMode(bool partyModeOn) {
			IsPartyModeOn = partyModeOn;
			UI.Invoke(NotifyStart);
			TheController.SetPartyMode(partyModeOn, result =>
			{
				if (result.Success)
					UI.Invoke(() =>
					{
						NotifyAll();
						NotifyFinish();
					});
			});
		}

		/// <summary>
		/// Tries to set the selected zone to the specified zone.
        /// If no zone is selected, then the first zone is set
        /// as the selected zone.
		/// </summary>
        public void SetSelectedZone(string selectedZone) {
            var z = SelectZone(selectedZone);
            if (z == null)
                SelectedZone = Zones.FirstOrDefault();
		}

		public void TogglePartyMode() {
			IsPartyModeOn = !IsPartyModeOn;
			SetPartyMode(IsPartyModeOn);
		}
		#endregion

		#region Helper Methods
		
        private void NotifyFinish() {
			IsRefreshing = false;
			if (NetworkTrafficFinished != null)
				NetworkTrafficFinished(this, EventArgs.Empty);
		}

		private void NotifyStart() {
			IsRefreshing = true;
			if (NetworkTrafficStarted != null)
				NetworkTrafficStarted(this, EventArgs.Empty);
		}

		private void SetZones() {
			if (null == TheController.Zones || 0 == TheController.Zones.Length) {
				Zones = new VMZone[] { new VMZone(this, Zone.Empty, TheController) };
			} else {
				Zones = TheController.Zones.Select(z => new VMZone(this, z, TheController)).ToArray();
			}
			SelectedZone = Zones.FirstOrDefault();
		}
		#endregion
	}
}
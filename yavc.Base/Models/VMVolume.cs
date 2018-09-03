using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Data;
using yavc.Base;
using yavc.Base.Commands;

namespace yavc.Base.Models {
	public class VMVolume : AVM {

        private VMZone ZoneVM;
        private Volume TheVolume
        {
			get {
				var z = Zone;
				if (null == z) return Volume.Empty;
				return z.Volume;
			}
		}
        private Zone Zone { get { if (null == ZoneVM) return null; else return ZoneVM.TheZone; } }

		public bool IsOn { get { return ZoneVM.IsOn; } }

        public double Maximum
        {
            get { return GetValue(() => Maximum); }
            set {
                if (SetValue(value, () => Maximum))
                    NotifyAll();
            }
        }

        public double Minimum
        {
            get { return GetValue(() => Minimum); }
            set {
                if (SetValue(value, () => Minimum))
                    NotifyAll();
                }
        }

		public bool Muted {
			get { return TheVolume.MuteOn; }
			set { if (value != TheVolume.MuteOn) { TheVolume.MuteOn = value; NotifyAll(); } }
		}

		public string MutedString {
			get { return TheVolume.MuteOn ? "Mute On" : "Mute Off"; }
		}

		public double Value {
			get { return TheVolume.Value; }
			set {
				if (value != TheVolume.Value) {
					TheVolume.Value = NormalizeVolume(value);
					NotifyAll();
				}
			}
		}

		public string VolumeString {
			get { return TheVolume.ToString(); }
		}

		public string DSP {
			get {
				if (null == ZoneVM) return string.Empty;
				return ZoneVM.DSPTitle;
			}
		}

		public VMMain MainVM {
			get {
				if (ZoneVM != null)
					return ZoneVM.MainVM;
				else
					return null;
			}
		}
		public VMVolume(VMZone zoneVM, IController c)
			: base(c) {
			ZoneVM = zoneVM;
            ZoneVM.PropertyChanged += (s, e) => { 
                if (e.PropertyName == GetPropertyName(() => ZoneVM.IsOn)) 
                    Notify(() => IsOn); 
            };

			Minimum = Volume.Min;
			Maximum = Volume.Max;
		}

		public void VolumeUp() {
			TheController.VolumeUp(Zone, null);
			NotifyAll();
		}

		public void VolumeDown() {
			TheController.VolumeDown(Zone, null);
			NotifyAll();
		}

		public void ToggleMute() {
			TheController.ToggleMute(Zone, null);
			NotifyAll();
		}

		public void ChangeVolume(double volume) {
			Zone.Volume.Value = NormalizeVolume(volume);
			NotifyAll();
			TheController.ChangeVolume(Zone, sr =>
			{
				if (!sr.Success) {
					MainVM.RefreshVM();
				}
			});
		}

		public void Refresh() {
            NotifyAll();
		}

		private double NormalizeVolume(double volume) {
			volume = Math.Round(volume, 2);

			double frac = volume - Math.Floor(volume);
			if (frac < 0.25)
				return Math.Floor(volume);
			else if (frac < 0.75D)
				return Math.Floor(volume) + 0.5D;
			else
				return Math.Ceiling(volume);
		}
	}
}

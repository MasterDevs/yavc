using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Yamaha.AVControl.Commands.Data;
using Yamaha.AVControl.Commands;
using System.ComponentModel;

namespace Yamaha.AVControl.Desktop.Models {
	public class VMVolume : AVM {


		public double Maximum {
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}
		public static readonly DependencyProperty MaximumProperty =
				DependencyProperty.Register("Maximum", typeof(double), typeof(VMVolume));

		public double Minimum {
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}
		public static readonly DependencyProperty MinimumProperty =
				DependencyProperty.Register("Minimum", typeof(double), typeof(VMVolume));

		public bool Muted {
			get { return (bool)GetValue(MutedProperty); }
			set { SetValue(MutedProperty, value); }
		}
		public static readonly DependencyProperty MutedProperty =
				DependencyProperty.Register("Muted", typeof(bool), typeof(VMVolume));

		public string MutedString {
			get { return (string)GetValue(MutedStringProperty); }
			set { SetValue(MutedStringProperty, value); }
		}
		public static readonly DependencyProperty MutedStringProperty =
				DependencyProperty.Register("MutedString", typeof(string), typeof(VMVolume));

		public double Value {
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public static readonly DependencyProperty ValueProperty =
				DependencyProperty.Register("Value", typeof(double), typeof(VMVolume));

		public string VolumeString {
			get { return (string)GetValue(VolumeStringProperty); }
			set { SetValue(VolumeStringProperty, value); }
		}
		public static readonly DependencyProperty VolumeStringProperty =
				DependencyProperty.Register("VolumeString", typeof(string), typeof(VMVolume));
		
		private Zone Zone;

		public VMVolume(Zone zone, Controller c) : base(c) {
			Zone = zone;
			UpdateVolume(zone);
			Minimum = Volume.Min;
			Maximum = Volume.Max;

			DependencyPropertyDescriptor valDescr = DependencyPropertyDescriptor.
				FromProperty(VMVolume.ValueProperty, typeof(VMVolume));

			if (valDescr != null) {
				valDescr.AddValueChanged(this, delegate {
					if (Value >= Volume.Min && Value <= Volume.Max) {
						Zone.Volume.Value = NormalizeVolume(Value);
						VolumeString = Zone.Volume.ToString();
					}
				});
			}
		}

		public void UpdateVolume(Zone zone) {
			Volume v = zone.Volume;
			Value = v.Value;
			if (v.MuteOn)
				MutedString = "Muted On";
			else
				MutedString = "Muted Off";
			Muted = v.MuteOn;
			VolumeString = v.ToString();
		}

		public void VolumeUp() {
			TheController.VolumeUp(Zone);
		}
		public void VolumeDown() {
			TheController.VolumeDown(Zone);
		}
		public void ToggleMute() {
			TheController.ToggleMute(Zone);
		}

		public void ChangeVolume(double volume) {
			Zone.Volume.Value = NormalizeVolume(volume);
			TheController.ChangeVolume(Zone);
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

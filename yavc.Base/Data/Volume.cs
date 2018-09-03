using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace yavc.Base.Data {
	public class Volume {

		public Volume(double value, string exp, string unit, bool muteOn) {
			Value = value;
			Exp = exp;
			Unit = unit;
			MuteOn = muteOn;
		}

		public Volume() { }

		private double _VolInterval = 0.5;
		public const double Min = -80D;
		public const double Max = 16D;

		[XmlElement]
		public bool MuteOn { get; set; }
		[XmlElement]
		public double Value { get; set; }
		[XmlIgnore]
		public double RawValue { get { return Value * 10; } }
		[XmlElement]
		public string Exp { get; set; }
		[XmlElement]
		public string Unit { get; set; }

		public override string ToString() {
			if (MuteOn)
				return "Mute On";
			else
				return string.Format("{0:00.0} {1}", Value, Unit);
		}

		public void Raise() {
			Value += _VolInterval;
		}

		public void Lower() {
			Value -= _VolInterval;
		}

		public void ToggleMute() {
			MuteOn = !MuteOn;
		}

		public static Volume Empty {
			get {
				Volume v = new Volume();
				v.MuteOn = false;
				v.Value = 0;
				v.Exp = string.Empty;
				v.Unit = string.Empty;
				return v;
			}
		}
	}
}

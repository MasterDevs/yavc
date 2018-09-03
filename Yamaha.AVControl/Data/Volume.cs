using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Util;

namespace Yamaha.AVControl.Commands.Data {
	public class Volume {

		private double _VolInterval = 0.5;
		public const double Min = -80D;
		public const double Max = 16D;
		
		public Volume(XElement data) {
			Value = (double)data.GetIntFromEV("Val", 0) / 10;
			Exp = data.GetStrFromEV("Exp", string.Empty);
			Unit = data.GetStrFromEV("Unit", string.Empty);
			MuteOn = (data.GetStrFromEV("Mute", "Off") != "Off");
		}

		protected Volume() { }

		public bool MuteOn { get; protected set; }
		public double Value { get; set; }
		public double RawValue { get { return Value * 10; } }
		public string Exp { get; protected set; }
		public string Unit { get; protected set; }

		public override string ToString() {
			return string.Format("{0:00.0} {1}", Value, Unit);
		}

		internal void Raise() {
			Value += _VolInterval;
		}

		internal void Lower() {
			Value -= _VolInterval;
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

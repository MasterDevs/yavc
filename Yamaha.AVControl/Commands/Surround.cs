using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Util;

namespace Yamaha.AVControl.Commands {
	public class Surround {

		public Surround(XElement data) {
			StraitOn = data.CompareElementVal("Straight", "On");
			EnhancerOn = data.CompareElementVal("Enhancer", "On");
			DSP = data.GetStrFromEV("Sound_Program", string.Empty);
		}

		public Surround(bool straitOn, bool enhancerOn, string dsp) {
			StraitOn = straitOn;
			EnhancerOn = enhancerOn;
			DSP = dsp;
		}

		public bool StraitOn { get; protected set; }
		public bool EnhancerOn { get; protected set; }
		public string DSP { get; protected set; }
		
		public static Surround Empty {
			get {
				return new Surround(false, false, string.Empty);
			}
		}
	}
}

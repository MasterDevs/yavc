using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Yamaha.AVControl.Util;

namespace Yamaha.AVControl.Commands.Data {
	public class Zone {

		public Zone(string name)  {
			Name = name;
		}

		public bool PowerOn { get; set; }
		public bool Sleeping { get; set; }
		public Volume Volume { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string SelectedInput { get; set; }
		public bool HasVolume { get; set; }
		public Scene[] Scenes { get; set; }
		public Input[] Inputs { get; set; }
		public Surround Surround { get; set; }

		public static Zone Empty {
			get {
				Zone z = new Zone(string.Empty);
				z.Volume = Volume.Empty;
				z.DisplayName = string.Empty;
				z.Inputs = new Input[0];
				z.Scenes = new Scene[0];
				z.SelectedInput = string.Empty;
				z.Surround = Surround.Empty;
				return z;
			}
		}
	}
}

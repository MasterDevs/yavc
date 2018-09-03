using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace yavc.Base.Data {
	public class Zone {

		public Zone() {
			DisplayName = string.Empty;
			Inputs = new Input[0];
			Name = string.Empty;
			Scenes = new Scene[0];
			SelectedInput = string.Empty;
			Volume = Volume.Empty;
		}
		public Zone(string name) :this()  {
			Name = name;
		}

		[XmlElement]
		public bool PowerOn { get; set; }
		[XmlElement]
		public bool PureDirectOn { get; set; }
		[XmlElement]
		public bool Sleeping { get; set; }
		[XmlElement]
		public Volume Volume { get; set; }
		[XmlElement]
		public string Name { get; set; }
		[XmlElement]
		public string DisplayName { get; set; }
		[XmlElement]
		public string SelectedInput { get; set; }
		[XmlElement]
		public bool HasVolume { get; set; }
		[XmlElement]
		public Scene[] Scenes { get; set; }
		[XmlElement]
		public Input[] Inputs { get; set; }
		[XmlElement]
		public Surround Surround { get; set; }

		public static Zone Empty { get { return new Zone(); } }
		/// <summary>
		/// Gets a value indicating the Query String Key used in the Navigation Uri
		/// when passing in a Device
		/// </summary>
		public static string PageUriKey { get { return "Zone"; } }
	}
}

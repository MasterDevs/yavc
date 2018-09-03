using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace yavc.Base.Data {
	public class Input {

		[XmlElement]
		public string Name { get; set; }
		[XmlElement]
		public string Param { get; set; }
		[XmlElement]
		public string Title { get; set; }
		[XmlElement]
		public string IconOn { get; set; }
		[XmlElement]
		public string IconOff { get; set; }
		[XmlElement]
		public string Src_Number { get; set; }
		[XmlElement]
		public string Src_Name { get; set; }

		public override string ToString() {
			return Title;
		}
	}
}

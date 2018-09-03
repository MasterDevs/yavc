using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace yavc.Base.Data {
	public class Scene {

		[XmlElement]
		public string Name { get; set; }
		[XmlElement]
		public string DisplayName { get; set; }
		[XmlElement]
		public string IconRelativeUri { get; set; }


		public Scene() { }

		public Scene(string name, string displayName, string icon) {
			this.Name = name;
			this.DisplayName = displayName;
			this.IconRelativeUri = icon;
		}
	}
}

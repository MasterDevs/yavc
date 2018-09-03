using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Yamaha.AVControl.Commands.Data {
	public class Scene {

		public string Name { get; protected set; }
		public string DisplayName { get; protected set; }

		public Scene(XElement s) {
			Name = s.Name.ToString().Replace("_", " ");
			DisplayName = s.Value;
		}

		internal static Scene[] Parse(XElement xml) {
			List<Scene> scenes = new List<Scene>();
			foreach (var s in xml.Descendants("Scene").Elements()) {
				scenes.Add(new Scene(s));
			}
			return scenes.ToArray(); 
		}
	}
}

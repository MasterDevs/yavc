using System;
using System.Xml.Serialization;

namespace yavc.Base.Data {
	
	public class Source {
		[XmlElement]
		public bool CanList { get; set; }
		[XmlElement]
		public string SourceName { get; set; }
		[XmlElement]
		public bool PlayInfo { get; set; }
		[XmlElement]
		public PlayControl Control { get; set; }
	}

	public class PlayControl {
		[XmlElement]
		public bool CanNext { get; set; }
		[XmlElement]
		public bool CanPlay { get; set; }
		[XmlElement]
		public bool CanPause { get; set; }
		[XmlElement]
		public bool CanPrev { get; set; }
		[XmlElement]
		public bool CanStop { get; set; }
	}
}

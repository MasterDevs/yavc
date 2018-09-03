using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Commands;

namespace yavc.Base {
	public abstract class AFactory : IFactory {
		public IImageCache ImageCache { get; protected set; }
		public IFindDevice DeviceFinder { get; protected set; }
		public IMessageBox MessageBox { get; protected set; }
		public IProcessRequest RequestProcessor { get; protected set; }
		public IStringHelper StringHelper { get; protected set; }
		public ITileService TileService { get; protected set; }
		public IXmlIsoFileStorage XmlIsoFileStore { get; protected set; }
		public IUIThread Dispatcher { get; protected set; }
	}
}

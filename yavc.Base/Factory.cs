using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base.Commands;
using yavc.Base.Data;

namespace yavc.Base {

	public interface IFactory {
		IProcessRequest RequestProcessor { get; }
		IFindDevice DeviceFinder { get; }
		IImageCache ImageCache { get; }
		IMessageBox MessageBox { get; }
		IStringHelper StringHelper { get; }
		ITileService TileService { get; }
		IUIThread Dispatcher { get; }
		IXmlIsoFileStorage XmlIsoFileStore { get; }
	}


	public static class Factory {

		public static void Initialize(IFactory f) { Default = f; }
		public static IFactory Default { get; private set; }

		public static IImageCache ImageCache { get { return Default.ImageCache; } }
		public static IFindDevice DeviceFinder { get { return Default.DeviceFinder; } }
		public static IMessageBox MessageBox { get { return Default.MessageBox; } }
		public static IProcessRequest RequestProcessor { get { return Default.RequestProcessor; } }
		public static IStringHelper StringHelper { get { return Default.StringHelper; } }
		public static ITileService TileService { get { return Default.TileService; } }
		public static IXmlIsoFileStorage FileStore { get { return Default.XmlIsoFileStore; } }
		public static IUIThread Dispatcher {
			get {
				if (Default == null) return null;
				return Default.Dispatcher;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base;

namespace yavc.WPF.Imp {
	public class WPFFactory : AFactory {
		
		public WPFFactory() {
			Dispatcher = new WPFThread();
			DeviceFinder = new WPFDeviceFinder();
			ImageCache = new WPFImageCache();
			MessageBox = new WPFMessageBox();
			RequestProcessor = new WPFRequestProcessor();
			StringHelper = new WPFStringHelper();
			TileService = new WPFTileService();
			XmlIsoFileStore = new WPFXmlIsoStore();
		}
	}
}

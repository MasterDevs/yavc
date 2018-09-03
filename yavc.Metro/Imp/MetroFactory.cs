using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using yavc.Base;

namespace yavc.Metro.Imp {
	public class MetroFactory : AFactory {
		
		public MetroFactory(CoreDispatcher dispatcher) {
			Dispatcher = new MetroUIThread(dispatcher);
			DeviceFinder = new MetroDeviceFinder();
			ImageCache = new MetroImageCache();
			MessageBox = new MetroMessageBox();
			RequestProcessor = new MetroRequestProcessor();
			StringHelper = new MetroStringHelper();
			TileService = new MetroTileService();
			XmlIsoFileStore = new MetroXmlIsoStore();
		}
	}
}

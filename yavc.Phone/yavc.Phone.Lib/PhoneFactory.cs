using yavc.Base;
using yavc.Base.Data;

namespace yavc.Phone.Lib {
	public class PhoneFactory : AFactory {

		public PhoneFactory() {
			Dispatcher = new PhoneUIThread();
			DeviceFinder = new PhoneDeviceFinder();
			ImageCache = new PhoneImageCache();
			MessageBox = new PhoneMessageBox();
			RequestProcessor = new BackgroundRequestProcessor();
			StringHelper = new PhoneStringHelper();
			TileService = new PhoneTileService();
			XmlIsoFileStore = new PhoneXmlIsoFileStore();
		}
	}
}

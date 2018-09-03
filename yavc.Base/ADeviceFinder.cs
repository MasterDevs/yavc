using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using yavc.Base.Data;

namespace yavc.Base {
	public abstract class ADeviceFinder : IFindDevice {

		protected XNamespace yamaha_ns = XNamespace.Get(@"urn:schemas-yamaha-com:device-1-0");
		protected XNamespace default_ns = XNamespace.Get("urn:schemas-upnp-org:device-1-0");

		protected const string multicastIP = "239.255.255.250";
		protected const int multicastPort = 1900;
		protected const int unicastPort = 1901;
		protected const int MaxResultSize = 8000;
		
		#region IFindDevice Members

		public void FindAllAsync(int seconds, Action<Data.Device> FoundCallback, Action FinishedSearching) {
			if (seconds < 1 || seconds > 5)
				throw new ArgumentOutOfRangeException();

			
			//var id = @"urn:schemas-upnp-org:device:MediaRenderer:1";
			//var id = @"uuid:9ab0c000-f668-11de-9976-00a0de716b7a";

			string find = "M-SEARCH * HTTP/1.1\r\n" +
				 "HOST: 239.255.255.250:1900\r\n" +
				 "MAN: \"ssdp:discover\"\r\n" +
				 "MX: " + seconds.ToString() + "\r\n" +
				 "ST: ssdp:all\r\n" +
				 "\r\n";

			FindAsync(find, seconds, FoundCallback, FinishedSearching);
		}

		protected abstract void FindAsync(string whatToFind, int seconds, Action<Device> FoundCallback, Action FinishedSearching);

		#endregion

		protected static string GetSSDPLocation(string response) {
			var dict = ParseSSDPResponse(response);
			if (dict != null && dict.ContainsKey("location"))
				return dict["location"];
			else
				return null;
		}
		
		protected static Dictionary<string, string> ParseSSDPResponse(string response) {
			StringReader reader = new StringReader(response);

			string line = reader.ReadLine();
			if (line != "HTTP/1.1 200 OK")
				return null;

			Dictionary<string, string> result = new Dictionary<string, string>();

			for (; ; ) {
				line = reader.ReadLine();
				if (line == null)
					break;
				if (line != "") {
					int colon = line.IndexOf(':');
					if (colon < 1) {
						return null;
					}
					string name = line.Substring(0, colon).Trim();

					string value = line.Substring(colon + 1).Trim();
					if (string.IsNullOrEmpty(name)) {
						return null;
					}
					result[name.ToLowerInvariant()] = value;
				}
			}
			return result;
		}

		protected void ParseMetafile(string metafile, Action<Device> OnFound) {
			try {
				var x = XElement.Parse(metafile);

				if (null != x) {
					var xtype = x.Descendants(yamaha_ns + @"X_specType").FirstOrDefault();

					//-- If we have this type, that means we have a Yamaha Media Receiver that's
					// compatible wiht our application.
					if (xtype != null) {
						//-- Time to get the Device Data
						var d = x.Element(default_ns + "device");
						var friendlyName = d.GetStrFromEV(default_ns + "friendlyName", string.Empty);
						var url = new Uri(d.GetStrFromEV(default_ns + "presentationURL", string.Empty));

						OnFound.NullableInvoke(new Device(url.Host, friendlyName));
					}
				}
			} catch { }
		}

		protected byte[] GetBytes(string value) {
			return Encoding.UTF8.GetBytes(value);
		}

		protected string GetString(byte[] buffer, int index, int count) {
			return Encoding.UTF8.GetString(buffer, index, count);
		}
	}
}

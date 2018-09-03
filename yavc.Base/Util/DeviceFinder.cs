using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Threading;
using System.Xml.Linq;
using yavc.Base.Data;

namespace yavc.Base.Util {

	/// <summary>
	/// See Andy  Pennell's Blog (http://blogs.msdn.com/b/andypennell/archive/2011/08/24/attempting-upnp-on-windows-phone-7-5-mango-part-1-ssdp-discovery.aspx)
	/// or http://wpupnp.codeplex.com/
	/// </summary>
	public class DeviceFinder {

		public const int DefaultTimeout = 4;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds">Valid values</param>
		public void FindAllAsync(int seconds, Action<Device> FoundCallback, Action FinishedSearching) {
			//var id = @"urn:schemas-upnp-org:device:MediaRenderer:1";
			//var id = @"uuid:9ab0c000-f668-11de-9976-00a0de716b7a";
			var id = @"ssdp:all";
			FindAsync(id, seconds, FoundCallback, FinishedSearching);
		}

		private void FindAsync(string whatToFind, int seconds, Action<Device> FoundCallback, Action FinishedSearching) {
			const string multicastIP = "239.255.255.250";
			const int multicastPort = 1900;
			const int unicastPort = 1901;
			const int MaxResultSize = 8000;

			if (seconds < 1 || seconds > 4)
				throw new ArgumentOutOfRangeException();

			string find = "M-SEARCH * HTTP/1.1\r\n" +
				 "HOST: 239.255.255.250:1900\r\n" +
				 "MAN: \"ssdp:discover\"\r\n" +
				 "MX: " + seconds.ToString() + "\r\n" +
				 "ST: " + whatToFind + "\r\n" +
				 "\r\n";

			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			byte[] MulticastData = Encoding.UTF8.GetBytes(find);
			socket.SendBufferSize = MulticastData.Length;
			SocketAsyncEventArgs sendEvent = new SocketAsyncEventArgs();
			sendEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(multicastIP), multicastPort);
			sendEvent.SetBuffer(MulticastData, 0, MulticastData.Length);
			sendEvent.Completed += new EventHandler<SocketAsyncEventArgs>((sender, e) =>
			{
				try {
					if (e.SocketError == SocketError.Success) {
						if (e.LastOperation == SocketAsyncOperation.SendTo) {
							// When the initial multicast is done, get rady to receive responses
							e.RemoteEndPoint = new IPEndPoint(IPAddress.Any, unicastPort);
							socket.ReceiveBufferSize = MaxResultSize;
							byte[] receiveBuffer = new byte[MaxResultSize];
							e.SetBuffer(receiveBuffer, 0, MaxResultSize);
							socket.ReceiveFromAsync(e);
						} else if (e.LastOperation == SocketAsyncOperation.ReceiveFrom) {
							// Got a response, so decode it
							string result = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
							if (result.StartsWith("HTTP/1.1 200 OK")) {
								ParseResult(result, FoundCallback);
							}

							// And kick off another read
							socket.ReceiveFromAsync(e);
						}
					}
				} catch  { }
			});

			// Set a one-shot timer for double the Search time, to be sure we are done before we stop everything
			DispatcherTimer dt = new DispatcherTimer();
			dt.Interval = TimeSpan.FromSeconds(seconds * 2);
			dt.Tick += new EventHandler((sender, args) => {
				FinishedSearching.NullableInvoke();
				socket.Close();
				dt.Stop(); //-- Do not keep invoking the timer.
			});
			dt.Start();
			
			// Kick off the initial Send
			socket.SendToAsync(sendEvent);
		}

		private void ParseResult(string result, Action<Device> FoundCallback) {
			try {
				var metafile_url = DeviceFinder.GetSSDPLocation(result);

				var wc = new WebClient();
				wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadMetaData_Completed);
				wc.DownloadStringAsync(new Uri(metafile_url), FoundCallback);
			} catch { }
		}

		private void DownloadMetaData_Completed(object sender, DownloadStringCompletedEventArgs e) {
			if (null != e.Error) return;

			try {
				var x = XElement.Parse(e.Result);

				var yamaha_ns = XNamespace.Get(@"urn:schemas-yamaha-com:device-1-0");
				var default_ns = XNamespace.Get("urn:schemas-upnp-org:device-1-0");

				if (null != x) {
					var xtype = x.Descendants(yamaha_ns + @"X_specType").FirstOrDefault();

					//-- If we have this type, that means we have a Yamaha Media Receiver that's
					// compatible wiht our application.
					if (xtype != null) {
						//-- Time to get the Device Data
						var d = x.Element(default_ns + "device");
						var friendlyName = d.GetStrFromEV(default_ns + "friendlyName", string.Empty);
						var url = new Uri(d.GetStrFromEV(default_ns + "presentationURL", string.Empty));

						var found = e.UserState as Action<Device>;

						found.NullableInvoke(new Device(url.Host, friendlyName));
					}
				}
			} catch { }
		}

		public static string GetSSDPLocation(string response) {
			var dict = ParseSSDPResponse(response);
			if (dict != null && dict.ContainsKey("location"))
				return dict["location"];
			else
				return null;
		}

		private static Dictionary<string, string> ParseSSDPResponse(string response) {
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
	}
}

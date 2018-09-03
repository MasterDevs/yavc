using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Threading;
using yavc.Base;
using yavc.Base.Data;

namespace yavc.Phone.Lib {
	
	/// <summary>
	/// See Andy  Pennell's Blog (http://blogs.msdn.com/b/andypennell/archive/2011/08/24/attempting-upnp-on-windows-phone-7-5-mango-part-1-ssdp-discovery.aspx)
	/// or http://wpupnp.codeplex.com/
	/// </summary>
	public class PhoneDeviceFinder : ADeviceFinder {

		protected override void FindAsync(string whatToFind, int seconds, Action<Device> FoundCallback, Action FinishedSearching) {

			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			byte[] MulticastData = GetBytes(whatToFind);
			socket.SendBufferSize = MulticastData.Length;
			var sendEvent = new SocketAsyncEventArgs();
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
							string result = GetString(e.Buffer, 0, e.BytesTransferred); 
							if (result.StartsWith("HTTP/1.1 200 OK")) {
								ParseResult(result, FoundCallback);
							}

							// And kick off another read
							socket.ReceiveFromAsync(e);
						}
					}
				} catch { }
			});

			// Set a one-shot timer for double the Search time, to be sure we are done before we stop everything
			DispatcherTimer dt = new DispatcherTimer();
			dt.Interval = TimeSpan.FromSeconds(seconds * 2);
			dt.Tick += new EventHandler((sender, args) =>
			{
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
				var metafile_url = GetSSDPLocation(result);

				var wc = new WebClient();
				wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadMetaData_Completed);
				wc.DownloadStringAsync(new Uri(metafile_url), FoundCallback);
			} catch { }
		}

		private void DownloadMetaData_Completed(object sender, DownloadStringCompletedEventArgs e) {
			if (null != e.Error) return;

			ParseMetafile(e.Result, e.UserState as Action<Device>);
			
		}
	}
}

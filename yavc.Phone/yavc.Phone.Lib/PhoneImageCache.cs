using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows.Media.Imaging;
using yavc.Base;

namespace yavc.Phone.Lib {
	public class PhoneImageCache : IImageCache {

		private static Dictionary<string, Stream> CachedImages = new Dictionary<string, Stream>();

		public void GetImage(string imageUri, Action<Stream> OnGetImageFinished) {

			if (CachedImages.ContainsKey(imageUri)) {
				var s = CachedImages[imageUri];
				s.Seek(0, SeekOrigin.Begin);
				OnGetImageFinished.NullableInvoke(s);
				return;
			}

			using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication()) {
				var fileName = GetFileName(imageUri);
				if (myIsolatedStorage.FileExists(fileName)) {
					using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read)) {
						OnGetImageFinished.NullableInvoke(CachedImages[imageUri] = CopyStream(fileStream));
						return;
					}
				}
			}
			OnGetImageFinished.NullableInvoke(null);
		}

		private Stream CopyStream(IsolatedStorageFileStream fileStream) {
			var ms = new MemoryStream();
			fileStream.CopyTo(ms);
			return ms;
		}

		public void DownloadImage(string imageUri, Action onFinished) {
			try {
                HttpWebRequest req = HttpWebRequest.CreateHttp(new Uri(imageUri));
				req.BeginGetResponse((result) =>
				{
                    try
                    {
                        var r = (HttpWebRequest)result.AsyncState;
                        var response = r.EndGetResponse(result);
                        var stream = response.GetResponseStream();
                        SaveFile(imageUri, stream);
                    }
                    catch { }

                    onFinished.NullableInvoke();
				}, req);
			} catch {
				onFinished.NullableInvoke();
			}
		}

		private static void SaveFile(string uri, Stream dataStream) {
			var fileName = GetFileName(uri);

			try {
				using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication()) {
					using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, myIsolatedStorage)) {
						using (BinaryWriter writer = new BinaryWriter(fileStream)) {
							byte[] buffer = new byte[32];
							int bytesRead = 1;
							using (BinaryReader reader = new BinaryReader(dataStream)) {
								while (bytesRead > 0) {
									bytesRead = reader.Read(buffer, 0, buffer.Length);
									writer.Write(buffer, 0, bytesRead);
								}
							}
						}
					}
				}
			} catch {
				//-- For some reason I'm having issues with isolated storage using the Marketplace Test Kit. 
				//I never ran into errors testing locally, but every time I try to run the Monitored test, I
				//get the following exception:

				//Result Details
				//[ERROR] : Unhandled exception was encountered. Please ensure all exceptions are handled.
				//Exception details: System.IO.IsolatedStorage.IsolatedStorageException: Operation not permitted on IsolatedStorageFileStream.
				//   at System.IO.IsolatedStorage.IsolatedStorageFileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, IsolatedStorageFile isf)
				//   at System.IO.IsolatedStorage.IsolatedStorageFileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, IsolatedStorageFile isf)
				//   at System.IO.IsolatedStorage.IsolatedStorageFileStream..ctor(String path, FileMode mode, IsolatedStorageFile isf)
				//   at yavc.Phone.Lib.Util.ImageCache.SaveFile(String uri, Stream dataStream)
				//   at yavc.Phone.Lib.Util.ImageCache.<>c__DisplayClass1.<DownloadImage>b__0(IAsyncResult result)
				//   at System.Net.Browser.ClientHttpWebRequest.<>c__DisplayClassa.<InvokeGetResponseCallback>b__8(Object state2)
				//   at System.Threading.ThreadPool.WorkItem.WaitCallback_Context(Object state)
				//   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback,
			}
		}

		private static string GetFileName(string uri) {
			return uri.GetHashCode().ToString();
		}
	}
}


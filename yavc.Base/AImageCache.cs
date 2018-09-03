using System;
using System.Collections.Generic;
using System.IO;

namespace yavc.Base {
	public abstract class AImageCache : IImageCache {

		protected static Dictionary<string, Stream> CachedImages = new Dictionary<string, Stream>();

		#region IImageCache Members

		public virtual void GetImage(string imageUri, Action<Stream> OnGetImage) {
			
			if (CachedImages.ContainsKey(imageUri)) {
				var s = CachedImages[imageUri];
				s.Seek(0, SeekOrigin.Begin);
				OnGetImage.NullableInvoke(s);
			}

			var storage = Factory.FileStore;
			var fileName = GetFileName(imageUri);

			storage.FileExists(fileName, fileExists =>
			{
				if (fileExists) {
					storage.GetStream(fileName, s =>
					{
						CachedImages[imageUri] = s;
						OnGetImage.NullableInvoke(s);
					});
				}
				else
					OnGetImage.NullableInvoke(null);
			});
		}

		public abstract void DownloadImage(string imageUri, Action onFinished);

		#endregion

		#region Helpers
		protected static string GetFileName(string uri) { return uri.GetHashCode().ToString(); }
		#endregion
	}
}

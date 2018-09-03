using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using yavc.Base;

namespace yavc.Metro.Imp {
	public class MetroImageCache : AImageCache {

		public override void GetImage(string imageUri, Action<Stream> OngetImageFinished) {
			var fileName = GetFileName(imageUri);
			Factory.FileStore.GetStream(fileName, OngetImageFinished);
		}
		
		public override async void DownloadImage(string imageUri, Action onFinished) {
			var client = new HttpClient();
			var stream = await client.GetStreamAsync(imageUri);
			Factory.FileStore.WriteStream(stream, GetFileName(imageUri), onFinished);
		}
	}
}

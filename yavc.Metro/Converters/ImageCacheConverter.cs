using System.Collections.Generic;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using yavc.Base;
namespace yavc.Metro.Converters {
	public class ImageCacheConverter : IValueConverter{
		#region IValueConverter Members

		private static Dictionary<string, BitmapImage> Images = new Dictionary<string, BitmapImage>();

		public object Convert(object value, System.Type targetType, object parameter, string language) {
			var imgUrl = value as string;
			
			if (!string.IsNullOrEmpty(imgUrl)) {

				if (Images.ContainsKey(imgUrl))
					return Images[imgUrl];

				var bitmap = new BitmapImage();
				var ms = new InMemoryRandomAccessStream();

				Factory.ImageCache.GetImage(imgUrl, async imgStream =>
				{
					var mssw = ms.AsStreamForWrite();
					await imgStream.CopyToAsync(mssw);
					await mssw.FlushAsync();
					ms.Seek(0);
					
					bitmap.SetSource(ms);
				});

				return bitmap;
			}
			return value;
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, string language) {
			throw new System.NotImplementedException();
		}

		#endregion
	}
}

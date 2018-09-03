using System;
using System.Windows.Data;
using yavc.Base;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.IO;


namespace yavc.Phone.Lib.Util {
	public class ImageCacheConverter : IValueConverter {
		#region IValueConverter Members

		private static Dictionary<string, BitmapImage> Images = new Dictionary<string, BitmapImage>();

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			var imgUrl = value as string;

			if (!string.IsNullOrEmpty(imgUrl)) {

				if (Images.ContainsKey(imgUrl))
					return Images[imgUrl];

				var bitmap = new BitmapImage();

				Factory.ImageCache.GetImage(imgUrl, imgStream =>
				{
					if(null != imgStream) bitmap.SetSource(imgStream);
				});

				return bitmap;
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return value;
		}

		#endregion
	}
}

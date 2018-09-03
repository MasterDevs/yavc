using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace yavc.Base {
	public interface IImageCache {
		void GetImage(string imageUri, Action<Stream> OnGetImage);
		void DownloadImage(string imageUri, Action onFinished);
	}
}

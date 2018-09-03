using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;
using yavc.Base;

namespace yavc.Metro.Imp {
	public class MetroXmlIsoStore : IXmlIsoFileStorage {
		#region IXmlIsoFileStorage Members

		public async void FileExists(string fileName, Action<bool> OnFileExists) {
			var exists = false;
			try {
				var file = await Folder.GetFileAsync(fileName);

				exists = true;
			} catch (FileNotFoundException) { }

			OnFileExists.NullableInvoke(exists);
		}

		public async void GetStream(string fileName, Action<Stream> OnGetStreamFinished) {
			var result = new MemoryStream();
			
			try {
				var file = await Folder.GetFileAsync(fileName);
				using (var inputStream = await file.OpenReadAsync())
				using (var readStream = inputStream.AsStreamForRead()) {
					await readStream.CopyToAsync(result);
					await result.FlushAsync();
				}

			} catch { }

			result.Seek(0, SeekOrigin.Begin);
			OnGetStreamFinished(result);
		}

		async public static Task<string> ReadAllTextAsync(StorageFile storageFile) {
			string content;
			var inputStream = await storageFile.OpenAsync(FileAccessMode.Read);
			var readStream = inputStream.GetInputStreamAt(0);
			var reader = new DataReader(readStream);
			uint fileLength = await reader.LoadAsync((uint)inputStream.Size);
			content = reader.ReadString(fileLength);
			return content;
		} 

		public async void Read<T>(string filename, Action<T> OnReadFinished) {
			var result = default(T);

			try {
				var fileTask = await Folder.GetFileAsync(filename);

				using (var fileStream = await fileTask.OpenReadAsync())
				using (var inStream = fileStream.GetInputStreamAt(0)) {
					var serializer = new XmlSerializer(typeof(T));
					result = (T)serializer.Deserialize(inStream.AsStreamForRead());
				}
			} catch { }

			OnReadFinished.NullableInvoke(result);
		}

		public async void WriteFile<T>(T obj, string filename, Action OnWriteFinished) {
			var serializer = new XmlSerializer(typeof(T));

			var file = await Folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

			try {
				using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
				using (var outStream = fileStream.GetOutputStreamAt(0)) {
					serializer.Serialize(outStream.AsStreamForWrite(), obj);
				}
			} catch { }
			
			OnWriteFinished.NullableInvoke();
		}

		public async void WriteStream(Stream obj, string fileName, Action OnWriteFinished) {
			try {
				var file = await Folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

				using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
				using (var writeStream = fileStream.AsStreamForWrite()) {
					await obj.CopyToAsync(writeStream);
					await writeStream.FlushAsync();
				}
			} catch { }
			
			OnWriteFinished.NullableInvoke();
		}

		#endregion

		#region Helper Methods

		private static StorageFolder Folder { get { return ApplicationData.Current.RoamingFolder; } }

		#endregion
	}
}

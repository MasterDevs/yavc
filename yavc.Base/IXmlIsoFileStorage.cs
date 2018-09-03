using System;
using System.IO;

namespace yavc.Base {
	/// <summary>
	/// Provides a simple interface for reading and writing objects that are XML Serializable
	/// to Isolated Storage
	/// </summary>
	public interface IXmlIsoFileStorage {
		void WriteFile<T>(T obj, string fileName, Action OnWriteFinished);
		void Read<T>(string fileName, Action<T> OnReadFinished);
		void FileExists(string fileName, Action<bool> OnFileExistsFinished);
		void GetStream(string fileName, Action<Stream> OnGetStreamFinished);
		void WriteStream(Stream stream, string fileName, Action OnWriteStreamFinished);
	}
}

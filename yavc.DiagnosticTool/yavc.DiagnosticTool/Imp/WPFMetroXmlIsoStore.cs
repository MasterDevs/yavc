using System;
using yavc.Base;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;


namespace yavc.WPF.Imp
{
    public class WPFXmlIsoStore : IXmlIsoFileStorage
    {

        #region IFileStorage Members
        public void FileExists(string fileName, Action<bool> OnFileExists)
        {
            using (var myStore = GetUserStore())
            {
                var files = myStore.GetFileNames(fileName);
                if (files == null || files.Length == 0)
                    OnFileExists.NullableInvoke(false);
                else
                    OnFileExists.NullableInvoke(true);
            }
        }

        public void GetStream(string fileName, Action<Stream> OnGetStreamFinished)
        {
            var ms = new MemoryStream();
            try
            {
                using (var myStore = GetUserStore())
                using (var isoStream = new IsolatedStorageFileStream(fileName, FileMode.Open, myStore))
                using (var reader = new StreamReader(isoStream)) 
                using (var writer = new StreamWriter(ms))
                {
                    isoStream.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                }
            }
            catch { }

            OnGetStreamFinished.NullableInvoke(ms);
        }

        public void Read<T>(string fileName, Action<T> OnReadFinished)
        {
            var result = default(T);
            try
            {
                using (var myStore = GetUserStore())
                using (var isoStream = new IsolatedStorageFileStream(fileName, FileMode.Open, myStore))
                {
                    result = (T)GetSerializer(typeof(T)).Deserialize(isoStream);
                }
            }
            catch { }
            OnReadFinished.NullableInvoke(result);
        }

        public void WriteFile<T>(T obj, string fileName, Action OnWriteFinished)
        {
            Write(fileName, isoStream => GetSerializer(typeof(T)).Serialize(isoStream, obj), OnWriteFinished);
        }

        public void WriteStream(Stream stream, string fileName, Action OnWriteStreamFinished)
        {
            Write(fileName, isoStream => stream.CopyTo(isoStream), OnWriteStreamFinished);
        }

        #endregion

        private static Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();
        private XmlSerializer GetSerializer(Type type)
        {
            if (serializers.ContainsKey(type))
                return serializers[type];

            return serializers[type] = new XmlSerializer(type);
        }

        private void Write(string fileName, Action<IsolatedStorageFileStream> streamAction, Action OnWriteFinished)
        {
            try
            {
                using (var myStore = GetUserStore())
                using (var isoStream = new IsolatedStorageFileStream(fileName, FileMode.Create, myStore))
                {
                    streamAction(isoStream);
                }
            }
            catch { }
            OnWriteFinished.NullableInvoke();
        }

        protected IsolatedStorageFile GetUserStore()
        {
            return IsolatedStorageFile.GetUserStoreForApplication();
        }
    }
}

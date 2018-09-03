using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static void CopyTo(this Stream me, Stream destination)
        {
            me.CopyTo(destination, 4096);
        }

        public static void CopyTo(this Stream me, Stream destination, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int read;
            while ((read = me.Read(buffer, 0, buffer.Length)) > 0)
            {
                destination.Write(buffer, 0, read);
            }

            //-- Reset the destination to the beginning
            destination.Position = 0;
        }
    }
}

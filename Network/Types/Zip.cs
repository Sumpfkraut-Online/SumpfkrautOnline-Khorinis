using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace GUC.Types
{
    public class Zip
    {
        public static void Compress(RakNet.BitStream source, int offset, RakNet.BitStream dest)
        {
            MemoryStream ms = new MemoryStream();
            MemoryStream sms = new MemoryStream();
            byte[] sData = source.GetData();
            sms.Write(sData, offset, sData.Length - offset);

            Compress(sms, ms);
            sms.Close();
            sms.Dispose();

            byte[] arr = ms.ToArray();
            dest.Write(arr.Length);
            dest.Write(arr, (uint)arr.Length);
        }

        public static void Compress(Stream source, RakNet.BitStream dest)
        {
            MemoryStream ms = new MemoryStream();
            Compress(source, ms);

            byte[] arr = ms.ToArray();
            dest.Write(arr.Length);
            dest.Write(arr, (uint)arr.Length);
        }

        public static void Decompress(RakNet.BitStream source, RakNet.BitStream dest)
        {
            int len = 0;
            source.Read(out len);
            byte[] arr = new byte[len];
            source.Read(arr, (uint)len);




            MemoryStream input = new MemoryStream();
            input.Write(arr, 0, len);

            MemoryStream output = new MemoryStream();
            Decompress(input, output);


            arr = output.ToArray();

            dest.Reset();
            dest.Write(arr, (uint)arr.Length);
            output.Close();
            output.Dispose();
        }

        public static void Decompress(RakNet.BitStream source, Stream dest)
        {
            int len = 0;
            source.Read(out len);
            byte[] arr = new byte[len];
            source.Read(arr, (uint)len);




            MemoryStream input = new MemoryStream();
            input.Write(arr, 0, len);

            Decompress(input, dest);
        }



        public static void Compress(Stream source, Stream dest)
        {
            source.Position = 0;
            using (GZipStream zipstream = new GZipStream(dest, CompressionMode.Compress))
                source.CopyTo(zipstream);
            
        }

        public static void Decompress(Stream source, Stream dest)
        {
            source.Position = 0;
            using (GZipStream zipstream = new GZipStream(source, CompressionMode.Decompress))
            {
                zipstream.CopyTo(dest);
            }
        }
    }
}

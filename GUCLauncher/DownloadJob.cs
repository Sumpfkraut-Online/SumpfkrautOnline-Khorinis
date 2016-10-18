using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace GUCLauncher
{
    class DownloadJob
    {
        const int BufferSize = 2048;

        public List<PackFile> Files = new List<PackFile>();

        public void Load(string url, Action<int> AddBytes)
        {
            int from = Files[0].offset;
            int to = Files[Files.Count - 1].offset + Files[Files.Count - 1].CompressedSize;
            int length = to - from;

            using (var response = Download.GetResponse(url, from, to))
            {
                Stream stream;
                if (response != null && (stream = response.GetResponseStream()) != null)
                {
                    byte[] buf = new byte[BufferSize];
                    int pos = from;

                    for (int i = 0; i < Files.Count; i++)
                    {
                        PackFile file = Files[i];
                        using (FileStream fs = file.Info.Create())
                        using (MemoryStream ms = new MemoryStream(BufferSize))
                        using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                        {
                            int left;
                            while ((left = file.offset + file.CompressedSize - pos) > 0)
                            {
                                int read = stream.Read(buf, 0, left > BufferSize ? BufferSize : left);
                                AddBytes(read);
                                pos += read;

                                ms.Position = 0;
                                ms.SetLength(0);
                                ms.Write(buf, 0, read);

                                ms.Position = 0;
                                ds.CopyTo(fs);
                            }
                        }

                        if (i + 1 < Files.Count && Files[i + 1].offset > pos)
                        {
                            // read up to next file
                            int left;
                            while ((left = Files[i + 1].offset - pos) > 0)
                            {
                                int read = stream.Read(buf, 0, left > BufferSize ? BufferSize : left);
                                pos += read;
                            }
                        }
                    }
                }
            }
        }
    }
}

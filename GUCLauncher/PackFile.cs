using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Security.Cryptography;

namespace FilePacker
{
    class PackFile : PackObject
    {
        const int BufferSize = 4096;
        static readonly byte[] buffer = new byte[BufferSize];

        int offset;
        byte[] hash;
        
        public override void Write(BinaryWriter header, Stream pack, string folder)
        {
            // Write file information into the header (Type, Name, Offset, Hash).
            // Compress file into Data Pack.

            header.Write((byte)(this.IsLast ? 4 : 0));
            header.Write(Name);
            header.Write((int)pack.Position);

            using (MD5 md5 = new MD5CryptoServiceProvider())
            using (DeflateStream stream = new DeflateStream(pack, CompressionLevel.Optimal, true))
            using (FileStream fs = new FileStream(Path.Combine(folder, Name), FileMode.Open, FileAccess.Read))
            {
                int readLen;
                while ((readLen = fs.Read(buffer, 0, BufferSize)) > 0)
                {
                    md5.TransformBlock(buffer, 0, readLen, buffer, 0);
                    stream.Write(buffer, 0, readLen);
                }
                md5.TransformFinalBlock(buffer, 0, 0);
                header.Write(md5.Hash, 0, 16);
            }
        }

        public override void Read(BinaryReader header)
        {
            base.Read(header);
            this.offset = header.ReadInt32();
            this.hash = header.ReadBytes(16);
        }

    }
}

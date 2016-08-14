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
        public override PackObjectType POType { get { return PackObjectType.File; } }

        new public FileInfo Info { get { return (FileInfo)this.info; } }

        public PackFile(FileInfo info) : base(info)
        {
        }

        int offset;
        byte[] hash;

        const int BufferSize = 4096;
        static readonly byte[] buffer = new byte[BufferSize];
        public void Write(Stream pack)
        {
            // Compress file into Data Pack.

            this.offset = (int)pack.Position;

            using (MD5 md5 = new MD5CryptoServiceProvider())
            using (DeflateStream stream = new DeflateStream(pack, CompressionLevel.Optimal, true))
            using (FileStream fs = Info.OpenRead())
            {
                int readLen;
                while ((readLen = fs.Read(buffer, 0, BufferSize)) > 0)
                {
                    md5.TransformBlock(buffer, 0, readLen, buffer, 0);
                    stream.Write(buffer, 0, readLen);
                }
                md5.TransformFinalBlock(buffer, 0, 0);
                this.hash = md5.Hash;
            }
        }

        public override void WriteHeader(BinaryWriter header)
        {
            base.WriteHeader(header);
            header.Write(this.offset);
            header.Write(this.hash);
        }

         public void ReadHeader(BinaryReader header)
         {
             this.offset = header.ReadInt32();
             this.hash = header.ReadBytes(16);
         }
    }
}

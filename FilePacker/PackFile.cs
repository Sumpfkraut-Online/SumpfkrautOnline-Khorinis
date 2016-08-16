using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        const int BufferSize = 8192;
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

        public override void WriteHeader(PacketStream header)
        {
            base.WriteHeader(header);
            header.Write(this.offset);
            header.WriteBytes(this.hash);
        }

        public void ReadHeader(PacketStream header)
        {
            this.offset = header.ReadInt();
            this.hash = header.ReadBytes(16);
        }

        public int PreCheck(List<PackFile> checkList, List<PackFile> neededList)
        {
            if (this.Info.Exists)
            {
                checkList.Add(this);
                return (int)this.Info.Length;
            }
            else
            {
                neededList.Add(this);
                return 0;
            }
        }

        public bool Check(Action<int> AddBytes)
        {
            byte[] otherHash;
            using (MD5 md5 = new MD5CryptoServiceProvider())
            using (Stream stream = this.Info.OpenRead())
            {
                int readLen;
                while ((readLen = stream.Read(buffer, 0, BufferSize)) > 0)
                {
                    md5.TransformBlock(buffer, 0, readLen, buffer, 0);
                    AddBytes(readLen);
                }
                md5.TransformFinalBlock(buffer, 0, 0);
                otherHash = md5.Hash;
            }

            for (int i = 0; i < 16; i++)
            {
                if (otherHash[i] != this.hash[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}

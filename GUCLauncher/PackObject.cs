using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FilePacker
{
    abstract class PackObject
    {
        protected static class PackType
        {
            public const int File = 0,
            Directory = 1,
            Directory_Empty = 2;
        }

        public string Name;
        public bool IsLast;
  
        public abstract void Write(BinaryWriter header, Stream pack, string folder);
        public virtual void Read(BinaryReader header)
        {
            this.Name = header.ReadString();
        }

        public static PackObject ReadNew(BinaryReader br)
        {
            int type = br.ReadByte();
            if ((type & ~4) == PackType.File)
            {
                PackFile file = new PackFile();
                file.IsLast = (type & 4) > 0;
                file.Read(br);
                return file;
            }
            else
            {
                PackDir dir = new PackDir();
                dir.IsLast = (type & 4) > 0;
                dir.Read(br);
                if (type == PackType.Directory_Empty)
                    dir.IsEmpty = true;

                return dir;
            }
        }
    }
}

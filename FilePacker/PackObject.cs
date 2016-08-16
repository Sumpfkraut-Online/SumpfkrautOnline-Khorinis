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
        public enum PackObjectType
        {
            File,
            Directory
        }

        public abstract PackObjectType POType { get; }

        protected FileSystemInfo info;
        public FileSystemInfo Info { get { return this.info; } }
        public string Name { get { return this.info.Name; } }
        public bool IsLast;
  
        public PackObject(FileSystemInfo info)
        {
            this.info = info;
        }
        
        public virtual void WriteHeader(PacketStream header)
        {
            if (this.IsLast)
            {
                header.WriteByte((int)this.POType | 2);
            }
            else
            {
                header.WriteByte((int)this.POType);
            }
            
            header.WriteStringShort(this.Name);
        }
        
        public static PackObject ReadNew(PacketStream stream, string path)
        {
            int type = stream.ReadByte();
            string name = Path.Combine(path, stream.ReadStringShort());
            bool isLast = (type & 2) > 0;

            if ((type & ~2) == (int)PackObjectType.File)
            {
                PackFile file = new PackFile(new FileInfo(name));
                file.ReadHeader(stream);
                file.IsLast = isLast;
                return file;
            }
            else
            {
                PackDir dir = new PackDir(new DirectoryInfo(name));
                dir.IsLast = isLast;
                return dir;
            }
        }
    }
}

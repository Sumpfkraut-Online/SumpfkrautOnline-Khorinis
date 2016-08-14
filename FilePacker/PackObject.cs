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
        
        public virtual void WriteHeader(BinaryWriter header)
        {
            if (this.IsLast)
            {
                header.Write((byte)((int)this.POType | 2));
            }
            else
            {
                header.Write((byte)this.POType);
            }
            
            header.Write(this.Name);
        }
        
        public static PackObject ReadNew(BinaryReader br, string path)
        {
            int type = br.ReadByte();
            string name = Path.Combine(path, br.ReadString());

            if ((type & ~2) == (int)PackObjectType.File)
            {
                PackFile file = new PackFile(new FileInfo(name));
                file.ReadHeader(br);
                file.IsLast = (type & 2) > 0;
                return file;
            }
            else
            {
                PackDir dir = new PackDir(new DirectoryInfo(name));
                dir.IsLast = (type & 2) > 0;
                return dir;
            }
        }
    }
}

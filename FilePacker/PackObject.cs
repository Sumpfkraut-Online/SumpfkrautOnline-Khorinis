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

        public PackObject(string name)
        {
            this.Name = name;
        }

        public abstract void Write(BinaryWriter header, Stream pack, string folder);
    }
}

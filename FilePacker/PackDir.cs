using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePacker
{
    class PackDir : PackObject
    {
        public bool IsEmpty = false;
        public PackDir(string name) : base(name)
        {
        }

        public override void Write(BinaryWriter header, Stream pack, string folder)
        {
            int type = IsEmpty ? PackType.Directory_Empty : PackType.Directory;
            if (this.IsLast) type |= 4;

            header.Write((byte)type);
            header.Write(Name);
        }
    }
}

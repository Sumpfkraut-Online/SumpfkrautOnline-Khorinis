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
        public override PackObjectType POType { get { return PackObjectType.Directory; } }

        new public DirectoryInfo Info { get { return (DirectoryInfo)this.info; } }

        public PackDir(DirectoryInfo info) : base(info)
        {
        }
    }
}

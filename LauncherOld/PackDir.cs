using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Launcher2
{
    class PackDir
    {
        public bool lastEntry;
        public int parentNum;
        public int VDFSNum; //so we don't have to sort them again
        public string name;
        public ushort vdfsOffset;

        public PackDir(BinaryReader br)
        {
            parentNum = br.ReadUInt16();
            lastEntry = (parentNum & 0x8000) == 0x8000;
            parentNum = (parentNum & ~0x8000) - 1; //attention: no parent directory == -1

            VDFSNum = br.ReadUInt16();

            name = br.ReadString();

            vdfsOffset = br.ReadUInt16();
        }

        public bool IsVDFS()
        {
            return VDFSNum < (ushort)0xFFFF;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace GUC.Client.WorldObjects
{
    abstract class AbstractInstance
    {
        internal abstract void Read(BinaryReader br);
        internal abstract void Write(BinaryWriter bw);
        public ushort ID;
    }
}

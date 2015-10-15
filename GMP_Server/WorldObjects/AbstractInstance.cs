using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Server.WorldObjects
{
    public abstract class AbstractInstance
    {
        public ushort ID { get; internal set; }
        public string instanceName { get; internal set; }

        internal abstract void Write(BinaryWriter bw);

        private AbstractInstance()
        {
        }

        public AbstractInstance(string instanceName)
            : this(0, instanceName)
        {
        }

        public AbstractInstance(ushort ID, string instanceName)
        {
            this.ID = ID;
            this.instanceName = instanceName.ToUpper();
        }
    }
}

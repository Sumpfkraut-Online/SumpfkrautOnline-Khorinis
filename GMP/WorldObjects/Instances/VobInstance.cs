using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using System.Security.Cryptography;
using GUC.Client.WorldObjects.Collections;

namespace GUC.Client.WorldObjects.Instances
{
    public class VobInstance : IVobObj<ushort>
    {
        public readonly static VobType sVobType = VobType.Vob;
        public readonly static InstanceCollection Instances = new InstanceCollection();

        public ushort ID { get; internal set; }

        public VobType VobType { get; protected set; }

        #region Properties

        protected string visual = "";
        public virtual string Visual
        {
            get { return visual; }
            set { visual = value.Trim().ToUpper(); }
        }

        protected bool cddyn = true;
        public virtual bool CDDyn
        {
            get { return cddyn; }
            set { cddyn = value; }
        }

        protected bool cdstatic = true;
        public virtual bool CDStatic
        {
            get { return cdstatic; }
            set { cdstatic = value; }
        }

        #endregion
        
        public VobInstance(ushort id)
        {
            this.ID = id;
            this.VobType = sVobType;
        }

        internal virtual void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadUShort();
            this.Visual = stream.ReadString();
            this.CDDyn = stream.ReadBit();
            this.CDStatic = stream.ReadBit();
            
            // ... 
        }
    }
}

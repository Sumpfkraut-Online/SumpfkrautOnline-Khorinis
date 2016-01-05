using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Instances
{
    public class MobContainerInstance : MobLockableInstance
    {
        new public readonly static Enumeration.VobTypes sVobType = Enumeration.VobTypes.MobContainer;
        new public readonly static Collections.InstanceDictionary Instances = Network.Server.sInstances.GetDict(sVobType);

        public MobContainerInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobContainerInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }

        public new static Action<MobContainerInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            if (MobContainerInstance.OnWriteProperties != null)
            {
                MobContainerInstance.OnWriteProperties(this, stream);
            }
        }
    }
}

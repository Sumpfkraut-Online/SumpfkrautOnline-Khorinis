using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobContainerInstance : MobLockableInstance
    {
        new public readonly static InstanceManager<MobContainerInstance> Table = new InstanceManager<MobContainerInstance>();

        public MobContainerInstance(string instanceName, object scriptObject)
            : base(0, instanceName, scriptObject)
        {
        }

        public MobContainerInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
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

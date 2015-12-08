using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobDoorInstance : MobLockableInstance
    {
        new public readonly static InstanceManager<MobDoorInstance> Table = new InstanceManager<MobDoorInstance>();

        public MobDoorInstance(string instanceName, object scriptObject)
            : base(0, instanceName, scriptObject)
        {
        }

        public MobDoorInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }

        public new static Action<MobDoorInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            if (MobDoorInstance.OnWriteProperties != null)
            {
                MobDoorInstance.OnWriteProperties(this, stream);
            }
        }
    }
}

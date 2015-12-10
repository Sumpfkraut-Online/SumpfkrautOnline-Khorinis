using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Instances
{
    public class MobDoorInstance : MobLockableInstance
    {
        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.MobDoor;
        new public readonly static Collections.InstanceDictionary Instances = Network.Server.sInstances.GetDict(sVobType);

        public MobDoorInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobDoorInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
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

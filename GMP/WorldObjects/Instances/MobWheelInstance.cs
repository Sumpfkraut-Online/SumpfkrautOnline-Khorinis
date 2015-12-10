using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Client.WorldObjects.Instances
{
    public class MobWheelInstance : MobInterInstance
    {
        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.MobWheel;
        new public readonly static Collections.InstanceDictionary Instances = Network.Server.sInstances.GetDict(sVobType);

        public MobWheelInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobWheelInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }

        public new static Action<MobWheelInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            if (MobWheelInstance.OnWriteProperties != null)
            {
                MobWheelInstance.OnWriteProperties(this, stream);
            }
        }
    }
}

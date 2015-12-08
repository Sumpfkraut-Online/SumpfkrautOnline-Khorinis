using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobWheelInstance : MobInterInstance
    {
        new public readonly static InstanceManager<MobWheelInstance> Table = new InstanceManager<MobWheelInstance>();

        public MobWheelInstance(string instanceName, object scriptObject)
            : base(0, instanceName, scriptObject)
        {
        }

        public MobWheelInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
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

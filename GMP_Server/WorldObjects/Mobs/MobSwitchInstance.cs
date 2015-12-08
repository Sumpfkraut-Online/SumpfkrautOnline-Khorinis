using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobSwitchInstance : MobInterInstance
    {
        new public readonly static InstanceManager<MobSwitchInstance> Table = new InstanceManager<MobSwitchInstance>();

        public MobSwitchInstance(string instanceName, object scriptObject)
            : base(0, instanceName, scriptObject)
        {
        }

        public MobSwitchInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }

        public new static Action<MobSwitchInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            if (MobSwitchInstance.OnWriteProperties != null)
            {
                MobSwitchInstance.OnWriteProperties(this, stream);
            }
        }
    }
}

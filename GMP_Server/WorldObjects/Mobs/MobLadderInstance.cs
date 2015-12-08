using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobLadderInstance : MobInterInstance
    {
        new public readonly static InstanceManager<MobLadderInstance> Table = new InstanceManager<MobLadderInstance>();

        public MobLadderInstance(string instanceName, object scriptObject)
            : base(0, instanceName, scriptObject)
        {
        }

        public MobLadderInstance(ushort ID, string instanceName, object scriptObject) 
            : base(ID, instanceName, scriptObject)
        {
        }

        public new static Action<MobLadderInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            if (MobLadderInstance.OnWriteProperties != null)
            {
                MobLadderInstance.OnWriteProperties(this, stream);
            }
        }
    }
}

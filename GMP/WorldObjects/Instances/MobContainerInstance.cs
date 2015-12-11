using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Client.WorldObjects.Instances
{
    public class MobContainerInstance : MobLockableInstance
    {
        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.MobContainer;

        public MobContainerInstance()
        {
            this.VobType = sVobType;
        }
        
        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            //...
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            //...
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobInterInstance : MobInstance
    {
        public static readonly Collections.InstanceDictionary MobInterInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.MobInter);

        #region Properties

        public string OnTriggerClientFunc = "";

        #endregion

        internal MobInterInstance()
        {
            this.VobType = VobTypes.MobInter;
        }
        
        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.OnTriggerClientFunc = stream.ReadString();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(OnTriggerClientFunc);
        }
    }
}

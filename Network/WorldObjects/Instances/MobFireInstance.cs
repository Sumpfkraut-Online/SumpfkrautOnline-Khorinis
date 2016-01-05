using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public partial class MobFireInstance : MobInterInstance
    {
        public static readonly Collections.InstanceDictionary MobFireInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.MobFire);

        #region Properties

        public string FireVobTree = "";

        #endregion

        internal MobFireInstance()
        {
            this.VobType = VobTypes.MobFire;
        }
        
        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FireVobTree = stream.ReadString();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FireVobTree);
        }
    }
}

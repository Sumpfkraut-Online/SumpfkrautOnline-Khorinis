using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Client.WorldObjects.Instances
{
    public class MobFireInstance : MobInterInstance
    {
        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.MobFire;

        #region Properties

        public string FireVobTree = "";

        #endregion


        public MobFireInstance(ushort ID) 
            : base(ID)
        {
            this.VobType = sVobType;
        }
        
        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FireVobTree = stream.ReadString();

            //...
        }
    }
}

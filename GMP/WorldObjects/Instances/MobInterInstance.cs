using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Client.WorldObjects.Instances
{
    public class MobInterInstance : MobInstance
    {
        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.MobInter;

        #region Properties
        
        public string OnTriggerClientFunc = "";

        #endregion

        public MobInterInstance(ushort ID) 
            : base(ID)
        {
            this.VobType = sVobType;
        }
        
        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.OnTriggerClientFunc = stream.ReadString();

            //...
        }
    }
}

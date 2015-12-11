using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Client.WorldObjects.Instances
{
    public class MobInstance : VobInstance
    {
        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.Mob;

        #region Properties

        public string FocusName = "";

        #endregion

        public MobInstance()
        {
            this.VobType = sVobType;
        }
        
        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FocusName = stream.ReadString();

            //...
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FocusName);

            //...
        }
    }
}

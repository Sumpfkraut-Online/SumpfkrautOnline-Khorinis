using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobInstance : VobInstance
    {
        public static readonly Collections.InstanceDictionary MobInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.Mob);

        #region Properties

        public string FocusName = "";

        #endregion

        internal MobInstance()
        {
            this.VobType = VobTypes.Mob;
        }
        
        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FocusName = stream.ReadString();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FocusName);
        }
    }
}

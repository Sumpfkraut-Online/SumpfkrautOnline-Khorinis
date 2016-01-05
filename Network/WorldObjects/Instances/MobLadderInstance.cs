using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public partial class MobLadderInstance : MobInterInstance
    {
        public static readonly Collections.InstanceDictionary MobLadderInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.MobLadder);

        internal MobLadderInstance()
        {
            this.VobType = VobTypes.MobLadder;
        }
    }
}

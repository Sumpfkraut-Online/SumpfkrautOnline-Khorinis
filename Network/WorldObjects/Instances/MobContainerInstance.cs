using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public partial class MobContainerInstance : MobLockableInstance
    {
        public static readonly Collections.InstanceDictionary MobContainerInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.MobContainer);

        internal MobContainerInstance()
        {
            this.VobType = VobTypes.MobContainer;
        }
    }
}

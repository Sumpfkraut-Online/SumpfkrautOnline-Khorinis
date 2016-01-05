using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public partial class MobSwitchInstance : MobInterInstance
    {
        public static readonly Collections.InstanceDictionary MobSwitchInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.MobSwitch);

        internal MobSwitchInstance()
        {
            this.VobType = VobTypes.MobSwitch;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public partial class MobWheelInstance : MobInterInstance
    {
        public static readonly Collections.InstanceDictionary MobWheelInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.MobWheel);

        internal MobWheelInstance()
        {
            this.VobType = VobTypes.MobWheel;
        }
    }
}

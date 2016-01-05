using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public partial class MobDoorInstance : MobLockableInstance
    {
        public static readonly Collections.InstanceDictionary MobDoorInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.MobDoor);

        internal MobDoorInstance()
        {
            this.VobType = VobTypes.MobDoor;
        }
    }
}

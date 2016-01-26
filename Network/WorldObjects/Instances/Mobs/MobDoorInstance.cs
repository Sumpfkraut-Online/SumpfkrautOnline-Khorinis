using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobDoorInstance : MobLockableInstance
    {
        public partial interface IScriptMobDoorInstance : IScriptMobLockableInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobDoor;
        public static readonly InstanceDictionary MobDoorInstances = VobInstance.AllInstances.GetDict(sVobType);

        new public IScriptMobDoorInstance ScriptObj { get; protected set; }
    }
}

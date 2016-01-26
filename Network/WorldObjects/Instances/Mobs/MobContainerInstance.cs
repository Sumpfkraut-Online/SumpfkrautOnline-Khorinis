using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobContainerInstance : MobLockableInstance
    {
        public partial interface IScriptMobContainerInstance : IScriptMobLockableInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobContainer;
        public static readonly InstanceDictionary MobContainerInstances = VobInstance.AllInstances.GetDict(sVobType);

        new public IScriptMobContainerInstance ScriptObj { get; protected set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobSwitchInstance : MobInterInstance
    {
        public partial interface IScriptMobSwitchInstance : IScriptMobInterInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobSwitch;
        public static readonly InstanceDictionary MobSwitchInstances = VobInstance.AllInstances.GetDict(sVobType);

        new public IScriptMobSwitchInstance ScriptObj { get; protected set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobWheelInstance : MobInterInstance
    {
        public partial interface IScriptMobWheelInstance : IScriptMobInterInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobWheel;
        public static readonly InstanceDictionary MobWheelInstances = VobInstance.AllInstances.GetDict(sVobType);

        new public IScriptMobWheelInstance ScriptObj { get; protected set; }
    }
}

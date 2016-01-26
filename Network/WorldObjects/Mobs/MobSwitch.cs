using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class MobSwitch : MobInter
    {
        public partial interface IScriptMobSwitch : IScriptMobInter
        {
        }

        new public const VobTypes sVobType = MobSwitchInstance.sVobType;
        public static readonly VobDictionary MobSwitchs = Vob.AllVobs.GetDict(sVobType);

        new public IScriptMobSwitch ScriptObj { get; protected set; }
        new public MobSwitchInstance Instance { get; protected set; }

        public MobSwitch(MobSwitchInstance instance, IScriptMobSwitch scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

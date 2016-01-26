using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class MobWheel : MobInter
    {
        public partial interface IScriptMobWheel : IScriptMobInter
        {
        }

        new public const VobTypes sVobType = MobWheelInstance.sVobType;
        public static readonly VobDictionary MobWheels = Vob.AllVobs.GetDict(sVobType);

        new public IScriptMobWheel ScriptObj { get; protected set; }
        new public MobWheelInstance Instance { get; protected set; }

        public MobWheel(MobWheelInstance instance, IScriptMobWheel scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

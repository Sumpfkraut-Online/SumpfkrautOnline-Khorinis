using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class MobInter : Mob
    {
        public partial interface IScriptMobInter : IScriptMob
        {
        }

        new public const VobTypes sVobType = MobInterInstance.sVobType;
        public static readonly VobDictionary MobInters = Vob.AllVobs.GetDict(sVobType);

        new public IScriptMobInter ScriptObj { get; protected set; }
        new public MobInterInstance Instance { get; protected set; }
        public string OnTriggerClientFunc { get { return Instance.OnTriggerClientFunc; } }

        public MobInter(MobInterInstance instance, IScriptMobInter scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

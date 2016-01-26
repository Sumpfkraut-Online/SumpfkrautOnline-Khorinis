using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.WorldObjects.Collections;
using GUC.Enumeration;

namespace GUC.WorldObjects
{
    public partial class Mob : Vob
    {
        public partial interface IScriptMob : IScriptVob
        {
        }

        new public const VobTypes sVobType = MobInstance.sVobType;
        public static readonly VobDictionary Mobs = Vob.AllVobs.GetDict(sVobType);

        new public IScriptMob ScriptObj { get; protected set; }
        new public MobInstance Instance { get; protected set; }
        public string FocusName { get { return Instance.FocusName; } }

        public Mob(MobInstance instance, IScriptMob scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

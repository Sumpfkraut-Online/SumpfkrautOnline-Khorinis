using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class MobFire : MobInter
    {
        public partial interface IScriptMobFire : IScriptMobInter
        {
        }

        new public const VobTypes sVobType = MobFireInstance.sVobType;
        public static readonly VobDictionary MobFires = Vob.AllVobs.GetDict(sVobType);

        new public IScriptMobFire ScriptObj { get; protected set; }
        new public MobFireInstance Instance { get; protected set; }
        public string FireVobTree { get { return Instance.FireVobTree; } }

        public MobFire(MobFireInstance instance, IScriptMobFire scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

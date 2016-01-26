using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class MobLadder : MobInter
    {
        public partial interface IScriptMobLadder : IScriptMobInter
        {
        }

        new public const VobTypes sVobType = MobLadderInstance.sVobType;
        public static readonly VobDictionary MobLadders = Vob.AllVobs.GetDict(sVobType);

        new public IScriptMobLadder ScriptObj { get; protected set; }
        new public MobLadderInstance Instance { get; protected set; }

        public MobLadder(MobLadderInstance instance, IScriptMobLadder scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

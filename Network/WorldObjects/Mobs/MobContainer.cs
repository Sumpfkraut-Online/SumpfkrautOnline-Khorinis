using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class MobContainer : MobLockable
    {
        public partial interface IScriptMobContainer : IScriptMobLockable
        {
        }

        new public const VobTypes sVobType = MobContainerInstance.sVobType;
        public static readonly VobDictionary MobContainers = Vob.AllVobs.GetDict(sVobType);

        new public IScriptMobContainer ScriptObj { get; protected set; }
        new public MobContainerInstance Instance { get; protected set; }

        public MobContainer(MobContainerInstance instance, IScriptMobContainer scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

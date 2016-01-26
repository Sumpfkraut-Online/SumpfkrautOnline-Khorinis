using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public abstract partial class MobLockable : MobInter
    {
        public partial interface IScriptMobLockable : IScriptMobInter
        {
        }

        new public MobLockableInstance Instance { get; protected set; }
        new public IScriptMobLockable ScriptObj { get; protected set; }

        public string OnTryOpenClientFunc { get { return Instance.OnTryOpenClientFunc; } }

        public MobLockable(MobLockableInstance instance, IScriptMobLockable scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

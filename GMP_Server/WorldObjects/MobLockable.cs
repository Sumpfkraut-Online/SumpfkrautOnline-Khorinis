using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    public abstract class MobLockable : MobInter
    {
        new public MobLockableInstance Instance { get; protected set; }
        public string OnTryOpenClientFunc { get { return Instance.OnTryOpenClientFunc; } }

        public MobLockable(MobLockableInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}

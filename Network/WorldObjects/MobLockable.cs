using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public abstract partial class MobLockable : MobInter
    {
        new public MobLockableInstance Instance { get; protected set; }
        public string OnTryOpenClientFunc { get { return Instance.OnTryOpenClientFunc; } }

        internal MobLockable()
        {
        }
    }
}

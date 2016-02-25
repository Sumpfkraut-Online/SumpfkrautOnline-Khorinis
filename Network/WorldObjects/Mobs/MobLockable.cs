using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public abstract partial class MobLockable : MobInter
    {
        #region ScriptObject

        public partial interface IScriptMobLockable : IScriptMobInter
        {
        }

        new public IScriptMobLockable ScriptObject
        {
            get { return (IScriptMobLockable)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobLockableInstance Instance { get { return (MobLockableInstance)base.Instance; } }

        public string OnTryOpenClientFunc { get { return Instance.OnTryOpenClientFunc; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

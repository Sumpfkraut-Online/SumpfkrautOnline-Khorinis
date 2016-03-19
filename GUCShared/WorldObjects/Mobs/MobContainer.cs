using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobContainer : MobLockable //, IContainer
    {
        public override VobTypes VobType { get { return VobTypes.MobContainer; } }

        #region ScriptObject

        public partial interface IScriptMobContainer : IScriptMobLockable
        {
        }

        new public IScriptMobContainer ScriptObject
        {
            get { return (IScriptMobContainer)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobContainerInstance Instance { get { return (MobContainerInstance)base.Instance; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobContainerInstance : MobLockableInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobContainer; } }

        #region ScriptObject

        public partial interface IScriptMobContainerInstance : IScriptMobLockableInstance
        {
        }

        public new IScriptMobContainerInstance ScriptObject
        {
            get { return (IScriptMobContainerInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}

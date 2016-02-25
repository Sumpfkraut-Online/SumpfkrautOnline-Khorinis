using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobDoorInstance : MobLockableInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobDoor; } }

        #region ScriptObject

        public partial interface IScriptMobDoorInstance : IScriptMobLockableInstance
        {
        }

        public new IScriptMobDoorInstance ScriptObject
        {
            get { return (IScriptMobDoorInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}

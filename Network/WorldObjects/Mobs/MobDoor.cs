using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobDoor : MobLockable
    {
        public override VobTypes VobType { get { return VobTypes.MobDoor; } }

        #region ScriptObject

        public partial interface IScriptMobDoor : IScriptMobLockable
        {
        }

        new public IScriptMobDoor ScriptObject
        {
            get { return (IScriptMobDoor)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobDoorInstance Instance { get { return (MobDoorInstance)base.Instance; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

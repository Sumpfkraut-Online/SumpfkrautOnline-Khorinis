using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobSwitchInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobSwitch; } }

        #region ScriptObject

        public partial interface IScriptMobSwitchInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobSwitchInstance ScriptObject
        {
            get { return (IScriptMobSwitchInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}

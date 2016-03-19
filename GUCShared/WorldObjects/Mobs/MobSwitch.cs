using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobSwitch : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobSwitch; } }

        #region ScriptObject

        public partial interface IScriptMobSwitch : IScriptMobInter
        {
        }

        new public IScriptMobSwitch ScriptObject
        {
            get { return (IScriptMobSwitch)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobSwitchInstance Instance { get { return (MobSwitchInstance)base.Instance; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobWheelInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobWheel; } }

        #region ScriptObject

        public partial interface IScriptMobWheelInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobWheelInstance ScriptObject
        {
            get { return (IScriptMobWheelInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}

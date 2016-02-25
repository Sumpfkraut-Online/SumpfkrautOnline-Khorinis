using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobWheel : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobWheel; } }

        #region ScriptObject

        public partial interface IScriptMobWheel : IScriptMobInter
        {
        }

        new public IScriptMobWheel ScriptObject
        {
            get { return (IScriptMobWheel)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobWheelInstance Instance { get { return (MobWheelInstance)base.Instance; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

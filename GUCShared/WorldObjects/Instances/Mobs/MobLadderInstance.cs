using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobLadderInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobLadder; } }

        #region ScriptObject

        public partial interface IScriptMobLadderInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobLadderInstance ScriptObject
        {
            get { return (IScriptMobLadderInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties
        
        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobLadder : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobLadder; } }

        #region ScriptObject

        public partial interface IScriptMobLadder : IScriptMobInter
        {
        }

        new public IScriptMobLadder ScriptObject
        {
            get { return (IScriptMobLadder)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobLadderInstance Instance { get { return (MobLadderInstance)base.Instance; } }

        #endregion
        
        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class Mob : Vob
    {
        public override VobTypes VobType { get { return VobTypes.Mob; } }

        #region ScriptObject

        public partial interface IScriptMob : IScriptVob
        {
        }

        new public IScriptMob ScriptObject
        {
            get { return (IScriptMob)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobInstance Instance { get { return (MobInstance)base.Instance; } }
        
        public string FocusName { get { return Instance.FocusName; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

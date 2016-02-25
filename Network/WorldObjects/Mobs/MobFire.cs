using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobFire : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobFire; } }

        #region ScriptObject

        public partial interface IScriptMobFire : IScriptMobInter
        {
        }

        new public IScriptMobFire ScriptObject
        {
            get { return (IScriptMobFire)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobFireInstance Instance { get { return (MobFireInstance)base.Instance; } }

        public string FireVobTree { get { return Instance.FireVobTree; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

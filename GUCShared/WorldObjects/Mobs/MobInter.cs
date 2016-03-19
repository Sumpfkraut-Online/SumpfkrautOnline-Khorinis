using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobInter : Mob
    {
        public override VobTypes VobType { get { return VobTypes.MobInter; } }

        #region ScriptObject

        public partial interface IScriptMobInter : IScriptMob
        {
        }

        new public IScriptMobInter ScriptObject
        {
            get { return (IScriptMobInter)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobInterInstance Instance { get { return (MobInterInstance)base.Instance; } }

        public string OnTriggerClientFunc { get { return Instance.OnTriggerClientFunc; } }

        //NPC user;
        //public NPC User { get { return this.user; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

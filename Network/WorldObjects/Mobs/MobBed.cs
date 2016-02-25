using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobBed : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobBed; } }

        #region ScriptObject

        public partial interface IScriptMobBed : IScriptMobInter
        {
        }

        new public IScriptMobBed ScriptObject
        {
            get { return (IScriptMobBed)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public MobBedInstance Instance { get { return (MobBedInstance)base.Instance; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

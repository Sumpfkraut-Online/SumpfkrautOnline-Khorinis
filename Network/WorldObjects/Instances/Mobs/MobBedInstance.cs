using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobBedInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobBed; } }

        #region ScriptObject

        public partial interface IScriptMobBedInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobBedInstance ScriptObject
        {
            get { return (IScriptMobBedInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}
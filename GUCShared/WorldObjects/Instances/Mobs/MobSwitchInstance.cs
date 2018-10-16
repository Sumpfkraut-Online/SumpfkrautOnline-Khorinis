using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobSwitchInstance : MobInterInstance
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobSwitch; } }

        #region ScriptObject

        public partial interface IScriptMobSwitchInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobSwitchInstance ScriptObject { get { return (IScriptMobSwitchInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public MobSwitchInstance(IScriptMobSwitchInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}

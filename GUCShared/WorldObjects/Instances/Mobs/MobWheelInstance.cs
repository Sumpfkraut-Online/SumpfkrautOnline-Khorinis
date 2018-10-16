using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobWheelInstance : MobInterInstance
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobWheel; } }

        #region ScriptObject

        public partial interface IScriptMobWheelInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobWheelInstance ScriptObject { get { return (IScriptMobWheelInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public MobWheelInstance(IScriptMobWheelInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}

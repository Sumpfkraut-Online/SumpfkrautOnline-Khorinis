using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobContainerInstance : MobLockableInstance
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobContainer; } }

        #region ScriptObject

        public partial interface IScriptMobContainerInstance : IScriptMobLockableInstance
        {
        }

        public new IScriptMobContainerInstance ScriptObject { get { return (IScriptMobContainerInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public MobContainerInstance(IScriptMobContainerInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}

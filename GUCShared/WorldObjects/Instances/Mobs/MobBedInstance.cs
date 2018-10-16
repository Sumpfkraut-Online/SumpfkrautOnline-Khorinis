using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobBedInstance : MobInterInstance
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobBed; } }

        #region ScriptObject

        public partial interface IScriptMobBedInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobBedInstance ScriptObject { get { return (IScriptMobBedInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public MobBedInstance(IScriptMobBedInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        #endregion
    }
}
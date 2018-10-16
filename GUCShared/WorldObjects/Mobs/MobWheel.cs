using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobWheel : MobInter
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobWheel; } }

        #region ScriptObject

        public partial interface IScriptMobWheel : IScriptMobInter
        {
        }
        
        /// <summary>  The ScriptObject of this object. </summary>
        public new IScriptMobWheel ScriptObject { get { return (IScriptMobWheel)base.ScriptObject; } }

        #endregion


        #region Constructors

        public MobWheel(Model.IScriptModel scriptModel, IScriptMobWheel scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobWheelInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobWheelInstance Instance
        {
            get { return (MobWheelInstance)base.Instance; }
            set { SetInstance(value); }
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

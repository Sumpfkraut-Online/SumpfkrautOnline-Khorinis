using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobLadder : MobInter
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobLadder; } }

        #region ScriptObject

        public partial interface IScriptMobLadder : IScriptMobInter
        {
        }
        
        /// <summary>  The ScriptObject of this object. </summary>
        public new IScriptMobLadder ScriptObject { get { return (IScriptMobLadder)base.ScriptObject; } }

        #endregion


        #region Constructors

        public MobLadder(Model.IScriptModel scriptModel, IScriptMobLadder scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobLadderInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobLadderInstance Instance
        {
            get { return (MobLadderInstance)base.Instance; }
            set { SetInstance(value); }
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

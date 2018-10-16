using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobFire : MobInter
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobFire; } }

        #region ScriptObject

        public partial interface IScriptMobFire : IScriptMobInter
        {
        }
        
        /// <summary>  The ScriptObject of this object. </summary>
        public new IScriptMobFire ScriptObject { get { return (IScriptMobFire)base.ScriptObject; } }

        #endregion
        
        #region Constructors

        public MobFire(Model.IScriptModel scriptModel, IScriptMobFire scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobFireInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobFireInstance Instance
        {
            get { return (MobFireInstance)base.Instance; }
            set { SetInstance(value); }
        }

        public string FireVobTree { get { return Instance.FireVobTree; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

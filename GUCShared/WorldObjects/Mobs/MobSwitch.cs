using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobSwitch : MobInter
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobSwitch; } }

        #region ScriptObject

        public partial interface IScriptMobSwitch : IScriptMobInter
        {
        }
        
        /// <summary>  The ScriptObject of this object. </summary>
        public new IScriptMobSwitch ScriptObject { get { return (IScriptMobSwitch)base.ScriptObject; } }

        #endregion


        #region Constructors

        public MobSwitch(Model.IScriptModel scriptModel, IScriptMobSwitch scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobSwitchInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobSwitchInstance Instance
        {
            get { return (MobSwitchInstance)base.Instance; }
            set { SetInstance(value); }
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public partial class Mob : Vob
    {
        public override VobTypes VobType { get { return VobTypes.Mob; } }
        
        #region ScriptObject

        public partial interface IScriptMob : IScriptVob
        {
        }
        
        /// <summary>  The ScriptObject of this object. </summary>
        public new IScriptMob ScriptObject {  get { return (IScriptMob)base.ScriptObject; } }

        #endregion

        #region Constructors

        public Mob(Model.IScriptModel scriptModel, IScriptMob scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobInstance Instance
        {
            get { return (MobInstance)base.Instance; }
            set { SetInstance(value); }
        }

        /// <summary> The name set by its Instance players will see when the focus this object. </summary>
        public string FocusName { get { return Instance.FocusName; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Definitions;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class GUCMobInst : GUCVobInst
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.Mob; } }
        
        #region ScriptObject

        public partial interface IScriptMob : IScriptVob
        {
        }
        
        /// <summary>  The ScriptObject of this object. </summary>
        public new IScriptMob ScriptObject {  get { return (IScriptMob)base.ScriptObject; } }

        #endregion

        #region Constructors

        public GUCMobInst(Model.IScriptModel scriptModel, IScriptMob scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(GUCMobDef); } }
        /// <summary> The Instance of this object. </summary>
        new public GUCMobDef Instance
        {
            get { return (GUCMobDef)base.Instance; }
            set { SetInstance(value); }
        }

        /// <summary> The name set by its Instance players will see when the focus this object. </summary>
        public string FocusName { get { return Instance.FocusName; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Definitions;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Instances
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

        public GUCMobInst(GUCModelInst.IScriptModelInst scriptModel, IScriptMob scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type DefinitionType { get { return typeof(GUCMobDef); } }
        /// <summary> The Definition of this object. </summary>
        new public GUCMobDef Definition
        {
            get { return (GUCMobDef)base.Definition; }
            set { SetDefinition(value); }
        }

        /// <summary> The name set by its Instance players will see when the focus this object. </summary>
        public string FocusName { get { return Definition.FocusName; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

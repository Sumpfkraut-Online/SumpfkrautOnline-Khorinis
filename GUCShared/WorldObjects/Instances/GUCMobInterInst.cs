using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Definitions;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class GUCMobInterInst : GUCMobInst
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobInter; } }

        #region ScriptObject

        public partial interface IScriptMobInter : IScriptMob
        {
        }
        
        public new IScriptMobInter ScriptObject { get { return (IScriptMobInter)base.ScriptObject; } }

        #endregion
        
        #region Constructors

        public GUCMobInterInst(GUCModelInst.IScriptModelInst scriptModel, IScriptMobInter scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type DefinitionType { get { return typeof(GUCMobInterDef); } }
        /// <summary> The Definition of this object. </summary>
        new public GUCMobInterDef Definition
        {
            get { return (GUCMobInterDef)base.Definition; }
            set { SetDefinition(value); }
        }

        public string OnTriggerClientFunc { get { return Definition.OnTriggerClientFunc; } }

        //NPC user;
        //public NPC User { get { return this.user; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

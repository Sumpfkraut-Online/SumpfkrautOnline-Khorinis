using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class MobInter : Mob
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobInter; } }

        #region ScriptObject

        public partial interface IScriptMobInter : IScriptMob
        {
        }
        
        public new IScriptMobInter ScriptObject { get { return (IScriptMobInter)base.ScriptObject; } }

        #endregion
        
        #region Constructors

        public MobInter(Model.IScriptModel scriptModel, IScriptMobInter scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(GUCMobInterDef); } }
        /// <summary> The Instance of this object. </summary>
        new public GUCMobInterDef Instance
        {
            get { return (GUCMobInterDef)base.Instance; }
            set { SetInstance(value); }
        }

        public string OnTriggerClientFunc { get { return Instance.OnTriggerClientFunc; } }

        //NPC user;
        //public NPC User { get { return this.user; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobBed : MobInter
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobBed; } }

        #region ScriptObject

        public partial interface IScriptMobBed : IScriptMobInter
        {
        }
        
        public new IScriptMobBed ScriptObject { get { return (IScriptMobBed)base.ScriptObject; } }

        #endregion
        
        #region Constructors

        public MobBed(Model.IScriptModel scriptModel, IScriptMobBed scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobBedInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobBedInstance Instance
        {
            get { return (MobBedInstance)base.Instance; }
            set { SetInstance(value); }
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobContainer : MobLockable //, IContainer
    {
        public override VobTypes VobType { get { return VobTypes.MobContainer; } }

        #region ScriptObject

        public partial interface IScriptMobContainer : IScriptMobLockable
        {
        }
        
        /// <summary> The ScriptObject of this object. </summary>
        public new IScriptMobContainer ScriptObject { get { return (IScriptMobContainer)base.ScriptObject; } }

        #endregion
        
        #region Constructors

        public MobContainer(Model.IScriptModel scriptModel, IScriptMobContainer scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobContainerInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobContainerInstance Instance
        {
            get { return (MobContainerInstance)base.Instance; }
            set { SetInstance(value); }
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

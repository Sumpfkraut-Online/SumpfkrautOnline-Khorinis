using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public abstract partial class MobLockable : MobInter
    {
        #region ScriptObject

        public partial interface IScriptMobLockable : IScriptMobInter
        {
        }

        new public IScriptMobLockable ScriptObject { get { return (IScriptMobLockable)base.ScriptObject; } }

        #endregion

        #region Constructors

        public MobLockable(Model.IScriptModel scriptModel, IScriptMobLockable scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobLockableInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobLockableInstance Instance
        {
            get { return (MobLockableInstance)base.Instance; }
            set { SetInstance(value); }
        }

        public string OnTryOpenClientFunc { get { return Instance.OnTryOpenClientFunc; } }

        #endregion

        #region Read & Write

        #endregion
    }
}

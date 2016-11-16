using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobDoor : MobLockable
    {
        public override VobTypes VobType { get { return VobTypes.MobDoor; } }

        #region ScriptObject

        public partial interface IScriptMobDoor : IScriptMobLockable
        {
        }
        
        /// <summary>  The ScriptObject of this object. </summary>
        public new IScriptMobDoor ScriptObject { get { return (IScriptMobDoor)base.ScriptObject; } }

        #endregion
        
        #region Constructors

        public MobDoor(Model.IScriptModel scriptModel, IScriptMobDoor scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(MobDoorInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public MobDoorInstance Instance
        {
            get { return (MobDoorInstance)base.Instance; }
            set { SetInstance(value); }
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

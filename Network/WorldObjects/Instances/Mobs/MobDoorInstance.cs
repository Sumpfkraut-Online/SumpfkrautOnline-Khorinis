using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobDoorInstance : MobLockableInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobDoor; } }

        #region ScriptObject

        public partial interface IScriptMobDoorInstance : IScriptMobLockableInstance
        {
        }

        public new IScriptMobDoorInstance ScriptObject
        {
            get { return (IScriptMobDoorInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        protected MobDoorInstance(IScriptMobDoorInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobDoorInstance(IScriptMobDoorInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class MobDoor : MobLockable
    {
        public override VobTypes VobType { get { return VobTypes.MobDoor; } }

        #region ScriptObject

        public partial interface IScriptMobDoor : IScriptMobLockable
        {
        }

        new public IScriptMobDoor ScriptObject { get { return (IScriptMobDoor)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobDoorInstance Instance { get { return (MobDoorInstance)base.Instance; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobDoor(IScriptMobDoor scriptObject, MobDoorInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobDoor(IScriptMobDoor scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

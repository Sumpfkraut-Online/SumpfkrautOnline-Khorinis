using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Network;

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

        #region Properties

        new public MobLockableInstance Instance { get { return (MobLockableInstance)base.Instance; } }

        public string OnTryOpenClientFunc { get { return Instance.OnTryOpenClientFunc; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobLockable(IScriptMobLockable scriptObject, MobLockableInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobLockable(IScriptMobLockable scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

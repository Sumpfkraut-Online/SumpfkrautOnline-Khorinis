using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobContainer : MobLockable //, IContainer
    {
        public override VobTypes VobType { get { return VobTypes.MobContainer; } }

        #region ScriptObject

        public partial interface IScriptMobContainer : IScriptMobLockable
        {
        }

        new public IScriptMobContainer ScriptObject { get { return (IScriptMobContainer)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobContainerInstance Instance { get { return (MobContainerInstance)base.Instance; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobContainer(IScriptMobContainer scriptObject, MobContainerInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobContainer(IScriptMobContainer scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

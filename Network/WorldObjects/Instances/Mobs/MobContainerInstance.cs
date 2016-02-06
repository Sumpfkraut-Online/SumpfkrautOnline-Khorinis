using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobContainerInstance : MobLockableInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobContainer; } }

        #region ScriptObject

        public partial interface IScriptMobContainerInstance : IScriptMobLockableInstance
        {
        }

        public new IScriptMobContainerInstance ScriptObject
        {
            get { return (IScriptMobContainerInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        protected MobContainerInstance(IScriptMobContainerInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobContainerInstance(IScriptMobContainerInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}

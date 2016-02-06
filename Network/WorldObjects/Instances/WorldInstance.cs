using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class WorldInstance : BaseInstance
    {
        #region ScriptObject

        public partial interface IScriptWorldInstance : IScriptBaseInstance
        {
        }

        public new IScriptWorldInstance ScriptObject
        {
            get { return (IScriptWorldInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        protected WorldInstance(IScriptWorldInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public WorldInstance(IScriptWorldInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
        }

        protected override void ReadProperties(PacketReader stream)
        {
        }

        #endregion
    }
}

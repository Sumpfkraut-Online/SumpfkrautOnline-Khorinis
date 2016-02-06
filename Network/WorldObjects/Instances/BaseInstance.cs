using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseInstance : WorldObject
    {
        public abstract VobTypes VobType { get; }

        #region ScriptObject

        public partial interface IScriptBaseInstance : IScriptWorldObject
        {
        }

        public new IScriptBaseInstance ScriptObject
        {
            get { return (IScriptBaseInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        internal int DictID;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or searches a new one when needed.
        /// </summary>
        protected BaseInstance(IScriptWorldObject scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        protected BaseInstance(IScriptWorldObject scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }
        #endregion
    }
}

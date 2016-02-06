using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseVobInstance : BaseInstance
    {
        public abstract VobTypes VobType { get; }

        #region ScriptObject

        public partial interface IScriptBaseVobInstance : IScriptBaseInstance
        {
        }

        public new IScriptBaseVobInstance ScriptObject
        {
            get { return (IScriptBaseVobInstance)base.ScriptObject; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        protected BaseVobInstance(IScriptBaseVobInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        protected BaseVobInstance(IScriptBaseVobInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }
        #endregion
    }
}

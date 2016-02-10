using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects.Instances;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class Vob : BaseVob
    {
        public override VobTypes VobType { get { return VobTypes.Vob; } }

        #region ScriptObject

        public partial interface IScriptVob : IScriptBaseVob
        {
        }

        public new IScriptVob ScriptObject
        {
            get { return (IScriptVob)base.ScriptObject; }
        }

        #endregion

        #region Properties

        new public VobInstance Instance { get { return (VobInstance)base.Instance; } }

        public string Visual { get { return Instance.Visual; } }
        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public Vob(IScriptVob scriptObject, VobInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public Vob(IScriptVob scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion
    }
}

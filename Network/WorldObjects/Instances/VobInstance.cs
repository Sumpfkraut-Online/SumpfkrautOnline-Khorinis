using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class VobInstance : BaseInstance
    {
        public override VobTypes VobType { get { return VobTypes.Vob; } }

        #region ScriptObject

        public partial interface IScriptVobInstance : IScriptBaseInstance
        {
        }

        public new IScriptVobInstance ScriptObject
        {
            get { return (IScriptVobInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        protected string visual = "";
        public virtual string Visual
        {
            get { return visual; }
            set { visual = value.ToUpper(); }
        }

        protected bool cddyn = true;
        public virtual bool CDDyn
        {
            get { return cddyn; }
            set { cddyn = value; }
        }

        protected bool cdstatic = true;
        public virtual bool CDStatic
        {
            get { return cdstatic; }
            set { cdstatic = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or searches a new one when needed.
        /// </summary>
        protected VobInstance(IScriptWorldObject scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public VobInstance(IScriptWorldObject scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write(this.Visual);
            stream.Write(this.CDDyn);
            stream.Write(this.CDStatic);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            this.Visual = stream.ReadString();
            this.CDDyn = stream.ReadBit();
            this.CDStatic = stream.ReadBit();
        }

        #endregion
    }
}

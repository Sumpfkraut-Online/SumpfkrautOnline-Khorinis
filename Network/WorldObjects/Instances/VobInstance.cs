using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class VobInstance : BaseVobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Vob; } }

        #region ScriptObject

        public partial interface IScriptVobInstance : IScriptBaseVobInstance
        {
        }

        public new IScriptVobInstance ScriptObject
        {
            get { return (IScriptVobInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        protected string visual = "";
        /// <summary>
        /// The visual of this vob (case insensitive).
        /// </summary>
        public virtual string Visual
        {
            get { return visual; }
            set { visual = value.ToUpper(); }
        }

        protected bool cddyn = true;
        /// <summary>
        /// Gothic-collision against dynamic Vobs.
        /// </summary>
        public virtual bool CDDyn
        {
            get { return cddyn; }
            set { cddyn = value; }
        }

        protected bool cdstatic = true;
        /// <summary>
        /// Gothic-collision against static Vobs.
        /// </summary>
        public virtual bool CDStatic
        {
            get { return cdstatic; }
            set { cdstatic = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public VobInstance(IScriptVobInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public VobInstance(IScriptVobInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(this.Visual);
            stream.Write(this.CDDyn);
            stream.Write(this.CDStatic);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Visual = stream.ReadString();
            this.CDDyn = stream.ReadBit();
            this.CDStatic = stream.ReadBit();
        }

        #endregion
    }
}

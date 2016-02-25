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
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        string visual = "";
        /// <summary>
        /// The visual of this vob (case insensitive).
        /// </summary>
        public string Visual
        {
            get { return visual; }
            set { visual = value.ToUpper(); }
        }
        
        /// <summary>
        /// Gothic-collision against dynamic Vobs.
        /// </summary>
        public bool CDDyn = true;
        
        /// <summary>
        /// Gothic-collision against static Vobs.
        /// </summary>
        public bool CDStatic = true;

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

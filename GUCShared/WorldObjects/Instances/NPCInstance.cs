using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class NPCInstance : VobInstance
    {
        public override VobTypes VobType { get { return VobTypes.NPC; } }

        #region ScriptObject

        public partial interface IScriptNPCInstance : IScriptVobInstance
        {
        }

        public new IScriptNPCInstance ScriptObject
        {
            get { return (IScriptNPCInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        string name = "";
        /// <summary>The name of the NPC.</summary>
        public string Name
        {
            get { return this.name; }
            set { if (value == null) this.name = ""; else this.name = value; }
        }

        protected string bodyMesh = "";
        /// <summary>The body mesh of the NPC (case insensitive).</summary>
        public string BodyMesh
        {
            get { return bodyMesh; }
            set { if (value == null) this.bodyMesh = ""; else this.bodyMesh = value.ToUpper(); }
        }

        /// <summary>The body texture of the NPC (byte).</summary>
        public int BodyTex = 0;

        protected string headMesh = "";
        /// <summary>The head mesh of the NPC (case insensitive).</summary>
        public string HeadMesh
        {
            get { return headMesh; }
            set { if (value == null) this.headMesh = ""; else this.headMesh = value.ToUpper(); }
        }

        /// <summary>The head texture of the NPC (byte).</summary>
        public int HeadTex = 0;

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(Name);
            stream.Write(BodyMesh);
            stream.Write((byte)BodyTex);
            stream.Write(HeadMesh);
            stream.Write((byte)HeadTex);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Name = stream.ReadString();
            this.BodyMesh = stream.ReadString();
            this.BodyTex = stream.ReadByte();
            this.HeadMesh = stream.ReadString();
            this.HeadTex = stream.ReadByte();
        }

        #endregion
    }
}
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
        }

        #endregion

        #region Properties

        /// <summary>The name of the NPC.</summary>
        public string Name = "";

        protected string bodyMesh = "";
        /// <summary>The body mesh of the NPC (case insensitive).</summary>
        public string BodyMesh
        {
            get { return bodyMesh; }
            set { bodyMesh = value.ToUpper(); }
        }

        /// <summary>The body texture of the NPC (byte).</summary>
        public int BodyTex = 0;

        protected string headMesh = "";
        /// <summary>The head mesh of the NPC (case insensitive).</summary>
        public string HeadMesh
        {
            get { return headMesh; }
            set { headMesh = value.ToUpper(); }
        }

        /// <summary>The head texture of the NPC (byte).</summary>
        public int HeadTex = 0;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public NPCInstance(IScriptNPCInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public NPCInstance(IScriptNPCInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

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
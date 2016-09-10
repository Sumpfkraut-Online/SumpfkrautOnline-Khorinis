using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class NPCInstance : VobInstance
    {
        public override VobTypes VobType { get { return VobTypes.NPC; } }

        #region ScriptObject

        public partial interface IScriptNPCInstance : IScriptVobInstance
        {
        }

        public new IScriptNPCInstance ScriptObject { get { return (IScriptNPCInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public NPCInstance(IScriptNPCInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        string name = "";
        /// <summary>The name of the NPC.</summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                CanChangeNow();
                this.name = value == null ? "" : value;
            }
        }

        string bodyMesh = "";
        /// <summary>The body mesh of the NPC (case insensitive).</summary>
        public string BodyMesh
        {
            get { return bodyMesh; }
            set
            {
                CanChangeNow();
                this.bodyMesh = value == null ? "" : value.ToUpperInvariant();
            }
        }

        int bodyTex;
        /// <summary>The body texture of the NPC. (byte)</summary>
        public int BodyTex
        {
            get { return this.bodyTex; }
            set
            {
                CanChangeNow();
                this.bodyTex = value;
            }
        }
        

        string headMesh = "";
        /// <summary>The head mesh of the NPC (case insensitive).</summary>
        public string HeadMesh
        {
            get { return headMesh; }
            set
            {
                CanChangeNow();
                this.headMesh = value == null ? "" : value.ToUpperInvariant();
            }
        }

        int headTex;
        /// <summary>The head texture of the NPC. (byte)</summary>
        public int HeadTex
        {
            get { return this.headTex; }
            set
            {
                CanChangeNow();
                this.headTex = value;
            }
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(name);
            stream.Write(bodyMesh);
            stream.Write((byte)bodyTex);
            stream.Write(headMesh);
            stream.Write((byte)headTex);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.name = stream.ReadString();
            this.bodyMesh = stream.ReadString();
            this.bodyTex = stream.ReadByte();
            this.headMesh = stream.ReadString();
            this.headTex = stream.ReadByte();
        }

        #endregion
    }
}
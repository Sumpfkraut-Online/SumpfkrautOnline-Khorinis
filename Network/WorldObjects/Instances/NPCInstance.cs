using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public partial class NPCInstance : VobInstance
    {
        public static readonly Collections.InstanceDictionary NPCInstances = VobInstance.AllInstances.GetDict(Enumeration.VobTypes.NPC);

        #region Properties

        /// <summary>The default name of the NPC.</summary>
        public string Name = "";

        protected string bodyMesh = "";
        /// <summary>The body mesh of the NPC.</summary>
        public string BodyMesh
        {
            get { return bodyMesh; }
            set { bodyMesh = value.Trim().ToUpper(); }
        }

        /// <summary>The body texture of the NPC.</summary>
        public byte BodyTex = 0;

        protected string headMesh = "";
        /// <summary>The head mesh of the NPC.</summary>
        public string HeadMesh
        {
            get { return headMesh; }
            set { headMesh = value.Trim().ToUpper(); }
        }

        /// <summary>The default head texture of the NPC.</summary>
        public byte HeadTex = 0;

        /// <summary>The default body height of the NPC in percent. Default: 100</summary>
        public byte BodyHeight = 100;
        /// <summary>The default body width (x & z) of the NPC in percent. Default: 100</summary>
        public byte BodyWidth = 100;
        /// <summary>The default fatness of the NPC in percent. Default: 0</summary>
        public short Fatness = 0;

        #endregion

        internal NPCInstance()
        {
            this.VobType = VobTypes.NPC;
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(Name);
            stream.Write(BodyMesh);
            stream.Write(BodyTex);
            stream.Write(HeadMesh);
            stream.Write(HeadTex);
            stream.Write(BodyHeight);
            stream.Write(BodyWidth);
            stream.Write(Fatness);
        }

        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Name = stream.ReadString();
            this.BodyMesh = stream.ReadString();
            this.BodyTex = stream.ReadByte();
            this.HeadMesh = stream.ReadString();
            this.HeadTex = stream.ReadByte();
            this.BodyHeight = stream.ReadByte();
            this.BodyWidth = stream.ReadByte();
            this.Fatness = stream.ReadShort();
        }
    }
}
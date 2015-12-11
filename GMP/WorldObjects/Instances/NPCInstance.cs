using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.Client.WorldObjects.Instances
{
    public class NPCInstance : VobInstance
    {
        new public readonly static VobType sVobType = VobType.NPC;

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

        public NPCInstance(ushort ID)
            : base(ID)
        {
            this.VobType = sVobType;
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

            //...
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using Gothic.zTypes;
using Gothic.zClasses;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Client.WorldObjects
{
    class NPCInstance : AbstractInstance
    {
        public static InstanceManager<NPCInstance> Table = new InstanceManager<NPCInstance>("npcs.pak");

        public String name;
        public String visual;
        public String bodyMesh;
        public byte bodyTex;
        public String headMesh;
        public byte headTex;

        public float bodyHeight;
        public float bodyWidth;
        public float fatness;

        public byte voice;

        public void SetProperties(NPC npc)
        {
            npc.Name = name;
            npc.SetBodyVisuals(bodyTex, headMesh, headTex);
            npc.BodyHeight = bodyHeight;
            npc.BodyWidth = bodyWidth;
            npc.Fatness = fatness;
            npc.Voice = voice;
        }
        internal override void Read(BinaryReader br)
        {
            ID = br.ReadUInt16();
            name = br.ReadString();
            visual = br.ReadString();
            bodyMesh = br.ReadString();
            bodyTex = br.ReadByte();
            headMesh = br.ReadString();
            headTex = br.ReadByte();
            bodyHeight = (float)br.ReadByte() / 100.0f;
            bodyWidth = (float)br.ReadByte() / 100.0f;
            fatness = (float)br.ReadInt16() / 100.0f;
            voice = br.ReadByte();
        }

        internal override void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(name);
            bw.Write(visual);
            bw.Write(bodyMesh);
            bw.Write((byte)bodyTex);
            bw.Write(headMesh);
            bw.Write((byte)headTex);
            bw.Write((byte)Math.Round(100.0f * (float)bodyHeight));
            bw.Write((byte)Math.Round(100.0f * (float)bodyWidth));
            bw.Write((short)Math.Round(100.0f * (float)fatness));
            bw.Write((byte)voice);
        }
    }
}

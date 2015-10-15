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
            npc.name = name;
            npc.visual = visual;
            npc.SetBodyVisuals(bodyMesh, bodyTex, headMesh, headTex);
            npc.bodyHeight = bodyHeight;
            npc.bodyWidth = bodyWidth;
            npc.fatness = fatness;
            npc.voice = voice;
        }

        new protected static Dictionary<ushort, AbstractInstance> InstanceList;
        new protected static string fileName = "npcs.pak";
        new protected static void ReadNew(BinaryReader br)
        {
            NPCInstance inst = new NPCInstance();

            inst.ID = br.ReadUInt16();
            inst.name = br.ReadString();
            inst.visual = br.ReadString();
            inst.bodyMesh = br.ReadString();
            inst.bodyTex = br.ReadByte();
            inst.headMesh = br.ReadString();
            inst.headTex = br.ReadByte();
            inst.bodyHeight = (float)br.ReadByte() / 100.0f;
            inst.bodyWidth = (float)br.ReadByte() / 100.0f;
            inst.fatness = (float)br.ReadInt16() / 100.0f;
            inst.voice = br.ReadByte();

            InstanceList.Add(inst.ID, inst);
        }

        protected override void Write(BinaryWriter bw)
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

        public static NPCInstance Get(ushort id)
        {
            AbstractInstance inst = null;
            InstanceList.TryGetValue(id, out inst);
            return (NPCInstance)inst;
        }
   } 
}

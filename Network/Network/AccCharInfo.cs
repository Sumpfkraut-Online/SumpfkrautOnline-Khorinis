using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;

namespace GUC.Network
{
    public class AccCharInfo
    {
        public const int Max_Slots = 20;

        public int SlotNum; //slot number in the character selection menu
        public string Name;
        public int BodyMesh;
        public int BodyTex;
        public int HeadMesh;
        public int HeadTex;
        public float Fatness;
        public float BodyHeight;
        public float BodyWidth;
        public int Voice;
        public int FormerClass;

        public void Write(PacketWriter stream)
        {
            stream.Write((byte)SlotNum);
            stream.Write(Name);
            stream.Write((byte)BodyMesh);
            stream.Write((byte)BodyTex);
            stream.Write((byte)HeadMesh);
            stream.Write((byte)HeadTex);
            stream.Write((sbyte)Math.Round(Fatness * 100.0f));
            stream.Write((byte)Math.Round(BodyHeight * 100.0f));
            stream.Write((byte)Math.Round(BodyWidth * 100.0f));
            stream.Write((byte)Voice);
            stream.Write((byte)FormerClass);
        }

        public void Read(PacketReader stream)
        {
            SlotNum = stream.ReadByte();
            Name = stream.ReadString();
            BodyMesh = stream.ReadByte();
            BodyTex = stream.ReadByte();
            HeadMesh = stream.ReadByte();
            HeadTex = stream.ReadByte();
            Fatness = stream.ReadSByte() / 100.0f;
            BodyHeight = stream.ReadByte() / 100.0f;
            BodyWidth = stream.ReadByte() / 100.0f;
            Voice = stream.ReadByte();
            FormerClass = stream.ReadByte();
        }

        public override string ToString()
        {
            return String.Format("{0} {1}: bm {2}, bt {3}, hm {4}, ht {5}, f {6}, bh {7}, bw {8}, v {9}, fc {10}",
                SlotNum, Name, BodyMesh, BodyTex, HeadMesh, HeadTex, Fatness, BodyHeight, BodyWidth, Voice, FormerClass);
        }
    }
}

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

        public void Write(BitStream stream)
        {
            stream.mWrite((byte)SlotNum);
            stream.mWrite(Name);
            stream.mWrite((byte)BodyMesh);
            stream.mWrite((byte)BodyTex);
            stream.mWrite((byte)HeadMesh);
            stream.mWrite((byte)HeadTex);
            stream.mWrite((sbyte)Math.Round(Fatness * 100.0f));
            stream.mWrite((byte)Math.Round(BodyHeight * 100.0f));
            stream.mWrite((byte)Math.Round(BodyWidth * 100.0f));
            stream.mWrite((byte)Voice);
            stream.mWrite((byte)FormerClass);
        }

        public void Read(BitStream stream)
        {
            SlotNum = stream.mReadByte();
            Name = stream.mReadString();
            BodyMesh = stream.mReadByte();
            BodyTex = stream.mReadByte();
            HeadMesh = stream.mReadByte();
            HeadTex = stream.mReadByte();
            Fatness = stream.mReadSByte() / 100.0f;
            BodyHeight = stream.mReadByte() / 100.0f;
            BodyWidth = stream.mReadByte() / 100.0f;
            Voice = stream.mReadByte();
            FormerClass = stream.mReadByte();
        }

        public override string ToString()
        {
            return String.Format("{0} {1}: bm {2}, bt {3}, hm {4}, ht {5}, f {6}, bh {7}, bw {8}, v {9}, fc {10}",
                SlotNum, Name, BodyMesh, BodyTex, HeadMesh, HeadTex, Fatness, BodyHeight, BodyWidth, Voice, FormerClass);
        }
    }
}

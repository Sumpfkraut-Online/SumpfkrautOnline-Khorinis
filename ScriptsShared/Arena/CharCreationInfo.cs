using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    class CharCreationInfo
    {
        public string Name = "Spieler";
        public HumBodyMeshs BodyMesh = HumBodyMeshs.HUM_BODY_NAKED0;
        public HumBodyTexs BodyTex = HumBodyTexs.G1Hero;
        public HumHeadMeshs HeadMesh = HumHeadMeshs.HUM_HEAD_PONY;
        public HumHeadTexs HeadTex = HumHeadTexs.Face_N_Player;
        public HumVoices Voice = HumVoices.Hero;

        public float BodyWidth = 1.0f;
        public float Fatness = 0.0f;

        public void Write(PacketWriter stream)
        {
            stream.Write(Name);
            stream.Write((byte)BodyMesh);
            stream.Write((byte)BodyTex);
            stream.Write((byte)HeadMesh);
            stream.Write((byte)HeadTex);
            stream.Write((byte)Voice);
            stream.Write(BodyWidth);
            stream.Write(Fatness);
        }

        public void Read(PacketReader stream)
        {
            Name = stream.ReadString();
            BodyMesh = (HumBodyMeshs)stream.ReadByte();
            BodyTex= (HumBodyTexs)stream.ReadByte();
            HeadMesh = (HumHeadMeshs)stream.ReadByte();
            HeadTex = (HumHeadTexs)stream.ReadByte();
            Voice = (HumVoices)stream.ReadByte();

            BodyWidth = stream.ReadFloat();
            Fatness = stream.ReadFloat();
        }
    }
}

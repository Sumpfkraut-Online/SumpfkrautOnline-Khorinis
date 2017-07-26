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
    struct CharCreationInfo
    {
        public string Name;
        public HumBodyMeshs BodyMesh;
        public HumBodyTexs BodyTex;
        public HumHeadMeshs HeadMesh;
        public HumHeadTexs HeadTex;
        public HumVoices Voice;
        
        public float BodyWidth;
        public float Fatness;

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

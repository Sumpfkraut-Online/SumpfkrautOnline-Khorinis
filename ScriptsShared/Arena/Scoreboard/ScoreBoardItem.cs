using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    partial struct ScoreBoardItem
    {
        public byte ID;
        public short Ping;
        public ushort Score;
        public ushort Kills;
        public ushort Deaths;

        public void Write(PacketWriter stream)
        {
            stream.Write(ID);
            stream.Write(Ping);
            stream.Write(Score);
            stream.Write(Kills);
            stream.Write(Deaths);
        }

        public void Read(PacketReader stream)
        {
            ID = stream.ReadByte();
            Ping = stream.ReadShort();
            Score = stream.ReadUShort();
            Kills = stream.ReadUShort();
            Deaths = stream.ReadUShort();
        }
    }
}

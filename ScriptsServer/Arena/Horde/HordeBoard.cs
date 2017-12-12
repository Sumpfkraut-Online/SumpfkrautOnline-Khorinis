using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    class HordeBoard : ScoreBoard
    {
        public static readonly HordeBoard Instance = new HordeBoard();

        private HordeBoard() : base(ScriptMessages.ScoreHordeMessage)
        {
        }

        protected override void WriteBoard(PacketWriter stream)
        {
            stream.Write((ushort)HordeMode.Enemies.Count);
            stream.Write((byte)HordeMode.Players.Count);
            HordeMode.Players.ForEach(c => WritePlayer(c, stream));
        }

        void WritePlayer(ArenaClient client, PacketWriter stream)
        {
            stream.Write((byte)client.ID);
            stream.Write((short)client.HordeScore);
            stream.Write((short)client.HordeKills);
            stream.Write((short)client.HordeDeaths);

            int ping = client.BaseClient.GetLastPing();
            if (ping < 0) ping = -1;
            else if (ping > short.MaxValue) ping = short.MaxValue;
            stream.Write((short)ping);
        }
    }
}

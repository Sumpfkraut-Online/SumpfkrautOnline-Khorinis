using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    class TOBoard : ScoreBoard
    {
        public static readonly TOBoard Instance = new TOBoard();

        private TOBoard() : base(ScriptMessages.ScoreTOMessage)
        {
        }

        protected override void WriteBoard(PacketWriter stream)
        {
            stream.Write((byte)TeamMode.Teams.Count);
            for (int t = 0; t < TeamMode.Teams.Count; t++)
            {
                var players = TeamMode.Teams[t].Players;
                stream.Write((byte)players.Count);
                players.ForEach(p => WritePlayer(p, stream));
            }
        }

        void WritePlayer(ArenaClient client, PacketWriter stream)
        {
            stream.Write((byte)client.ID);
            stream.Write((short)client.TOScore);
            stream.Write((short)client.TOKills);
            stream.Write((short)client.TODeaths);

            int ping = client.BaseClient.GetLastPing();
            if (ping < 0) ping = -1;
            else if (ping > short.MaxValue) ping = short.MaxValue;
            stream.Write((short)ping);
        }
    }
}

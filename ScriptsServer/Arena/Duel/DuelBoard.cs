using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Arena.GameModes;

namespace GUC.Scripts.Arena.Duel
{
    class DuelBoard : ScoreBoard
    {
        public static readonly DuelBoard Instance = new DuelBoard();

        private DuelBoard() : base(ScriptMessages.DuelScoreBoard)
        {
        }
        
        protected override void WriteBoard(PacketWriter stream)
        {
            // players
            byte count = 0;
            int countPos = stream.Position++;
            ArenaClient.ForEach(c =>
            {
                if (c.GMTeamID == TeamIdent.FFAPlayer)
                {
                    WritePlayer(c, stream);
                    count++;
                }
            });
            stream.Edit(countPos, count);

            // spectators
            count = 0;
            countPos = stream.Position++;
            ArenaClient.ForEach(c =>
            {
                if (c.GMTeamID == TeamIdent.FFASpectator)
                {
                    WritePlayer(c, stream);
                    count++;
                }
            });
            stream.Edit(countPos, count);
        }

        void WritePlayer(ArenaClient client, PacketWriter stream)
        {
            stream.Write((byte)client.ID);
            stream.Write((short)client.DuelScore);
            stream.Write((short)client.DuelKills);
            stream.Write((short)client.DuelDeaths);

            int ping = client.BaseClient.GetLastPing();
            if (ping < 0) ping = -1;
            else if (ping > 999) ping = 999;
            stream.Write((short)ping);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    class HordeBoard : ScoreBoard
    {
        public static readonly HordeBoard Instance = new HordeBoard();

        private HordeBoard() : base(ScriptMessages.HordeScoreBoard)
        {
        }
        
        protected override void WriteBoard(PacketWriter stream)
        {
            // players
            byte count = 0;
            int countPos = stream.Position++;
            ArenaClient.ForEach(c =>
            {
                if (c.GMTeamID >= TeamIdent.GMPlayer)
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
                if (c.GMTeamID == TeamIdent.GMSpectator)
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
            stream.Write((short)client.GMScore);
            stream.Write((short)client.GMKills);
            stream.Write((short)client.GMDeaths);

            int ping = client.BaseClient.GetLastPing();
            if (ping < 0) ping = -1;
            else if (ping > 999) ping = 999;
            stream.Write((short)ping);
        }
    }
}

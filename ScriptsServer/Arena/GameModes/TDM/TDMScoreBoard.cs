using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    class TDMScoreBoard : ScoreBoard
    {
        public static readonly TDMScoreBoard Instance = new TDMScoreBoard();

        private TDMScoreBoard() : base(ScriptMessages.TDMScore)
        {
        }

        protected override void WriteBoard(PacketWriter stream)
        {
            var teams = TDMMode.ActiveMode.Teams;
            stream.Write((byte)teams.Count);
            foreach (var team in teams)
            {
                stream.Write((short)team.Score);
                var players = team.Players;
                stream.Write((byte)players.Count);
                players.ForEach(p => WritePlayer(p, stream));
            }

            // spectators
            byte count = 0;
            int countPos = stream.Position++;
            TDMMode.ActiveMode.Players.ForEach(c =>
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
            else if (ping > short.MaxValue) ping = short.MaxValue;
            stream.Write((short)ping);
        }
    }
}

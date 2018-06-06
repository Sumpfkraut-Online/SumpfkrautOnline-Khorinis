using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    class DuelBoard : ScoreBoard
    {
        public static readonly DuelBoard Instance = new DuelBoard();

        private DuelBoard() : base(ScriptMessages.ScoreDuelMessage)
        {
        }

        List<ArenaClient> list = new List<ArenaClient>(20);
        protected override void WriteBoard(PacketWriter stream)
        {
            ArenaClient.ForEach(c =>
            {
                if (Cast.Try(c, out ArenaClient client))
                    list.Add(client);
            });
            stream.Write((byte)list.Count);
            list.ForEach(c => WritePlayer(c, stream));
            list.Clear();
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

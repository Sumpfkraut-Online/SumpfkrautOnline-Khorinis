using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static class TeamMenu
    {
        static List<ArenaClient> clients = new List<ArenaClient>();

        public static void Toggle(ArenaClient client)
        {
            if (!clients.Remove(client))
            {
                clients.Add(client);
                client.SendScriptMessage(GetData(), NetPriority.Low, NetReliability.Unreliable);

                if (clients.Count == 1)
                    TeamMode.OnChangeTeamComposition += WriteUpdate;
            }
            else
            {
                if (clients.Count == 0)
                    TeamMode.OnChangeTeamComposition -= WriteUpdate;
            }
        }

        public static void Remove(ArenaClient client)
        {
            clients.Remove(client);
        }

        static PacketWriter GetData()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOTeamCount);
            stream.Write((byte)TeamMode.Teams.Count);
            for (int i = 0; i < TeamMode.Teams.Count; i++)
                stream.Write((byte)TeamMode.Teams[i].Players.Count);
            return stream;
        }

        static void WriteUpdate()
        {
            var stream = GetData();
            clients.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Unreliable));
        }
    }
}

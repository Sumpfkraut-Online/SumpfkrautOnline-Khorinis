using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static partial class Chat
    {
        public static void ReadMessage(ArenaClient sender, PacketReader stream)
        {
            string message = sender.CharInfo.Name;
            if (!sender.IsSpecating)
            {
                message += ": ";
            }
            else
            {
                message += "(Zuschauer): ";
            }
            message += stream.ReadString();
            SendMessage(message);
        }

        public static void ReadTeamMessage(ArenaClient sender, PacketReader stream)
        {
            SendMessage(sender.CharInfo.Name + " (Team): " + stream.ReadString());
        }

        public static void SendMessage(string message)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.ChatMessage);
            stream.Write(message);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));
        }

        public static void SendTeamMessage(TOTeamInst team, string message)
        {
            if (team == null)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.ChatTeamMessage);
            stream.Write(message);
            team.Players.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));
        }
    }
}

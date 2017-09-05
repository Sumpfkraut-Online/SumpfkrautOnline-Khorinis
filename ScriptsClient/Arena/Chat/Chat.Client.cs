using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static partial class Chat
    {
        public static void SendMessage(string message)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.ChatMessage);
            stream.Write(message);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public static void SendTeamMessage(string message)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.ChatTeamMessage);
            stream.Write(message);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public static void ReadMessage(PacketReader stream)
        {
            ChatMenu.Menu.ReceiveServerMessage(ChatMode.All, stream.ReadString());
        }

        public static void ReadTeamMessage(PacketReader stream)
        {
            ChatMenu.Menu.ReceiveServerMessage(ChatMode.Team, stream.ReadString());
        }
    }
}

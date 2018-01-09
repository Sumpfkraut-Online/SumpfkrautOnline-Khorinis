using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static partial class Chat
    {
        static StringBuilder builder = new StringBuilder(200);
        public static void ReadMessage(ArenaClient sender, PacketReader stream)
        {
            string message = stream.ReadString();

            if (message == "/lift")
            {
                if (sender.Character != null)
                    sender.Character.LiftUnconsciousness();

                return;
            }
            else if (message == "/nextsec")
            {
                HordeMode.ForceNextSection();
                return;
            }

            builder.Clear();
            builder.Append(sender.CharInfo.Name);
            if (sender.IsSpecating)
                builder.Append(" (Zuschauer)");
            
            builder.Append(": ");
            builder.Append(message);
            SendMessage(builder.ToString());
        }

        public static void ReadTeamMessage(ArenaClient sender, PacketReader stream)
        {
            if (sender.Team == null)
                return;

            string message = stream.ReadString();

            builder.Clear();
            builder.Append(sender.CharInfo.Name);
            builder.Append(" (Team): ");
            builder.Append(message);

            SendTeamMessage(sender.Team, builder.ToString());
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

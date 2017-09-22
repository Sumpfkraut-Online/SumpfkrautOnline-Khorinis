using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        new public static ArenaClient Client { get { return (ArenaClient)ScriptClient.Client; } }

        public static void SendJoinGameMessage()
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.JoinGame);
            SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public static void SendSpectateMessage()
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.Spectate);
            SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public static void SendCharEditMessage(CharCreationInfo info)
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.CharEdit);
            info.Write(stream);
            SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        void ReadGameInfo(PacketReader stream)
        {
            PlayerInfo.ReadHeroInfo(stream);
            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
                PlayerInfo.ReadPlayerInfoMessage(stream);

            TeamMode.ReadGameInfo(stream);
        }

        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessages id = (ScriptMessages)stream.ReadByte();
            switch (id)
            {
                case ScriptMessages.GameInfo:
                    ReadGameInfo(stream);
                    break;
                case ScriptMessages.DuelRequest:
                    DuelMode.ReadRequest(stream);
                    break;
                case ScriptMessages.DuelStart:
                    DuelMode.ReadStart(stream);
                    break;
                case ScriptMessages.DuelWin:
                    DuelMode.ReadWin(stream);
                    break;
                case ScriptMessages.DuelEnd:
                    DuelMode.ReadEnd(stream);
                    break;
                case ScriptMessages.TOWarmup:
                    TeamMode.ReadWarmup(stream);
                    break;
                case ScriptMessages.TOStart:
                    TeamMode.ReadStart(stream);
                    break;
                case ScriptMessages.TOFinish:
                    TeamMode.ReadFinish(stream);
                    break;
                case ScriptMessages.TOEnd:
                    TeamMode.ReadEnd(stream);
                    break;
                case ScriptMessages.ChatMessage:
                    Chat.ReadMessage(stream);
                    break;
                case ScriptMessages.ChatTeamMessage:
                    Chat.ReadTeamMessage(stream);
                    break;
                case ScriptMessages.TOJoinTeam:
                    TeamMode.ReadJoinTeam(stream);
                    break;
                case ScriptMessages.ScoreDuelMessage:
                    DuelBoardScreen.Instance.ReadMessage(stream);
                    break;
                case ScriptMessages.ScoreTOMessage:
                    TOBoardScreen.Instance.ReadMessage(stream);
                    break;
                case ScriptMessages.PlayerInfoMessage:
                    PlayerInfo.ReadPlayerInfoMessage(stream);
                    break;
                case ScriptMessages.PlayerQuitMessage:
                    PlayerInfo.ReadPlayerQuitMessage(stream);
                    break;                
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Network;
using GUC.Scripts.Arena.Duel;
using GUC.Scripts.Arena.GameModes;
using GUC.Scripts.Arena.GameModes.TDM;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        public static bool DetectSchinken = false;

        #region GameMode
        public static bool GMJoined { get { return PlayerInfo.HeroInfo.TeamID >= TeamIdent.GMSpectator; } }
        public static bool FFAJoined { get { return PlayerInfo.HeroInfo.TeamID == TeamIdent.FFASpectator || PlayerInfo.HeroInfo.TeamID == TeamIdent.FFAPlayer; } }
        #endregion
        
        static ArenaClient()
        {
            GUCScripts.OnWorldEnter += () =>
            {
                if (FFAJoined)
                    Menus.FreeModeMenu.Instance.Open();
            };
        }

        new public static ArenaClient Client { get { return (ArenaClient)ScriptClient.Client; } }

        public static void SendJoinGameMessage()
        {
            var stream = GetStream(ScriptMessages.JoinGame);
            SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public static void SendSpectateMessage()
        {
            var stream = GetStream(ScriptMessages.Spectate);
            SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public static void SendCharEditMessage(CharCreationInfo info)
        {
            var stream = GetStream(ScriptMessages.CharEdit);
            info.Write(stream);
            SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        void ReadGameInfo(PacketReader stream)
        {
            PlayerInfo.ReadHeroInfo(stream);
            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
                PlayerInfo.ReadPlayerInfoMessage(stream);

            GameMode.ReadGameInfo(stream);
        }

        public static PacketWriter GetStream(ScriptMessages id)
        {
            var s = GameClient.GetScriptMessageStream();
            s.Write((byte)id);
            return s;
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
                case ScriptMessages.ChatMessage:
                    Chat.ReadMessage(stream);
                    break;
                case ScriptMessages.ChatTeamMessage:
                    Chat.ReadTeamMessage(stream);
                    break;
                case ScriptMessages.DuelScore:
                    DuelBoardScreen.Instance.ReadMessage(stream);
                    break;
                case ScriptMessages.PlayerInfo:
                    PlayerInfo.ReadPlayerInfoMessage(stream);
                    break;
                case ScriptMessages.PlayerInfoTeam:
                    PlayerInfo.ReadPlayerInfoTeam(stream);
                    break;
                case ScriptMessages.PlayerQuit:
                    PlayerInfo.ReadPlayerQuitMessage(stream);
                    break;
                case ScriptMessages.PointsMessage:
                    int points = stream.ReadSByte();
                    Sumpfkraut.Menus.ScreenScrollText.AddText((points > 0 ? "Punkte +" : "Punkte ") + points);
                    break;

                case ScriptMessages.ModeStart:
                    GameMode.Start(stream.ReadString());
                    break;

                case ScriptMessages.ModePhase:
                    GameMode.ReadPhase(stream);
                    break;

                case ScriptMessages.TDMScore:
                    TDMScoreBoard.Instance.ReadMessage(stream);
                    break;
                case ScriptMessages.TDMWin:
                    TDMMode.ReadWinMessage(stream);
                    break;

            }
        }
    }
}

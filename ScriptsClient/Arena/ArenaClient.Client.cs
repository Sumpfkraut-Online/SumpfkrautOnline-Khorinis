using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Utilities;
using GUC.Scripts.Sumpfkraut.GUI;

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

        static LockTimer requestTime = new LockTimer(500);
        public static void SendDuelRequest(NPCInst target)
        {
            if (requestTime.IsReady)
            {
                var stream = GetScriptMessageStream();
                stream.Write((byte)ScriptMessages.DuelRequest);
                stream.Write((ushort)target.ID);
                SendScriptMessage(stream, NetPriority.Low, NetReliability.Unreliable);
            }
        }

        public static void SendChatMessage(Chat.ChatMode chatMode, string message)
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.ChatMessage);
            stream.Write((byte)chatMode);
            stream.Write(message);
            SendScriptMessage(stream, NetPriority.Low, NetReliability.Unreliable);
        }

        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessages id = (ScriptMessages)stream.ReadByte();
            switch (id)
            {
                case ScriptMessages.DuelRequest:
                    NPCInst requester, target;
                    if (WorldInst.Current.TryGetVob(stream.ReadUShort(), out requester) && WorldInst.Current.TryGetVob(stream.ReadUShort(), out target))
                    {
                        if (requester == this.Character)
                            DuelMessage("Du hast " + target.CustomName + " zum Duell herausgefordert.");
                        else
                            DuelMessage("Du wurdest von " + requester.CustomName + " zum Duell herausgefordert.");
                    }
                    break;
                case ScriptMessages.DuelStart:
                    NPCInst enemy;
                    if (WorldInst.Current.TryGetVob(stream.ReadUShort(), out enemy))
                    {
                        SetEnemy(enemy);
                        DuelMessage("Duell gegen " + enemy.CustomName + " + gestartet");
                    }
                    break;
                case ScriptMessages.DuelWin:
                    NPCInst winner;
                    if (WorldInst.Current.TryGetVob(stream.ReadUShort(), out winner))
                    {
                        if (winner == this.Character)
                            DuelMessage("Du hast das Duell gegen " + Enemy.CustomName + " gewonnen.");
                        else
                            DuelMessage("Du hast das Duell gegen " + Enemy.CustomName + " verloren.");
                        SetEnemy(null);
                    }
                    break;
                case ScriptMessages.DuelEnd:
                    DuelMessage("Duell beendet.");
                    SetEnemy(null);
                    break;
                case ScriptMessages.TOWarmup:
                    string name = stream.ReadString();
                    if ((activeTODef = TODef.TryGet(name)) == null)
                        throw new Exception("TODef not found: " + name);
                    Log.Logger.Log("TO Warmup: " + name);
                    Menus.TOInfoScreen.Show(activeTODef);
                    break;
                case ScriptMessages.TOStart:
                    Log.Logger.Log("TO Start: " + activeTODef.Name);
                    break;
                case ScriptMessages.TOFinish:
                    Log.Logger.Log("TO Finish: " + activeTODef.Name);
                    break;
                case ScriptMessages.TOEnd:
                    Log.Logger.Log("TO End");
                    Menus.TOInfoScreen.Hide();
                    break;
                case ScriptMessages.ChatMessage:
                    byte chatMode = stream.ReadByte();
                    string message = stream.ReadString();
                    Chat.ChatMenu.ReceiveServerMessage((Chat.ChatMode)chatMode, message);
                    break;
            }
        }

        #region TeamObjective
        TODef activeTODef;
        public TODef ActiveTODef { get { return activeTODef; } }

        #endregion

        #region Duel

        void DuelMessage(string text)
        {
            Log.Logger.Log(text);
        }

        GUCWorldSprite enemySprite = new GUCWorldSprite(100, 100, false);
        void SetEnemy(NPCInst enemy)
        {
            enemySprite.SetBackTexture("Letters.tga");
            this.Enemy = enemy;
            enemySprite.SetTarget(enemy);
            if (enemy == null) enemySprite.Hide();
            else enemySprite.Show();
        }

        #endregion
    }
}

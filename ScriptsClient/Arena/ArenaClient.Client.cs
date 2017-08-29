using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Utilities;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        new public static ArenaClient Client { get { return (ArenaClient)ScriptClient.Client; } }

        public static void SendCharCreationMessage(CharCreationInfo info)
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.CharCreation);
            info.Write(stream);
            SendScriptMessage(stream, PktPriority.Low, PktReliability.Reliable);
        }

        static LockTimer requestTime = new LockTimer(500);
        public static void SendDuelRequest(NPCInst target)
        {
            if (requestTime.IsReady)
            {
                var stream = GetScriptMessageStream();
                stream.Write((byte)ScriptMessages.DuelRequest);
                stream.Write((ushort)target.ID);
                SendScriptMessage(stream, PktPriority.Low, PktReliability.Unreliable);
            }
        }

        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessages id = (ScriptMessages)stream.ReadByte();
            switch (id)
            {
                case ScriptMessages.ScreenMessage:
                    string msg = stream.ReadString();
                    var color = stream.ReadColorRGBA();
                    Log.Logger.LogWarning(msg);
                    break;
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
                        this.Enemy = enemy;
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
                        this.Enemy = null;
                    }
                    break;
                case ScriptMessages.DuelEnd:
                    DuelMessage("Duell beendet.");
                    this.Enemy = null;
                    break;

            }
        }

        void DuelMessage(string text)
        {
            Log.Logger.Log(text);
        }
    }
}

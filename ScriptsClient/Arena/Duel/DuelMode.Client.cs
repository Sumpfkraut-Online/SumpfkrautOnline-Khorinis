using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.GUI;

namespace GUC.Scripts.Arena
{
    static partial class DuelMode
    {
        static NPCInst enemy;
        public static NPCInst Enemy { get { return enemy; } }

        public static void ReadRequest(PacketReader stream)
        {
            NPCInst requester, target;
            if (WorldInst.Current.TryGetVob(stream.ReadUShort(), out requester) && WorldInst.Current.TryGetVob(stream.ReadUShort(), out target))
            {
                if (requester == ArenaClient.Client.Character)
                    DuelMessage("Du hast " + target.CustomName + " zum Duell herausgefordert.");
                else
                    DuelMessage("Du wurdest von " + requester.CustomName + " zum Duell herausgefordert.");
            }
        }
        public static void ReadStart(PacketReader stream)
        {
            NPCInst enemy;
            if (WorldInst.Current.TryGetVob(stream.ReadUShort(), out enemy))
            {
                SetEnemy(enemy);
                DuelMessage("Duell gegen " + enemy.CustomName + " + gestartet");
            }
        }
        public static void ReadWin(PacketReader stream)
        {
            NPCInst winner;
            if (WorldInst.Current.TryGetVob(stream.ReadUShort(), out winner))
            {
                if (winner == ArenaClient.Client.Character)
                    DuelMessage("Du hast das Duell gegen " + Enemy.CustomName + " gewonnen.");
                else
                    DuelMessage("Du hast das Duell gegen " + Enemy.CustomName + " verloren.");
                SetEnemy(null);
            }
        }
        public static void ReadEnd(PacketReader stream)
        {
            DuelMessage("Duell beendet.");
            SetEnemy(null);
        }

        static LockTimer requestTime = new LockTimer(500);
        public static void SendRequest(NPCInst target)
        {
            if (!requestTime.IsReady)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelRequest);
            stream.Write((ushort)target.ID);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Unreliable);
        }

        static void DuelMessage(string text)
        {
            ChatMenu.Menu.AddMessage(ChatMode.Private, text);
            Log.Logger.Log(text);
        }

        static GUCWorldSprite enemySprite = new GUCWorldSprite(100, 100, false);
        static void SetEnemy(NPCInst newEnemy)
        {
            enemySprite.SetBackTexture("Letters.tga");

            enemy = newEnemy;
            enemySprite.SetTarget(newEnemy);

            if (newEnemy == null) enemySprite.Hide();
            else enemySprite.Show();
        }
    }
}

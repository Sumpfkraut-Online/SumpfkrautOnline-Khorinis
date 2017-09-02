using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Log;
using GUC.Utilities;
using GUC.Scripting;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        #region DuelMode

        const long DuelRequestDuration = 20 * 1000 * 10000; // 20 secs
        const float DuelMaxDistance = 1500.0f; // distance between players for the duel to automatically end
        
        static ArenaClient()
        {
            Logger.Log("Duel mode initialised.");
            NPCInst.sOnHitCheck += (a, t) => 
            {
                if (a.Client != null)
                {
                    var client = (ArenaClient)a.Client;
                    if (client.EnemyClient != null && client.EnemyClient == t.Client)
                        return true;
                }
                return false;
            };
            NPCInst.sOnHit += (a, t, d) =>
            {
                if (a.Client != null)
                {
                    var client = (ArenaClient)a.Client;
                    if (client.EnemyClient != null && t.GetHealth() <= 0 && client.EnemyClient == t.Client)
                        client.DuelWin();
                }
            };
            NPCInst.sOnNPCInstMove += (npc, p, d, m) =>
            {
                if (npc.Client != null)
                {
                    var client = (ArenaClient)npc.Client;
                    var enemy = client.EnemyClient;
                    if (enemy != null && enemy.Character != null 
                    && npc.GetPosition().GetDistance(enemy.Character.GetPosition()) > DuelMaxDistance)
                        client.DuelEnd();
                }
            };
        }

        public ArenaClient EnemyClient { get { return (ArenaClient)this.Enemy?.Client; } }
        List<ArenaClient, GUCTimer> duelRequests = new List<ArenaClient, GUCTimer>(3);

        public void DuelRequest(NPCInst target)
        {
            if (this.IsSpecating || this.EnemyClient != null || target.Client == null
                || this.Character.IsDead || target.IsDead || target.Client.IsSpecating)
                return;

            var targetClient = (ArenaClient)target.Client;
            if (targetClient.EnemyClient != null)
                return;

            int index;
            if ((index = targetClient.duelRequests.FindIndex(r => r.Item1 == this)) >= 0) // other player has already sent a request
            {
                targetClient.duelRequests[index].Item2.Stop();
                targetClient.duelRequests.RemoveAt(index);
                this.DuelStart(targetClient);
            }
            else if ((index = duelRequests.FindIndex(r => r.Item1 == targetClient)) >= 0) // already sent a request
            {
                duelRequests[index].Item2.Restart();
            }
            else // add new request
            {
                var timer = new GUCTimer(DuelRequestDuration, () => this.duelRequests.RemoveAll(r => r.Item1 == targetClient));
                timer.Start();
                duelRequests.Add(targetClient, timer);

                var stream = ArenaClient.GetScriptMessageStream();
                stream.Write((byte)ScriptMessages.DuelRequest);
                stream.Write((ushort)this.Character.ID);
                stream.Write((ushort)target.ID);
                this.SendScriptMessage(stream, PktPriority.Low, PktReliability.ReliableOrdered);
                targetClient.SendScriptMessage(stream, PktPriority.Low, PktReliability.ReliableOrdered);
            }
        }

        void DuelStart(ArenaClient enemyClient)
        {
            this.Enemy = enemyClient.Character;
            enemyClient.Enemy = this.Character;

            var character = this.Character;
            var enemyChar = enemyClient.Character;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelStart);
            stream.Write((ushort)character.ID);
            enemyClient.SendScriptMessage(stream, PktPriority.Low, PktReliability.ReliableOrdered);

            stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelStart);
            stream.Write((ushort)enemyChar.ID);
            this.SendScriptMessage(stream, PktPriority.Low, PktReliability.ReliableOrdered);
        }

        void DuelWin()
        {
            if (this.EnemyClient == null)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelWin);
            stream.Write((ushort)this.Character.ID);
            this.SendScriptMessage(stream, PktPriority.Low, PktReliability.ReliableOrdered);
            this.EnemyClient.SendScriptMessage(stream, PktPriority.Low, PktReliability.ReliableOrdered);

            this.EnemyClient.Enemy = null;
            this.Enemy = null;
        }

        void DuelEnd()
        {
            if (this.EnemyClient == null)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelEnd);
            this.SendScriptMessage(stream, PktPriority.Low, PktReliability.ReliableOrdered);
            this.EnemyClient.SendScriptMessage(stream, PktPriority.Low, PktReliability.ReliableOrdered);

            this.EnemyClient.Enemy = null;
            this.Enemy = null;
        }

        #endregion

        partial void pOnConnect()
        {
            this.SetToSpectator(WorldInst.Current, new Vec3f(), new Vec3f());
        }

        public void SendScreenMessage(string message)
        {
            SendScreenMessage(message, ColorRGBA.White);
        }

        public void SendScreenMessage(string message, ColorRGBA color)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.ScreenMessage);
            stream.Write(message);
            stream.Write(color);
            this.BaseClient.SendScriptMessage(stream, PktPriority.Low, PktReliability.Reliable);
        }

        public void SendChatMessage(byte chatMode, string message)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.ChatMessage);
            stream.Write(chatMode);
            stream.Write(message);
            this.BaseClient.SendScriptMessage(stream, PktPriority.Low, PktReliability.Reliable);
        }

        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessages id = (ScriptMessages)stream.ReadByte();
            switch (id)
            {
                case ScriptMessages.CharCreation:
                    ReadCharCreation(stream);
                    break;
                case ScriptMessages.DuelRequest:
                    NPCInst target;
                    if (!this.IsSpecating && this.Character.World.TryGetVob(stream.ReadUShort(), out target))
                        this.DuelRequest(target);
                    break;
                case ScriptMessages.ChatMessage:
                    SendChatMessage(stream.ReadByte(), stream.ReadString());
                    break;
            }
        }

        void ReadCharCreation(PacketReader stream)
        {
            CharCreationInfo info = new CharCreationInfo();
            info.Read(stream);

            NPCInst npc = new NPCInst(NPCDef.Get(info.BodyMesh == Sumpfkraut.Visuals.HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"));
            npc.UseCustoms = true;
            npc.CustomBodyTex = info.BodyTex;
            npc.CustomHeadMesh = info.HeadMesh;
            npc.CustomHeadTex = info.HeadTex;
            npc.CustomVoice = info.Voice;
            npc.CustomFatness = info.Fatness;
            npc.CustomScale = new Vec3f(info.BodyWidth, 1.0f, info.BodyWidth);
            npc.CustomName = info.Name;

            var item = new ItemInst(ItemDef.Get("ItMw_1h_Bau_Mace"));
            npc.Inventory.AddItem(item);
            npc.EquipItem(item);

            item = new ItemInst(ItemDef.Get("ITAR_Prisoner"));
            npc.Inventory.AddItem(item);
            npc.EquipItem(item);

            Sumpfkraut.Visuals.ScriptOverlay ov;
            if (npc.ModelDef.TryGetOverlay("1HST1", out ov))
                npc.ModelInst.ApplyOverlay(ov);

            npc.Spawn(WorldInst.Current);
            this.SetControl(npc);
        }
    }
}

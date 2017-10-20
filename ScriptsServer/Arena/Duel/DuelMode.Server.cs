using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripting;
using GUC.Log;

namespace GUC.Scripts.Arena
{
    static partial class DuelMode
    {

        const long DuelRequestDuration = 20 * 1000 * 10000; // 20 secs
        const float DuelMaxDistance = 1500.0f; // distance between players for the duel to automatically end

        static DuelMode()
        {
            Logger.Log("Duel mode initialised.");
            NPCInst.sOnHitCheck += (a, t) =>
            {
                if (a.Client != null)
                {
                    var attacker = (ArenaClient)a.Client;
                    var target = (ArenaClient)t.Client;
                    if (target != null)
                    {
                        if (attacker.Team != null && target.Team != null)
                            return true;
                        else if (attacker.DuelEnemy == target)
                            return true;
                    }
                }
                return false;
            };
            NPCInst.sOnHit += (NPCInst a, NPCInst t, int d) =>
            {
                if (a == null)
                    return;

                var attacker = (ArenaClient)a.Client;
                var target = (ArenaClient)t.Client;

                if (attacker != null && target != null)
                {
                    if (t.GetHealth() <= 0)
                    {
                        if (attacker.Team != null && target.Team != null)
                            TeamMode.Kill(attacker, target);
                        else if (attacker.DuelEnemy == target)
                            DuelWin(attacker);
                    }
                }
            };

            NPCInst.sOnNPCInstMove += (npc, p, d, m) =>
            {
                if (npc.Client != null)
                {
                    var client = (ArenaClient)npc.Client;
                    var enemy = client.DuelEnemy;
                    if (enemy != null && enemy.Character != null
                    && npc.GetPosition().GetDistance(enemy.Character.GetPosition()) > DuelMaxDistance)
                        DuelEnd(client);
                }
            };
        }

        public static void ReadRequest(ArenaClient requester, PacketReader stream)
        {
            if (requester.Character == null || requester.IsDueling || requester.Character.IsDead || requester.Team != null)
                return;

            if (!requester.Character.World.TryGetVob(stream.ReadUShort(), out NPCInst target))
                return;

            if (target.Client == null || target.IsDead)
                return;

            var targetClient = (ArenaClient)target.Client;
            if (targetClient.IsDueling || targetClient.Team != null)
                return;

            int index;
            if ((index = targetClient.DuelRequests.FindIndex(r => r.Item1 == requester)) >= 0) // other player has already sent a request
            {
                targetClient.DuelRequests[index].Item2.Stop();
                targetClient.DuelRequests.RemoveAt(index);
                DuelStart(requester, targetClient);
            }
            else if ((index = requester.DuelRequests.FindIndex(r => r.Item1 == targetClient)) >= 0) // already sent a request
            {
                requester.DuelRequests[index].Item2.Restart();
            }
            else // add new request
            {
                var timer = new GUCTimer(DuelRequestDuration, () => requester.DuelRequests.RemoveAll(r => r.Item1 == targetClient));
                timer.Start();
                requester.DuelRequests.Add(targetClient, timer);

                SendRequest(requester, targetClient);
            }
        }

        static void SendRequest(ArenaClient requester, ArenaClient target)
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelRequest);
            stream.Write((ushort)requester.Character.ID);
            stream.Write((ushort)target.Character.ID);
            requester.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
            target.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        static void DuelStart(ArenaClient player1, ArenaClient player2)
        {
            player1.DuelEnemy = player2;
            player2.DuelEnemy = player1;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelStart);
            stream.Write((ushort)player2.Character.ID);
            player1.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);

            stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelStart);
            stream.Write((ushort)player1.Character.ID);
            player2.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);
        }

        public static void DuelWin(ArenaClient winner)
        {
            if (!winner.IsDueling)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelWin);
            stream.Write((ushort)winner.Character.ID);
            winner.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);
            winner.DuelEnemy.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);

            winner.DuelEnemy.DuelDeaths++;
            winner.DuelEnemy.DuelScore--;

            winner.DuelKills++;
            winner.DuelScore += 2;

            winner.DuelEnemy.DuelEnemy = null;
            winner.DuelEnemy = null;
        }

        static void DuelEnd(ArenaClient client)
        {
            if (!client.IsDueling)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.DuelEnd);
            client.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);
            client.DuelEnemy.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);

            client.DuelEnemy.DuelEnemy = null;
            client.DuelEnemy = null;
        }
    }
}

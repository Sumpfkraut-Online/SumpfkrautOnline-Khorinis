using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripting;
using GUC.Utilities;
using GUC.Log;

namespace GUC.Scripts.Arena
{
    class DuelMode
    {
        const long DuelRequestDuration = 20 * 1000 * 10000; // 20 secs
        const float DuelMaxDistance = 1500.0f; // distance between players for the duel to automatically end
        
        public class ClientInfo
        {
            static ClientInfo Get(NPCInst npc)
            {
                return (npc == null || npc.Client == null) ? null : ((ArenaClient)npc.Client).GetDuelInfo();
            }

            static ClientInfo()
            {
                Logger.Log("Duel mode initialised.");
                NPCInst.sOnHitCheck += (a, t) => { return Get(a).Enemy != null && Get(a).Enemy == Get(t); };
                NPCInst.sOnHit += (a, t, d) =>
                {
                    if (t.GetHealth() <= 0 && Get(a).Enemy == Get(t))
                        Get(a).DuelWin();
                };
                NPCInst.sOnNPCInstMove += (npc, p, d, m) =>
                {
                    ClientInfo npcInfo, enemyInfo;
                    if ((npcInfo = Get(npc)) == null || (enemyInfo = npcInfo.Enemy) == null)
                        return;

                    NPCInst enemy = enemyInfo.Client.Character;
                    if (enemy == null)
                        return;

                    if (npc.GetPosition().GetDistance(enemy.GetPosition()) > DuelMaxDistance)
                        npcInfo.DuelEnd();
                };
            }

            public ArenaClient Client { get; private set; }
            public ClientInfo Enemy { get; private set; }

            List<NPCInst, GUCTimer> duelRequests = new List<NPCInst, GUCTimer>(3);

            public ClientInfo(ArenaClient client)
            {
                this.Client = client;
            }

            public void DuelRequest(NPCInst target)
            {
                if (Client.IsSpecating || Enemy != null || target.Client == null
                    || Client.Character.IsDead || target.IsDead || target.Client.IsSpecating)
                    return;

                var targetClient = ((ArenaClient)target.Client).GetDuelInfo();
                if (targetClient.Enemy != null)
                    return;

                int index;
                if ((index = targetClient.duelRequests.FindIndex(r => r.Item1 == Client.Character)) >= 0) // other player has already sent a request
                {
                    targetClient.duelRequests[index].Item2.Stop();
                    targetClient.duelRequests.RemoveAt(index);
                    this.DuelStart(targetClient);
                }
                else if ((index = duelRequests.FindIndex(r => r.Item1 == target)) >= 0) // already sent a request
                {
                    duelRequests[index].Item2.Restart();
                }
                else // add new request
                {
                    var timer = new GUCTimer(DuelRequestDuration, () => this.duelRequests.RemoveAll(r => r.Item1 == target));
                    timer.Start();
                    duelRequests.Add(target, timer);
                }
            }

            void DuelStart(ClientInfo enemy)
            {
                this.Enemy = enemy;
                enemy.Enemy = this;

                var character = this.Client.Character;
                var enemyChar = enemy.Client.Character;
                
                this.Client.SendScreenMessage("Duell gegen " + enemyChar.CustomName + " gestartet.");
                enemy.Client.SendScreenMessage("Duell gegen " + character.CustomName + " gestartet.");
            }

            void DuelWin()
            {
                if (this.Enemy == null)
                    return;
                
                this.Client.SendScreenMessage("Duell gegen " + Client.Character.CustomName + " gewonnen.");
                Enemy.Client.SendScreenMessage("Duell gegen " + Enemy.Client.Character.CustomName + " verloren.");

                this.Enemy.Enemy = null;
                this.Enemy = null;
            }

            void DuelEnd()
            {
                if (this.Enemy == null)
                    return;

                this.Client.SendScreenMessage("Duell gegen " + Client.Character.CustomName + " beendet.");
                Enemy.Client.SendScreenMessage("Duell gegen " + Enemy.Client.Character.CustomName + " beendet.");

                this.Enemy.Enemy = null;
                this.Enemy = null;
            }
        }

    }
}

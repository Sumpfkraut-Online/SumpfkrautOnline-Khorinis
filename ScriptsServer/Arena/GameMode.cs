using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripting;

namespace GUC.Scripts.Arena
{
    class GameMode
    {
        // Normal Mode (DUEL)

        struct DuelPair
        {
            public NPCInst Player1;
            public NPCInst Player2;
        }
        static List<DuelPair> activeDuels = new List<DuelPair>();

        const long RequestDuration = 10 * 1000 * 10000; // 10 secs
        struct DuelRequestPair
        {
            public NPCInst Requester;
            public NPCInst Target;
            public GUCTimer Timer;
        }
        static List<DuelRequestPair> duelRequests = new List<DuelRequestPair>();

        public static void DuelRequest(NPCInst requester, NPCInst target)
        {
            ((ArenaClient)requester.BaseInst.Client.ScriptObject).SendScreenMessage("Duel requested");
            for (int i = 0; i < duelRequests.Count; i++)
            {
                var pair = duelRequests[i];
                if (pair.Requester == requester && pair.Target == target)
                { 
                    pair.Timer.Restart();
                    return;
                }
                else if (pair.Requester == target && pair.Target == requester)
                {
                    pair.Timer.Stop();
                    duelRequests.RemoveAt(i);
                    DuelStart(requester, target);
                    return;
                }
            }

            var newPair = new DuelRequestPair();
            newPair.Requester = requester;
            newPair.Target = target;
            newPair.Timer = new GUCTimer(RequestDuration, () => duelRequests.Remove(newPair));
            newPair.Timer.Start();
            duelRequests.Add(newPair);
        }

        static void DuelStart(NPCInst player1, NPCInst player2)
        {

        }
    }
}

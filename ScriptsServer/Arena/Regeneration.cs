using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena
{
    static class Regeneration
    {
        const long RegenerationInterval = 300 * TimeSpan.TicksPerMillisecond;
        const long RegenerationOffset = 8 * TimeSpan.TicksPerSecond;

        static GUCTimer timer;

        public static void Init()
        {
            timer = new GUCTimer(RegenerationInterval, RegeneratePlayers);
            timer.Start();
        }

        static void RegeneratePlayers()
        {
            ArenaClient.ForEach(c =>
            {
                ArenaClient client = (ArenaClient)c;
                if (client.IsCharacter && client.DuelEnemy == null && client.HordeClass == null)
                    RegeneratePlayer(client.Character);
            });
        }

        static void RegeneratePlayer(NPCInst npc)
        {
            int diff = npc.HPMax - npc.HP;
            if (diff <= 0 || npc.IsDead || (GameTime.Ticks - npc.LastHitMove) < RegenerationOffset)
                return;

            int add = npc.TeamID == -1 ? 8 : 2;
            if (add > diff)
                add = diff;

            npc.SetHealth(npc.HP + add);
        }
    }
}

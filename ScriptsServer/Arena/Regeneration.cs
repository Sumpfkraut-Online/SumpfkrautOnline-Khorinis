using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Scripts.Arena.GameModes;

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
                if (client.IsCharacter && client.DuelEnemy == null)
                    RegeneratePlayer(client);
            });
        }

        static void RegeneratePlayer(ArenaClient client)
        {
            var npc = client.Character;

            int diff = npc.HPMax - npc.HP;
            if (diff <= 0 || npc.IsDead)
                return;

            if (client.GMTeamID >= TeamIdent.GMPlayer && (GameTime.Ticks - npc.LastHitMove) < RegenerationOffset)
                return;

            int add = client.GMTeamID >= TeamIdent.GMPlayer ? 2 : 8;
            if (add > diff)
                add = diff;

            npc.SetHealth(npc.HP + add);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI
{
    public static class AI_Events
    {
        public delegate void RoutineFunction(NPC proto);
        
        public delegate void AssessDamageFunction(NPC npc, NPC victim, NPC attacker, int damage, bool dropUnconscious, bool dropDead);
        public delegate void AssessTargetFunction(NPC npc, NPC target);

        public delegate void FightRoutine(NPC npc);
        public delegate void UpdateRoutine(NPC npc);


        public static void Init()
        {
            DamageScript.Damages += OnDamage;
        }

        public static void OnDamage(NPC victim, NPC attacker, 
            int damage, bool dropUnconscious, bool dropDead)
        {
            if (victim.getAI().AssessDamageRoutine != null)
                victim.getAI().AssessDamageRoutine(victim, victim, attacker, damage,
                    dropUnconscious, dropDead);

            
            foreach (KeyValuePair<NPC, NPC> npcPair in victim.getAI().TargetList)
            {
                NPC npc = npcPair.Key;

                if (npc.getAI().AssessOtherDamageRoutine != null)
                    npc.getAI().AssessOtherDamageRoutine(npc, victim, attacker, damage,
                        dropUnconscious, dropDead);
            }

        }
    }
}

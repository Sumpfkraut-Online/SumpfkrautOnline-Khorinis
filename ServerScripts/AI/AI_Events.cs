using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI
{
    public static class AI_Events
    {
        public delegate void RoutineFunction(NPCProto proto);
        
        public delegate void AssessDamageFunction(NPCProto npc, NPCProto victim, NPCProto attacker, int damage, bool dropUnconscious, bool dropDead);
        public delegate void AssessTargetFunction(NPCProto npc, NPCProto target);

        public delegate void FightRoutine(NPCProto npc);
        public delegate void UpdateRoutine(NPCProto npc);


        public static void Init()
        {
            DamageScript.Damages += OnDamage;
        }

        public static void OnDamage(NPCProto victim, NPCProto attacker, 
            int damage, bool dropUnconscious, bool dropDead)
        {
            if (victim.getAI().AssessDamageRoutine != null)
                victim.getAI().AssessDamageRoutine(victim, victim, attacker, damage,
                    dropUnconscious, dropDead);

            
            foreach (KeyValuePair<NPCProto, NPCProto> npcPair in victim.getAI().TargetList)
            {
                NPCProto npc = npcPair.Key;

                if (npc.getAI().AssessOtherDamageRoutine != null)
                    npc.getAI().AssessOtherDamageRoutine(npc, victim, attacker, damage,
                        dropUnconscious, dropDead);
            }

        }
    }
}

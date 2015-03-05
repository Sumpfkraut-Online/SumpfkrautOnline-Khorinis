using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI
{
    /** 
     * Static class that contains Events for the AI.
     * This class implements an OnDamage-Event */
    public static class AI_Events
    {
        public delegate void RoutineFunction(NPCProto proto); /**< ???*/
        
        public delegate void AssessDamageFunction(NPCProto npc, NPCProto victim, NPCProto attacker, int damage, bool dropUnconscious, bool dropDead); /**< Called when an NPC notices someone nearby taking (dealing?) damage.*/
        public delegate void AssessTargetFunction(NPCProto npc, NPCProto target); /**< ???*/

        public delegate void FightRoutine(NPCProto npc); /**< Some sort of Fight-AI? Why is this an Event?*/
        public delegate void UpdateRoutine(NPCProto npc); /**< (Why) Is this an Event?*/

        /**
         * This adds OnDamage() to the standard Damage-Handler.
         */
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

using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs;

namespace GUC.Scripts.Sumpfkraut.Mobs
{
    public partial class MobBedInst : MobInst
    {
        /// <summary>
        /// Called when NPC starts interacting with the mob.
        /// </summary>
        public override void StartUsing(NPCInst npcInst)
        {
        }

        /// <summary>
        /// Called when NPC stops interacting with the mob.
        /// </summary>
        public override void StopUsing(NPCInst npcInst)
        {
        }
        /// <summary>
        /// Check whether the npc has the requirements to use this vob. Display Feedback if not.
        /// </summary>
        public override bool HasRequirements(NPCInst npc)
        {
            return true;
        }
    }
}

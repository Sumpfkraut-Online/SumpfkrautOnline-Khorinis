using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs;
using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Mobs
{
    public partial class MobLadderInst : MobInst
    {
        /// <summary>
        /// Called when NPC starts interacting with the mob.
        /// </summary>
        public override void StartUsing(NPCInst npcInst)
        {
            NPCCatalog AniCatalog = (NPCCatalog)npcInst.ModelDef?.Catalog;
            //StartAnimation(npcInst, AniCatalog.Mob.Ladder.ClimbUp);
            npcInst.SetPosAng(this.GetPosition() + new Vec3f(53.0f, 0, -58), this.GetAngles());
        }

        /// <summary>
        /// Called when NPC stops interacting with the mob.
        /// </summary>
        public override void StopUsing(NPCInst npcInst)
        {
            NPCCatalog AniCatalog = (NPCCatalog)npcInst.ModelDef?.Catalog;
            StopAnimation(npcInst, AniCatalog.Mob.Ladder.ClimbDown);
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

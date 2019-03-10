using GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Mobs
{
    public partial class MobChair : MobInst
    {
        /// <summary>
        /// Called when NPC starts interacting with the mob.
        /// </summary>
        public override void StartUsing(NPCInst npcInst)
        {
            NPCCatalog AniCatalog = (NPCCatalog)npcInst.ModelDef?.Catalog;
            npcInst.ModelInst.StartAniJob(AniCatalog.Mob.Chair.Sit);
            npcInst.SetPosAng(this.GetPosition() + new Vec3f(53.0f, 0, -58), this.GetAngles());
        }

        /// <summary>
        /// Called when NPC stops interacting with the mob.
        /// </summary>
        public override void StopUsing(NPCInst npcInst)
        {
            NPCCatalog AniCatalog = (NPCCatalog)npcInst.ModelDef?.Catalog;
            npcInst.ModelInst.StartAniJob(AniCatalog.Mob.Chair.StandUp);
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

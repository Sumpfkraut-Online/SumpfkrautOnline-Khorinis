using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.Mobs
{
    public partial class MobChairInst : MobInst
    {
        /// <summary>
        /// Called when NPC starts interacting with the mob.
        /// </summary>
        public override void StartUsing(NPCInst npcInst)
        {
            Log.Logger.Log("StartUsing chair Mob on client");
            //NPCCatalog AniCatalog = (NPCCatalog)npcInst.ModelDef?.Catalog;
            //ScriptAniJob standToSit = new ScriptAniJob("mob_chair_standToSit", "t_Chair_S0_2_S1", new ScriptAni(0, 39));
            //model.AddAniJob(standToSit);
            //npcInst.ModelInst.StartAniJob()
            //npcInst.SetPosAng(this.GetPosition() + new Vec3f(53.0f, 0, -58), this.GetAngles());
        }

        /// <summary>
        /// Called when NPC stops interacting with the mob.
        /// </summary>
        public override void StopUsing(NPCInst npcInst)
        {
            Log.Logger.Log("StopUsing Mob on client");
            //NPCCatalog AniCatalog = (NPCCatalog)npcInst.ModelDef?.Catalog;
            //npcInst.ModelInst.StartAniJob(AniCatalog.Mob.Chair.StandUp);
        }

        /// <summary>
        /// 
        /// Check whether the npc has the requirements to use this vob. Display Feedback if not.
        /// </summary>
        public override bool HasRequirements(NPCInst npc)
        {
            return true;
        }
    }
}

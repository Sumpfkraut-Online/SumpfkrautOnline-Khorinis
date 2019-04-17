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
    public partial class MobLadderInst : MobInst
    {
        /// <summary>
        /// Called when NPC starts interacting with the mob.
        /// </summary>
        public override void StartUsing(NPCInst npcInst)
        {
            ScriptAniJob AniJob = new ScriptAniJob();
            npcInst.ModelInst.StartAniJob(AniJob);
            Log.Logger.Log("Client Animation");
            //npcInst.BaseInst.gModel.StartAnimation("t_Ladder_Stand_2_S0");
            //npcInst.ModelInst.StartAniJob
            var gModel = npcInst.BaseInst.gModel;
            int aniID = gModel.GetAniIDFromAniName("t_Ladder_Stand_2_S0");
            Log.Logger.Log("AniID=" + aniID);
            if (aniID > 0)
            {
                var gAni = gModel.GetAniFromAniID(aniID);
                if (gAni.Address != 0)
                {
                    gModel.StartAni(gAni, 0);
                }
            }
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

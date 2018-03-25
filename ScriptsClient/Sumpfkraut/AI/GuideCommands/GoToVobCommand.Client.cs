using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.VobGuiding;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToVobCommand
    {
        public override void Update(GuidedVob vob, long now)
        {
            if (Target == null)
                return;

            if (!Cast.Try(vob.ScriptObject, out NPCInst npc))
                throw new Exception("Vob used with GoToVobCommand is no NPC!");

            if (npc.IsDead)
                return;

            var gNpc = npc.BaseInst.gVob;

            gNpc.RbtTimer = 500;
            gNpc.RbtTargetVob = Target.BaseInst.gVob;
            Target.GetPosition().SetGVec(gNpc.RbtTargetPos);
            gNpc.RbtMaxTargetDist = Distance * Distance;
            gNpc.RbtGotoFollowPosition();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.VobGuiding;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using Gothic.Objects.EventManager;
using GUC.Types;
using Gothic.Types;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToVobCommand
    {
        public override void Update(GuidedVob vob, long now)
        {
            if (!Cast.Try(vob.ScriptObject, out NPCInst npc))
                throw new Exception("Vob used with GoToPosCommand is no NPC!");

            if (npc.IsDead)
                return;

            var gNpc = npc.BaseInst.gVob;

            gNpc.RbtTimer = 500;
            gNpc.RbtTargetVob = Target.BaseInst.gVob;
            Target.GetPosition().SetGVec(gNpc.RbtTargetPos);
            gNpc.RbtMaxTargetDist = 200 * 200;
            gNpc.RbtGotoFollowPosition();
        }
    }
}

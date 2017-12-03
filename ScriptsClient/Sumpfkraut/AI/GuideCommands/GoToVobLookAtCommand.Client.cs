using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects.VobGuiding;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToVobLookAtCommand
    {
        public override void Update(GuidedVob vob, long now)
        {
            if (Target == null)
                return;

            if (!Cast.Try(vob.ScriptObject, out NPCInst npc))
                throw new Exception("Vob used with GoToPosLookAtCommand is no NPC!");

            if (npc.IsDead)
                return;

            var gNpc = npc.BaseInst.gVob;

            gNpc.RbtTimer = 500;
            gNpc.RbtTargetVob = Target.BaseInst.gVob;
            Target.GetPosition().SetGVec(gNpc.RbtTargetPos);
            gNpc.RbtMaxTargetDist = Distance * Distance;
            gNpc.RbtGotoFollowPosition();

            Vec3f npcPos = npc.GetPosition();
            Vec3f targetPos = Target.GetPosition();
            if (npcPos.GetDistance(targetPos) <= Distance)
            {
                const float maxTurnFightSpeed = 0.08f;

                float bestYaw = Angles.GetYawFromAtVector(targetPos - npcPos);
                Angles curAngles = npc.GetAngles();

                float yawDiff = Angles.Difference(bestYaw, curAngles.Yaw);
                curAngles.Yaw += Alg.Clamp(-maxTurnFightSpeed, yawDiff, maxTurnFightSpeed);

                npc.SetAngles(curAngles);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.VobGuiding;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using Gothic.Objects;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToPosCommand : GuideCmd
    {
        public override void Start(GuidedVob vob)
        {
        }

        public override void Update(GuidedVob vob, long now)
        {
            if (!Cast.Try(vob.ScriptObject, out NPCInst npc))
                throw new Exception("Vob used with GoToPos is no NPC!");

            if (npc.IsDead)
                return;

            var gNpc = npc.BaseInst.gVob;

            gNpc.RbtTimer = 500;
            gNpc.RbtTargetVob = zCVob.NullVob;
            gNpc.RbtBitfield = gNpc.RbtBitfield | (1 << 4); // stand when reached
            this.Destination.SetGVec(gNpc.RbtTargetPos);
            gNpc.RbtMaxTargetDist = 100 * 100;

            gNpc.RobustTrace();
        }

        public override void Stop(GuidedVob vob)
        {
        }
    }
}

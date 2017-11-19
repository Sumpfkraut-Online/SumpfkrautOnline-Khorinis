using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.VobGuiding;
using GUC.WorldObjects;
using Gothic.Objects.EventManager;
using GUC.Types;
using Gothic.Types;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToVobCommand
    {
        bool tracing = false;
        public override void Update(GuidedVob vob, long now)
        {
            if (!(vob is NPC))
                throw new Exception("Vob used with GoToPosCommand is no NPC!");

            if (((NPC)vob).IsDead)
                return;

            Vec3f dest = this.Destination;

            var gVob = ((NPC)vob).gVob;
            if (vob.Position.GetDistance(dest) < 1)
            {
                tracing = false;
                return;
            }
            
            if (!tracing)
            {
                gVob.RbtReset();
                tracing = true;
            }

            using (zVec3 vec = dest.CreateGVec())
                gVob.RbtUpdate(vec, new Gothic.Objects.zCVob(0));

            gVob.RobustTrace();

            // improve -> use gVob.RbtGotoFollowPosition();
            // or start an oCMsgMovement with SubTypes.GoRoute on gVob->failurePossibility >= 100
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.VobGuiding;
using Gothic.Objects.EventManager;
using Gothic.Types;
using GUC.WorldObjects;

namespace GUC.Scripts.Sumpfkraut.AI.GuideCommands
{
    partial class GoToPosCommand : GuideCmd
    {
        public override void Start(GuidedVob vob)
        {
        }

        public override void Update(GuidedVob vob, long now)
        {
            if (!(vob is NPC))
                throw new Exception("Vob used with GoToPosCommand is no NPC!");

            if (((NPC)vob).IsDead)
                return;

            if (vob.GetPosition().GetDistance(this.destination) < 50)
                return;

            var gVob = vob.gVob;
            var em = gVob.GetEM(0);

            if (em.GetActiveMsg().Address == 0)
            {
                using (zVec3 vec = destination.CreateGVec())
                    em.OnMessage(oCMsgMovement.Create(oCMsgMovement.SubTypes.RobustTrace, vec), gVob);
            }
        }

        public override void Stop(GuidedVob vob)
        {
            vob.gVob.GetEM(0).KillMessages();
        }
    }
}

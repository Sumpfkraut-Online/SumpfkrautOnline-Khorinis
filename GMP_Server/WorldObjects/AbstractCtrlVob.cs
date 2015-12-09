using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Types;

namespace GUC.Server.WorldObjects
{
    /*public abstract class AbstractCtrlVob : AbstractVob
    {
        protected AbstractCtrlVob(object scriptObject) : base (scriptObject)
        {
        }

        internal Client VobController;

        public override void Despawn()
        {
            base.Despawn();

            if (VobController != null)
                VobController.RemoveControlledVob(this);
        }

        internal void FindNewController()
        {
            Client newCtrler = FindNearestController();

            if (newCtrler == VobController)
                return;

            if (VobController != null)
            {
                VobController.RemoveControlledVob(this);
            }
            VobController = newCtrler;
            if (newCtrler != null)
            {
                VobController.AddControlledVob(this);
            }
        }

        Client FindNearestController()
        {
            Client nearest = null;
            float bestDist = float.MaxValue;
            float dist;

            foreach (NPC npc in cell.SurroundingPlayers())
            {
                dist = this.Position.GetDistance(npc.Position);
                if (dist < bestDist)
                {
                    nearest = npc.client;
                    bestDist = dist;
                }
            }
            return nearest;
        }
    }*/
}

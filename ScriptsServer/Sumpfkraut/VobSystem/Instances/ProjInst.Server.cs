using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ProjInst
    {
        public ItemInst Item { get { return (ItemInst)this.BaseInst.Item?.ScriptObject; } set { this.BaseInst.Item = value?.BaseInst; } }

        public NPCInst Shooter;
        public int Damage;
        public float Radius;

        public ProjInst(ProjDef def) : this()
        {
            this.Definition = def;
        }

        partial void pUpdatePos()
        {
            NPCInst target = null;

            var curPos = GetPosition();
            this.World.BaseWorld.ForEachNPCRoughPredicate(curPos, 10 * Radius, npc =>
              {
                  if (!npc.IsDead && npc != Shooter.BaseInst)
                  {
                      var npcPos = npc.GetPosition();
                      var npcModel = ((ModelDef)npc.ModelInstance.ScriptObject);
                      if (npcPos.GetDistancePlanar(curPos) <= Radius + npcModel.Radius
                          && Math.Abs(npcPos.Y - curPos.Y) <= Radius + npcModel.Height)
                      {
                          target = (NPCInst)npc.ScriptObject;
                          return true;
                      }
                  }
                  return false;
              });

            if (target != null)
            {
                target.Hit(this.Shooter, Damage);
                this.Despawn();
            }
        }

        partial void pOnEndPos()
        {
            ItemInst item = this.Item;
            if (item != null)
            {
                item.Spawn(this.World, this.Destination, this.GetAngles());
            }
        }
    }
}

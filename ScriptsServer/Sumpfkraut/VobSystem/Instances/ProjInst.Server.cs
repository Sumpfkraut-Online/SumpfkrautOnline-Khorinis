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
        public const int MaxProjectileItems = 500;

        public ItemInst Item { get { return (ItemInst)this.BaseInst.Item?.ScriptObject; } set { this.BaseInst.Item = value?.BaseInst; } }

        public NPCInst Shooter;
        public int Damage;

        public ProjInst(ProjDef def) : this()
        {
            this.Definition = def;
        }

        partial void pUpdatePos()
        {
            Vec3f curPos = GetPosition();

            if (curPos.GetDistance(BaseInst.StartPosition) > 100000)
            {
                this.Despawn();
                return;
            }

            if (DoHitDetection(BaseInst.LastPosition, curPos))
                this.Despawn();
        }

        bool DoHitDetection(Vec3f from, Vec3f to)
        {
            // fixme: expensive and also shitty check   

            if (DetectHit(from)) // check last position
                return true;

            float flownDist = from.GetDistance(to);
            if (flownDist < 20)
                return false;  // flew a really short distance since last check

            int interChecks = (int)(flownDist / GUCScripts.SmallestNPCRadius);
            if (interChecks > 1)
            {
                Vec3f dir = (to - from).Normalise();
                float inc = flownDist / interChecks;

                for (int i = 1; i < interChecks; i++)
                {
                    if (DetectHit(from + inc * dir))
                        return true;
                }
            }

            return DetectHit(to);
        }

        bool DetectHit(Vec3f position)
        {
            NPCInst target = null;
            this.World.BaseWorld.ForEachNPCRoughPredicate(position, GUCScripts.BiggestNPCRadius, baseNPC =>
            {
                NPCInst npc = (NPCInst)baseNPC.ScriptObject;
                if (!npc.IsDead && npc != Shooter 
                && NPCInst.AllowHitEvent.TrueForAll(Shooter, npc)
                && npc.AllowHitTarget.TrueForAll(Shooter, npc) && Shooter.AllowHitAttacker.TrueForAll(Shooter, npc))
                {
                    var modelDef = npc.ModelDef;

                    var npcPos = npc.GetPosition() + npc.BaseInst.GetAtVector() * npc.ModelDef.CenterOffset;

                    if (npcPos.GetDistancePlanar(position) <= modelDef.Radius
                        && Math.Abs(npcPos.Y - position.Y) <= modelDef.HalfHeight)
                    {
                        target = npc;
                        return false;
                    }
                }
                return true;
            });

            if (target != null)
            {
                target.Hit(Shooter, Damage);
                return true;
            }

            return false;
        }

        partial void pOnEndPos()
        {
            if (DoHitDetection(BaseInst.LastPosition, Destination))
                return;

            ItemInst item = this.Item;
            if (item != null)
            {
                item.Spawn(this.World, this.Destination, this.GetAngles());
                projDespawnList.AddVob(item);
            }
        }

        static DespawnList<ItemInst> projDespawnList = new DespawnList<ItemInst>(MaxProjectileItems);
    }
}

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
            this.World.BaseWorld.ForEachNPCRoughPredicate(position, GUCScripts.BiggestNPCRadius, npc =>
            {
                if (!npc.IsDead && npc != Shooter.BaseInst && NPCInst.AllowHit(Shooter, (NPCInst)npc.ScriptObject))
                {
                    var npcPos = npc.Position;
                    if (npc.ModelInstance.Visual == "CRAWLER.MDS") // fixme
                    {
                        npcPos += npc.GetAtVector() * 50;
                    }

                    var npcModel = ((ModelDef)npc.ModelInstance.ScriptObject);
                    if (npcPos.GetDistancePlanar(position) <= npcModel.Radius
                        && Math.Abs(npcPos.Y - position.Y) <= npcModel.HalfHeight)
                    {
                        target = (NPCInst)npc.ScriptObject;
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

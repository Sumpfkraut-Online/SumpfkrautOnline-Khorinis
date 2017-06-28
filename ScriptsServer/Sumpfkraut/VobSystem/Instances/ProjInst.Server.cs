using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ProjInst
    {
        protected override BaseEffectHandler CreateHandler()
        {
            throw new NotImplementedException();
        }

        public NPCInst Shooter;
        public int Damage;

        public ProjInst(ProjDef def) : this()
        {
            this.Definition = def;
        }

        public void UpdatePos(Vec3f newPos, Vec3f oldPos)
        {
            NPCInst target = null;
            this.World.BaseWorld.ForEachNPCRough(newPos, ModelDef.LargestNPC.Radius, npc =>
            {
                if (!npc.IsDead && npc != Shooter.BaseInst)
                    if (npc.GetPosition().GetDistance(newPos) - ((ModelDef)npc.Model.ScriptObject).Radius <= 0)
                    {
                        target = (NPCInst)npc.ScriptObject;
                    }
            });

            if (target != null)
            {
                var strm = this.BaseInst.GetScriptVobStream();
                strm.Write((byte)ScriptVobMessageIDs.HitMessage);
                strm.Write((ushort)target.ID);
                this.BaseInst.SendScriptVobStream(strm);

                //target.Hit(this.Shooter, Damage - (target.Armor == null ? 0 : target.Armor.Definition.Protection));

                this.Despawn();
            }
        }

        public void OnEndPos(Vec3f pos, Vec3f dir)
        {
            ItemDef arrow = ItemDef.Get<ItemDef>("itrw_arrow");
            float offset = 30.0f;
            if (this.Model != arrow.Model)
            {
                arrow = ItemDef.Get<ItemDef>("itrw_bolt");
                offset = 10.0f;
            }

            Vec3f realPos = pos - dir * offset;
            Vec3f realDir = dir.Cross(new Vec3f(0, -1, 0)).Normalise();

            ItemInst inst = new ItemInst(arrow);
            inst.Spawn(this.World, realPos, realDir); 
        }
    }
}

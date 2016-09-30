using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Left4Gothic
{
    partial class L4Client
    {
        private bool once = true;
        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessageIDs id = (ScriptMessageIDs)stream.ReadByte();
            switch (id)
            {
                case ScriptMessageIDs.CharCreation:
                    ReadCharCreation(stream);
                    break;
            }
        }

        partial void pOnConnect()
        {
            NPCInst npc = new NPCInst(NPCDef.Get("maleplayer"));
            if (once)
            {
                once = false;
                // for testing item dropping

                ModelDef m;
                if (!ModelDef.TryGetModel("itrw_arrow", out m))
                {
                    m = new ModelDef("itrw_arrow", "ItRw_Arrow.3ds");
                    m.Create();
                }
                ItemDef itemDef;
                itemDef = ItemDef.Get("itrw_arrow");
                if (itemDef == null)
                {
                    itemDef = new ItemDef("itrw_arrow");
                    itemDef.Name = "Pfeil";
                    itemDef.Material = ItemMaterials.Wood;
                    itemDef.ItemType = ItemTypes.AmmoBow;
                    itemDef.Damage = 5;
                    itemDef.Model = m;
                    itemDef.Create();
                    var projDef = new ProjDef("arrow");
                    projDef.Model = m;
                    projDef.Velocity = 0.0003f;
                    projDef.Create();
                }
                ItemInst newItem = new ItemInst(itemDef);
                newItem.SetAmount(200);
                npc.Inventory.AddItem(newItem);

                /* m = new ModelDef("itrw_arrow", "ItRw_Arrow.3ds");
                 m.Create();
                 itemDef = new ItemDef("itrw_arrow");
                 itemDef.Name = "Pfeil";
                 itemDef.Material = ItemMaterials.Wood;
                 itemDef.ItemType = ItemTypes.AmmoBow;
                 itemDef.Damage = 5;
                 itemDef.Model = m;
                 itemDef.Create();
                 var projDef = new ProjDef("arrow");
                 projDef.Model = m;
                 projDef.Velocity = 0.0003f;
                 projDef.Create();
                 newItem = new ItemInst(itemDef);
                 newItem.SetAmount(200);
                 npc.Inventory.AddItem(newItem); */
            }
            this.SetControl(npc);
            npc.Spawn(Sumpfkraut.WorldSystem.WorldInst.Current);
        }

        void ReadCharCreation(PacketReader stream)
        {
            CharacterInfo info = new CharacterInfo();
            info.Read(stream);
            
            NPCInst npc = new NPCInst(NPCDef.Get(info.BodyMesh == Sumpfkraut.Visuals.HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"));
            npc.UseCustoms = true;
            npc.CustomBodyTex = info.BodyTex;
            npc.CustomHeadMesh = info.HeadMesh;
            npc.CustomHeadTex = info.HeadTex;
            npc.CustomVoice = info.Voice;
            npc.CustomFatness = info.Fatness;
            npc.CustomScale = new Types.Vec3f(info.BodyWidth, 1.0f, info.BodyWidth);
            npc.CustomName = info.Name;
            
            this.SetControl(npc);
            npc.Spawn(Sumpfkraut.WorldSystem.WorldInst.Current);
            
        }
    }
}

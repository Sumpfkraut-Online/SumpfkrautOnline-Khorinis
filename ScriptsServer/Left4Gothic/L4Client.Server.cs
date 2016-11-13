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
    public class TestItems
    {
        private static bool defined = false;
        public static List<ItemDef> ItemDefs = new List<ItemDef>();
        public static void DefineItems()
        {
            if (defined) return;
            defined = true;

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
            ItemDefs.Add(itemDef);

            /*newItem = new ItemInst(itemDef);
            newItem.SetAmount(200);
            npc.Inventory.AddItem(newItem); */

            // SÖLDNERRÜSTUNG
            m = new ModelDef("ITAR_Söldner", "ItAr_Sld_M.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Söldner");
            itemDef.Name = "Söldnerrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Sld_M.asc";
            itemDef.Protection = 30;
            itemDef.Model = m;
            itemDef.Create();
            ItemDefs.Add(itemDef);

            // Breitschwert
            m = new ModelDef("1hschwert", "Itmw_025_1h_Mil_Sword_broad_01.3DS");
            m.Create();
            itemDef = new ItemDef("1hschwert");
            itemDef.Name = "Breitschwert";
            itemDef.ItemType = ItemTypes.Wep1H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 40;
            itemDef.Range = 90;
            itemDef.Create();
            ItemDefs.Add(itemDef);

            // GARDERÜSTUNG
            m = new ModelDef("ITAR_Garde", "ItAr_Bloodwyn_ADDON.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Garde");
            itemDef.Name = "Gardistenrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.Protection = 30;
            itemDef.VisualChange = "Armor_Bloodwyn_ADDON.asc";
            itemDef.Model = m;
            itemDef.Create();
            ItemDefs.Add(itemDef);

            // SCHATTENRÜSTUNG
            m = new ModelDef("ITAR_Schatten", "ItAr_Diego.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Schatten");
            itemDef.Name = "Schattenrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Diego.asc";
            itemDef.Protection = 27;
            itemDef.Model = m;
            itemDef.Create();
            ItemDefs.Add(itemDef);

            //ZWEIHAND AXT
            m = new ModelDef("2haxt", "ItMw_060_2h_axe_heavy_01.3DS");
            m.Create();
            itemDef = new ItemDef("2haxt");
            itemDef.Name = "Söldneraxt";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 44;
            itemDef.Range = 95;
            itemDef.Create();
            ItemDefs.Add(itemDef);


            // Apfel
            m = new ModelDef("itfoapple", "ItFo_Apple.3ds");
            m.Create();
            itemDef = new ItemDef("itfoapple");
            itemDef.Name = "Bester Apfel";
            itemDef.ItemType = ItemTypes.SmallEatable;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            ItemDefs.Add(itemDef);

            // Cheese
            m = new ModelDef("itfocheese", "ItFo_Cheese.3ds");
            m.Create();
            itemDef = new ItemDef("itfocheese");
            itemDef.Name = "Alter Käse";
            itemDef.ItemType = ItemTypes.LargeEatable;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            ItemDefs.Add(itemDef);

            //  Readable object
            m = new ModelDef("itwrscroll", "ItWr_Scroll_01.3ds");
            m.Create();
            itemDef = new ItemDef("itwrscroll");
            itemDef.Name = "Lesbar";
            itemDef.ItemType = ItemTypes.Readable;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            ItemDefs.Add(itemDef);

            // WATER

            m = new ModelDef("itfowater", "ItFo_Water.3ds");
            m.Create();
            itemDef = new ItemDef("itfowater");
            itemDef.Name = "Ich bin ein Wasser";
            itemDef.ItemType = ItemTypes.Drinkable;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            ItemDefs.Add(itemDef);

            //torch ItLs_Torch_01.3ds
            m = new ModelDef("itlstorchx", "ItLs_Torch_01.3ds");
            m.Create();
            itemDef = new ItemDef("itlstorchx");
            itemDef.Name = "Ne fagel";
            itemDef.ItemType = ItemTypes.Torch;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            ItemDefs.Add(itemDef);
        }

    }

    partial class L4Client
    {
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
            TestItems.DefineItems();
            for(int i = 0; i < TestItems.ItemDefs.Count; i++)
            { npc.Inventory.AddItem(new ItemInst(TestItems.ItemDefs[i])); }
            // for testing item dropping
            /*
            ModelDef m;
            ItemInst newItem;
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
            newItem = new ItemInst(itemDef);
            newItem.SetAmount(200);
            npc.Inventory.AddItem(newItem);

            // SÖLDNERRÜSTUNG
            m = new ModelDef("ITAR_Söldner", "ItAr_Sld_M.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Söldner");
            itemDef.Name = "Söldnerrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Sld_M.asc";
            itemDef.Protection = 30;
            itemDef.Model = m;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);

            // Breitschwert
            m = new ModelDef("1hschwert", "Itmw_025_1h_Mil_Sword_broad_01.3DS");
            m.Create();
            itemDef = new ItemDef("1hschwert");
            itemDef.Name = "Breitschwert";
            itemDef.ItemType = ItemTypes.Wep1H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 40;
            itemDef.Range = 90;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);

            // GARDERÜSTUNG
            m = new ModelDef("ITAR_Garde", "ItAr_Bloodwyn_ADDON.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Garde");
            itemDef.Name = "Gardistenrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.Protection = 30;
            itemDef.VisualChange = "Armor_Bloodwyn_ADDON.asc";
            itemDef.Model = m;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);

            // SCHATTENRÜSTUNG
            m = new ModelDef("ITAR_Schatten", "ItAr_Diego.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Schatten");
            itemDef.Name = "Schattenrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Diego.asc";
            itemDef.Protection = 27;
            itemDef.Model = m;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);

            //ZWEIHAND AXT
            m = new ModelDef("2haxt", "ItMw_060_2h_axe_heavy_01.3DS");
            m.Create();
            itemDef = new ItemDef("2haxt");
            itemDef.Name = "Söldneraxt";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 44;
            itemDef.Range = 95;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);


            // Apfel
            m = new ModelDef("itfoapple", "ItFo_Apple.3ds");
            m.Create();
            itemDef = new ItemDef("itfoapple");
            itemDef.Name = "Bester Apfel";
            itemDef.ItemType = ItemTypes.SmallEatable;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);

            // Cheese
            m = new ModelDef("itfocheese", "ItFo_Cheese.3ds");
            m.Create();
            itemDef = new ItemDef("itfocheese");
            itemDef.Name = "Alter Käse";
            itemDef.ItemType = ItemTypes.LargeEatable;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);

            //  Readable object
            m = new ModelDef("itwrscroll", "ItWr_Scroll_01.3ds");
            m.Create();
            itemDef = new ItemDef("itwrscroll");
            itemDef.Name = "Lesbar";
            itemDef.ItemType = ItemTypes.Readable;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);

            // WATER

            m = new ModelDef("itfowater", "ItFo_Water.3ds");
            m.Create();
            itemDef = new ItemDef("itfowater");
            itemDef.Name = "Ich bin ein Wasser";
            itemDef.ItemType = ItemTypes.Drinkable;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);

            //torch ItLs_Torch_01.3ds
            m = new ModelDef("itlstorchx", "ItLs_Torch_01.3ds");
            m.Create();
            itemDef = new ItemDef("itlstorchx");
            itemDef.Name = "Ne fagel";
            itemDef.ItemType = ItemTypes.Torch;
            itemDef.Material = ItemMaterials.Clay;
            itemDef.Model = m;
            itemDef.Create();
            newItem = new ItemInst(itemDef);
            npc.Inventory.AddItem(newItem);


            
             //ZWEIHANDER
        ModelDef m = new ModelDef("2hschwert", "ItMw_060_2h_sword_01.3DS");
        m.Create();
        ItemDef itemDef = new ItemDef("2hschwert");
        itemDef.Name = "Zweihänder";
        itemDef.ItemType = ItemTypes.Wep2H;
        itemDef.Material = ItemMaterials.Metal;
        itemDef.Model = m;
        itemDef.Range = 110;
        itemDef.Damage = 42;
        itemDef.Create();

        // GARDERÜSTUNG
        m = new ModelDef("ITAR_Garde", "ItAr_Bloodwyn_ADDON.3ds");
        m.Create();
        itemDef = new ItemDef("ITAR_Garde");
        itemDef.Name = "Gardistenrüstung";
        itemDef.Material = ItemMaterials.Leather;
        itemDef.ItemType = ItemTypes.Armor;
        itemDef.Protection = 30;
        itemDef.VisualChange = "Armor_Bloodwyn_ADDON.asc";
        itemDef.Model = m;
        itemDef.Create();

        //EINHANDER
        m = new ModelDef("1hschwert", "Itmw_025_1h_Mil_Sword_broad_01.3DS");
        m.Create();
        itemDef = new ItemDef("1hschwert");
        itemDef.Name = "Breitschwert";
        itemDef.ItemType = ItemTypes.Wep1H;
        itemDef.Material = ItemMaterials.Metal;
        itemDef.Model = m;
        itemDef.Damage = 40;
        itemDef.Range = 90;
        itemDef.Create();

        // SCHATTENRÜSTUNG
        m = new ModelDef("ITAR_Schatten", "ItAr_Diego.3ds");
        m.Create();
        itemDef = new ItemDef("ITAR_Schatten");
        itemDef.Name = "Schattenrüstung";
        itemDef.Material = ItemMaterials.Leather;
        itemDef.ItemType = ItemTypes.Armor;
        itemDef.VisualChange = "Armor_Diego.asc";
        itemDef.Protection = 27;
        itemDef.Model = m;
        itemDef.Create();

        //ZWEIHAND AXT
        m = new ModelDef("2haxt", "ItMw_060_2h_axe_heavy_01.3DS");
        m.Create();
        itemDef = new ItemDef("2haxt");
        itemDef.Name = "Söldneraxt";
        itemDef.ItemType = ItemTypes.Wep2H;
        itemDef.Material = ItemMaterials.Metal;
        itemDef.Model = m;
        itemDef.Damage = 44;
        itemDef.Range = 95;
        itemDef.Create();

        //EINHAND AXT
        m = new ModelDef("1haxt", "ItMw_025_1h_sld_axe_01.3DS");
        m.Create();
        itemDef = new ItemDef("1haxt");
        itemDef.Name = "Grobes Kriegsbeil";
        itemDef.ItemType = ItemTypes.Wep1H;
        itemDef.Material = ItemMaterials.Wood;
        itemDef.Damage = 42;
        itemDef.Model = m;
        itemDef.Range = 75;
        itemDef.Create();

        // BANDITENRÜSTUNG
        m = new ModelDef("ITAR_bandit", "ItAr_Bdt_H.3ds");
        m.Create();
        itemDef = new ItemDef("ITAR_bandit");
        itemDef.Name = "Banditenrüstung";
        itemDef.Material = ItemMaterials.Leather;
        itemDef.ItemType = ItemTypes.Armor;
        itemDef.VisualChange = "Armor_Bdt_H.asc";
        itemDef.Protection = 27;
        itemDef.Model = m;
        itemDef.Create();

        // PFEIL
        m = new ModelDef("itrw_arrow", "ItRw_Arrow.3ds");
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

        // LANGBOGEN
        m = new ModelDef("itrw_longbow", "ItRw_Bow_M_01.mms");
        m.Create();
        itemDef = new ItemDef("itrw_longbow");
        itemDef.Name = "Langbogen";
        itemDef.Material = ItemMaterials.Wood;
        itemDef.ItemType = ItemTypes.WepBow;
        itemDef.Damage = 32;
        itemDef.Model = m;
        itemDef.Create();

        // BOLZEN
        m = new ModelDef("itrw_bolt", "ItRw_Bolt.3ds");
        m.Create();
        itemDef = new ItemDef("itrw_Bolt");
        itemDef.Name = "Bolzen";
        itemDef.Material = ItemMaterials.Wood;
        itemDef.ItemType = ItemTypes.AmmoXBow;
        itemDef.Damage = 6;
        itemDef.Model = m;
        itemDef.Create();

        projDef = new ProjDef("bolt");
        projDef.Model = m;
        projDef.Velocity = 0.0003f;
        projDef.Create();

        // ARMBRUST
        m = new ModelDef("itrw_crossbow", "ItRw_Crossbow_L_01.mms");
        m.Create();
        itemDef = new ItemDef("itrw_crossbow");
        itemDef.Name = "Armbrust";
        itemDef.Material = ItemMaterials.Wood;
        itemDef.ItemType = ItemTypes.WepXBow;
        itemDef.Damage = 32;
        itemDef.Model = m;
        itemDef.Create();
              */

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

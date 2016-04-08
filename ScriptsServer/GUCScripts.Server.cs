using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using GUC.Log;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public GUCScripts()
        {
            Logger.Log("######## Initalise SumpfkrautOnline ServerScripts #########");

            AddSomeDefs();

            CreateTestWorld();

            Logger.Log("######################## Finished #########################");
        }

        void AddSomeDefs()
        {
            AddItems();

            // HUMAN MODEL
            var m = new ModelDef("human", "humans.mds");
            m.Radius = 30;
            m.Height = 200;
            
            Add2hAttacks(m);

            m.Create();

            // NPCs

            NPCDef npcDef = new NPCDef("player");
            npcDef.Name = "Spieler";
            npcDef.Model = m;
            npcDef.BodyMesh = Enumeration.HumBodyMeshs.HUM_BODY_NAKED0.ToString();
            npcDef.BodyTex = (int)Enumeration.HumBodyTexs.G1Hero;
            npcDef.HeadMesh = Enumeration.HumHeadMeshs.HUM_HEAD_PONY.ToString();
            npcDef.HeadTex = (int)Enumeration.HumHeadTexs.Face_N_Player;
            npcDef.Create();
        }

        void CreateTestWorld()
        {
            WorldDef wDef = new WorldDef();
            WorldInst.Current = new WorldInst(default(WorldDef));
            
            WorldInst.Current.Create();
            WorldInst.Current.Clock.SetTime(new Types.WorldTime(0, 8), 5.0f);
            WorldInst.Current.Clock.Start();
        }

        void Add2hAttacks(ModelDef model)
        {
            var ov1 = new ScriptOverlay("2HST1", "Humans_2hST1"); model.AddOverlay(ov1);
            var ov2 = new ScriptOverlay("2HST2", "Humans_2hST2"); model.AddOverlay(ov2);

            // 2h COMBO 1
            ScriptAniJob aniJob = new ScriptAniJob("attack2hfwd1");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HFwd1;
            aniJob.AniName = "s_2hAttack";
            model.AddAniJob(aniJob);

            var ani = new ScriptAni(10000000); ani.ComboTime = 5800000; ani.HitTime = 2800000; aniJob.SetDefaultAni(ani);
            ani = new ScriptAni(13000000); ani.ComboTime = 6000000; ani.HitTime = 2000000; aniJob.AddOverlayAni(ani, ov1);
            ani = new ScriptAni(13000000); ani.ComboTime = 4800000; ani.HitTime = 1600000; aniJob.AddOverlayAni(ani, ov2);
            
            // 2h COMBO 2
            aniJob = new ScriptAniJob("attack2hfwd2");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HFwd2;
            aniJob.AniName = "s_2hAttack";
            model.AddAniJob(aniJob);

            ani = new ScriptAni(6000000); ani.StartFrame = 31; ani.ComboTime = 4400000; ani.HitTime = 2300000; aniJob.SetDefaultAni(ani);
            ani = new ScriptAni(14000000); ani.StartFrame = 40; ani.ComboTime = 8000000; ani.HitTime = 4000000; aniJob.AddOverlayAni(ani, ov1);
            ani = new ScriptAni(11500000); ani.StartFrame = 41; ani.ComboTime = 6800000; ani.HitTime = 3600000; aniJob.AddOverlayAni(ani, ov2);


            // 2h COMBO 3
            aniJob = new ScriptAniJob("attack2hfwd3");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HFwd3;
            aniJob.AniName = "s_2hAttack";
            model.AddAniJob(aniJob);

            ani = new ScriptAni(7000000); ani.StartFrame = 80; ani.ComboTime = 7000000; ani.HitTime = 4000000; aniJob.AddOverlayAni(ani, ov1);
            ani = new ScriptAni(13500000); ani.StartFrame = 81; ani.ComboTime = 8800000; ani.HitTime = 5600000; aniJob.AddOverlayAni(ani, ov2);
            
            // 2h COMBO 4
            aniJob = new ScriptAniJob("attack2hfwd4");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HFwd4;
            aniJob.AniName = "s_2hAttack";
            model.AddAniJob(aniJob);

            ani = new ScriptAni(7500000); ani.StartFrame = 126; ani.ComboTime = 7500000; ani.HitTime = 6800000; aniJob.AddOverlayAni(ani, ov2);
            
            // 2h LEFT ATTACK
            aniJob = new ScriptAniJob("attack2hleft");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HLeft;
            aniJob.AniName = "t_2hAttackL";
            model.AddAniJob(aniJob);

            ani = new ScriptAni(14000000); ani.ComboTime = 7200000; ani.HitTime = 2400000; aniJob.SetDefaultAni(ani);
            ani = new ScriptAni(10700000); ani.ComboTime = 5600000; ani.HitTime = 2000000; aniJob.AddOverlayAni(ani, ov1);
            ani = new ScriptAni(10200000); ani.ComboTime = 5600000; ani.HitTime = 2000000; aniJob.AddOverlayAni(ani, ov2);


            // 2h RIGHT ATTACK
            aniJob = new ScriptAniJob("attack2hright");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HRight;
            aniJob.AniName = "t_2hAttackR";
            model.AddAniJob(aniJob);

            ani = new ScriptAni(14000000); ani.ComboTime = 7200000; ani.HitTime = 2400000; aniJob.SetDefaultAni(ani);
            ani = new ScriptAni(11600000); ani.ComboTime = 5600000; ani.HitTime = 2000000; aniJob.AddOverlayAni(ani, ov1);
            ani = new ScriptAni(10600000); ani.ComboTime = 5600000; ani.HitTime = 2000000; aniJob.AddOverlayAni(ani, ov2);

            // 2h RUN ATTACK
            aniJob = new ScriptAniJob("attack2hrun");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HRun;
            aniJob.AniName = "t_2hAttackMove";
            model.AddAniJob(aniJob);

            ani = new ScriptAni(8800000); ani.ComboTime = 8800000; ani.HitTime = 6000000; aniJob.SetDefaultAni(ani);
            
            // 2h Parry
            aniJob = new ScriptAniJob("attack2hparry");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HParry;
            aniJob.AniName = "T_2HPARADE_0";
            model.AddAniJob(aniJob);

            ani = new ScriptAni(5600000); ani.ComboTime = 5600000; aniJob.SetDefaultAni(ani);
            
            // 2h Dodge
            aniJob = new ScriptAniJob("attack2hdodge");
            aniJob.BaseAniJob.ID = (int)SetAnis.Attack2HDodge;
            aniJob.AniName = "T_2HPARADEJUMPB";
            model.AddAniJob(aniJob);

            ani = new ScriptAni(9200000); ani.ComboTime = 9200000; aniJob.SetDefaultAni(ani);
        }

        void AddItems()
        {
            //ZWEIHANDER
            ModelDef m = new ModelDef("2hschwert", "ItMw_060_2h_sword_01.3DS");
            m.Create();
            ItemDef itemDef = new ItemDef("2hschwert");
            itemDef.Name = "Zweihänder";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = Enumeration.ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Range = 100;
            itemDef.Create();

            // GARDERÜSTUNG
            m = new ModelDef("ITAR_Garde", "ItAr_Bloodwyn_ADDON.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Garde");
            itemDef.Name = "Gardistenrüstung";
            itemDef.Material = Enumeration.ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Bloodwyn_ADDON.asc";
            itemDef.Model = m;
            itemDef.Create();

            //EINHANDER
            m = new ModelDef("1hschwert", "Itmw_025_1h_Mil_Sword_broad_01.3DS");
            m.Create();
            itemDef = new ItemDef("1hschwert");
            itemDef.Name = "Breitschwert";
            itemDef.ItemType = ItemTypes.Wep1H;
            itemDef.Material = Enumeration.ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Range = 70;
            itemDef.Create();

            // SCHATTENRÜSTUNG
            m = new ModelDef("ITAR_Schatten", "ItAr_Diego.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Schatten");
            itemDef.Name = "Schattenrüstung";
            itemDef.Material = Enumeration.ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Diego.asc";
            itemDef.Model = m;
            itemDef.Create();

            //ZWEIHAND AXT
            m = new ModelDef("2haxt", "ItMw_060_2h_axe_heavy_01.3DS");
            m.Create();
            itemDef = new ItemDef("2haxt");
            itemDef.Name = "Söldneraxt";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = Enumeration.ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Range = 90;
            itemDef.Create();

            // SÖLDNERRÜSTUNG
            m = new ModelDef("ITAR_Söldner", "ItAr_Sld_M.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Söldner");
            itemDef.Name = "Söldnerrüstung";
            itemDef.Material = Enumeration.ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Sld_M.asc";
            itemDef.Model = m;
            itemDef.Create();

            //EINHAND AXT
            m = new ModelDef("1haxt", "ItMw_025_1h_sld_axe_01.3DS");
            m.Create();
            itemDef = new ItemDef("1haxt");
            itemDef.Name = "Grobes Kriegsbeil";
            itemDef.ItemType = ItemTypes.Wep1H;
            itemDef.Material = Enumeration.ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Range = 60;
            itemDef.Create();

            // BANDITENRÜSTUNG
            m = new ModelDef("ITAR_bandit", "ItAr_Bdt_H.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_bandit");
            itemDef.Name = "Banditenrüstung";
            itemDef.Material = Enumeration.ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Bdt_H.asc";
            itemDef.Model = m;
            itemDef.Create();
        }
    }
}

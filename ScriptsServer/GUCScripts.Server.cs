using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using GUC.Log;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Utilities;
using GUC.Types;
using GUC.WorldObjects;

namespace GUC.Scripts
{
    #region
    interface IProbabilityItems
    {
        List<ItemInst> GetItems();
    }

    class ProbItemGroup : IProbabilityItems
    {
        public struct BucketPair
        {
            public IProbabilityItems Bucket;
            public float Probability;
        }

        public List<BucketPair> pairs = new List<BucketPair>();

        public void Add(IProbabilityItems item, float probability)
        {
            BucketPair pair = new BucketPair();
            pair.Bucket = item;
            pair.Probability = probability;
            pairs.Add(pair);
        }

        public virtual List<ItemInst> GetItems()
        {
            List<ItemInst> result = new List<ItemInst>();
            foreach (BucketPair pair in pairs)
            {
                if (pair.Probability == 1.0f || Randomizer.GetDouble() >= pair.Probability)
                {
                    result.AddRange(pair.Bucket.GetItems());
                }
            }
            return result;
        }
    }

    class ProbItemGroupSingle : ProbItemGroup  // gibt ein item von vielen zurück
    {
        public override List<ItemInst> GetItems()
        {
            double prob = Randomizer.GetDouble();
            foreach (BucketPair pair in pairs)
            {
                if (prob < pair.Probability)
                    return pair.Bucket.GetItems();
                prob -= pair.Probability;
            }
            throw new Exception("Sum of all probabilities is < 1!");
        }
    }

    class ProbItem : IProbabilityItems // gibt ein item zufälliger anzahl zurück
    {
        public ItemDef itemDef;

        public int minAmount = 1; // 1 bis maxAmount
        public int maxAmount = 1; // minAmount bis unendlich
        public double exponent = 1; // Exponent der Wahrscheinlichkeit für exponentielle Verteilung

        public ProbItem(ItemDef def, int min = 1, int max = 1, double expo = 1)
        {
            this.itemDef = def;
            this.minAmount = min;
            this.maxAmount = max;
            this.exponent = expo;
        }

        public List<ItemInst> GetItems()
        {
            int amount;
            int diff = maxAmount - minAmount;
            if (diff > 0)
            {
                amount = minAmount + (int)(diff * Math.Pow(Randomizer.GetDouble(), exponent));
            }
            else
            {
                amount = minAmount;
            }

            ItemInst inst = new ItemInst(itemDef);
            inst.SetAmount(amount);
            return new List<ItemInst>() { inst };
        }
    }
    #endregion

    public partial class GUCScripts : ScriptInterface
    {
        public WorldObjects.VobGuiding.TargetCmd GetTestCmd(BaseVob target)
        {
            return new Sumpfkraut.AI.GuideCommands.GoToVobCommand((BaseVobInst)target.ScriptObject);
        }

        partial void pConstruct()
        {

            Logger.Log("######## Initalise SumpfkrautOnline ServerScripts #########");

            //Sumpfkraut.EffectSystem.Changes.ChangeInitializer.Init();
            //Sumpfkraut.EffectSystem.Destinations.DestInitializer.Init();

            //Sumpfkraut.Daedalus.AniParser.ReadMDSFiles();
            /*Sumpfkraut.Daedalus.ConstParser.ParseConstValues();
            Sumpfkraut.Daedalus.FuncParser.ParseConstValues();
            Sumpfkraut.Daedalus.PrototypeParser.ParsePrototypes();
            Sumpfkraut.Daedalus.InstanceParser.ParseInstances();

            Sumpfkraut.Daedalus.InstanceParser.AddInstances();

            Sumpfkraut.Daedalus.ConstParser.Free();
            Sumpfkraut.Daedalus.FuncParser.Free();
            Sumpfkraut.Daedalus.PrototypeParser.Free();
            Sumpfkraut.Daedalus.InstanceParser.Free();*/

            NPCInst.Requests.OnJump += (npc, move) => npc.EffectHandler.TryJump(move);
            NPCInst.Requests.OnClimb += (npc, move, ledge) => npc.EffectHandler.TryClimb(move, ledge);
            NPCInst.Requests.OnDrawFists += npc => npc.EffectHandler.TryDrawFists();
            NPCInst.Requests.OnDrawWeapon += (npc, item) => npc.EffectHandler.TryDrawWeapon(item);
            NPCInst.Requests.OnFightMove += (npc, move) => npc.EffectHandler.TryFightMove(move);
            NPCInst.Requests.OnEquipItem += (npc, item) => npc.EffectHandler.TryEquipItem(item);
            NPCInst.Requests.OnUnequipItem += (npc, item) => npc.EffectHandler.TryUnequipItem(item);

            AddSomeDefs();

            // -- Websocket-Server --
            Sumpfkraut.Web.WS.WSServer wsServer = new Sumpfkraut.Web.WS.WSServer();
            wsServer.Init();
            wsServer.Start();

            // -- command console --
            Sumpfkraut.CommandConsole.CommandConsole cmdConsole = new Sumpfkraut.CommandConsole.CommandConsole();
            
            Sumpfkraut.TestingThings.Init();
            //Sumpfkraut.AI.TestingAI.Test();

            CreateTestWorld();
            Arena.DuelMode.Init();
            Arena.Regeneration.Init();

            Logger.Log("######################## Finished #########################");
        }

        void CreateTestWorld()
        {
            var world = new WorldInst(null);
            world.Path = "G1-OLDCAMP.ZEN";
            world.Create();
            world.Clock.SetTime(new WorldTime(0, 8), 15.0f);
            world.Clock.Start();
            WorldInst.List.Add(world);
            
            world = new WorldInst(null);
            world.Path = "G1-OLDMINE.ZEN";
            world.Create();
            world.Clock.SetTime(new WorldTime(0, 12), 1.0f);
            world.Clock.Stop();
            world.Barrier.StopTimer();
            world.Weather.StopRainTimer();
            WorldInst.List.Add(world);

            world = new WorldInst(null);
            world.Path = "G2-PASS.ZEN";
            world.Create();
            world.Clock.SetTime(new WorldTime(0, 20), 1.0f);
            world.Clock.Stop();
            world.Barrier.StopTimer();
            world.Weather.StopRainTimer();
            world.Weather.SetNextWeight(world.Clock.Time, 1.0f);
            WorldInst.List.Add(world);

            world = new WorldInst(null);
            world.Path = "ADDON-TEMPLE.ZEN";
            world.Create();
            world.Clock.SetTime(new WorldTime(0, 12), 15.0f);
            world.Clock.Start();
            world.Barrier.StopTimer();
            WorldInst.List.Add(world);
        }

        void AddSomeDefs()
        {
            // HUMAN MODEL
            ModelDef m = new ModelDef("humans", "HUMANS.MDS");
            m.SetAniCatalog(new Sumpfkraut.Visuals.AniCatalogs.NPCCatalog());
            AddFistAnis(m);
            Add1HAnis(m);
            Add2hAnis(m);
            AddJumpAnis(m);
            AddClimbAnis(m);

            m.Radius = 80;
            m.Height = 180;
            m.FistRange = 40;
            m.Create();

            //Sumpfkraut.TestingThings.TestLoadHumanModelDef();
            //ModelDef m;
            //ModelDef.TryGetModel("humans", out m);


            // NPCs
            NPCDef npcDef = new NPCDef("maleplayer");
            npcDef.Name = "Spieler";
            npcDef.Model = m;
            npcDef.BodyMesh = HumBodyMeshs.HUM_BODY_NAKED0.ToString();
            npcDef.BodyTex = (int)HumBodyTexs.G1Hero;
            npcDef.HeadMesh = HumHeadMeshs.HUM_HEAD_PONY.ToString();
            npcDef.HeadTex = (int)HumHeadTexs.Face_N_Player;
            npcDef.Create();

            npcDef = new NPCDef("femaleplayer");
            npcDef.Name = "Spielerin";
            npcDef.Model = m;
            npcDef.BodyMesh = HumBodyMeshs.HUM_BODY_BABE0.ToString();
            npcDef.BodyTex = (int)HumBodyTexs.F_Babe1;
            npcDef.HeadMesh = HumHeadMeshs.HUM_HEAD_BABE.ToString();
            npcDef.HeadTex = (int)HumHeadTexs.FaceBabe_N_Anne;
            npcDef.Create();
            
            npcDef = new NPCDef("skeleton");
            npcDef.Name = "Skelett";
            npcDef.Model = m;
            npcDef.BodyMesh = "Ske_Body";
            npcDef.BodyTex = 0;
            npcDef.HeadMesh = "";
            npcDef.HeadTex = 0;
            npcDef.Create();

            AddItems();

            AddCrawlers();
            AddOrcs();
        }

        #region Items

        void AddItems()
        {
            // TEMPLER MINE

            ModelDef m = new ModelDef("leichter_zweihaender", "ItMw_032_2h_sword_light_01.3DS");
            m.Create();
            ItemDef itemDef = new ItemDef("leichter_zweihaender");
            itemDef.Name = "Leichter Zweihänder";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Range = 100;
            itemDef.Damage = 50;
            itemDef.Create();

            m = new ModelDef("ITAR_templer", "ARMOR_TPLM.3DS");
            m.Create();
            itemDef = new ItemDef("ITAR_templer");
            itemDef.Name = "Templerrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.Protection = 30;
            itemDef.VisualChange = "ARMOR_TPLM.ASC";
            itemDef.Model = m;
            itemDef.Create();

            // GARDIST MINE

            m = new ModelDef("grobes_schwert", "ItMw_025_1h_sld_sword_01.3DS");
            m.Create();
            itemDef = new ItemDef("grobes_schwert");
            itemDef.Name = "Grobes Schwert";
            itemDef.ItemType = ItemTypes.Wep1H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 45;
            itemDef.Range = 65;
            itemDef.Create();

            m = new ModelDef("ITAR_garde_l", "ARMOR_GRDL.3DS");
            m.Create();
            itemDef = new ItemDef("ITAR_garde_l");
            itemDef.Name = "Leichte Garderüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.Protection = 40;
            itemDef.VisualChange = "ARMOR_GRDL.ASC";
            itemDef.Model = m;
            itemDef.Create();

            // GARDIST BURG

            m = new ModelDef("2hschwert", "ItMw_060_2h_sword_01.3DS");
            m.Create();
            itemDef = new ItemDef("2hschwert");
            itemDef.Name = "Zweihänder";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Range = 100;
            itemDef.Damage = 50;
            itemDef.Create();
            
            m = new ModelDef("ITAR_Garde", "ItAr_Bloodwyn_ADDON.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Garde");
            itemDef.Name = "Gardistenrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.Protection = 40;
            itemDef.VisualChange = "Armor_Bloodwyn_ADDON.asc";
            itemDef.Model = m;
            itemDef.Create();

            // SCHATTEN BURG

            m = new ModelDef("1hschwert", "Itmw_025_1h_Mil_Sword_broad_01.3DS");
            m.Create();
            itemDef = new ItemDef("1hschwert");
            itemDef.Name = "Breitschwert";
            itemDef.ItemType = ItemTypes.Wep1H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 45;
            itemDef.Range = 80;
            itemDef.Create();
            
            m = new ModelDef("ITAR_Schatten", "ItAr_Diego.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Schatten");
            itemDef.Name = "Schattenrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Diego.asc";
            itemDef.Protection = 30;
            itemDef.Model = m;
            itemDef.Create();

            // SÖLDNER BURG

            m = new ModelDef("2haxt", "ItMw_060_2h_axe_heavy_01.3DS");
            m.Create();
            itemDef = new ItemDef("2haxt");
            itemDef.Name = "Söldneraxt";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 50;
            itemDef.Range = 100;
            itemDef.Create();
            
            m = new ModelDef("ITAR_Söldner", "ItAr_Sld_M.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Söldner");
            itemDef.Name = "Söldnerrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Sld_M.asc";
            itemDef.Protection = 40;
            itemDef.Model = m;
            itemDef.Create();

            // BANDIT BURG

            m = new ModelDef("1haxt", "ItMw_025_1h_sld_axe_01.3DS");
            m.Create();
            itemDef = new ItemDef("1haxt");
            itemDef.Name = "Grobes Kriegsbeil";
            itemDef.ItemType = ItemTypes.Wep1H;
            itemDef.Material = ItemMaterials.Wood;
            itemDef.Damage = 45;
            itemDef.Model = m;
            itemDef.Range = 80;
            itemDef.Create();
            
            m = new ModelDef("ITAR_bandit", "ItAr_Bdt_H.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_bandit");
            itemDef.Name = "Banditenrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Bdt_H.asc";
            itemDef.Protection = 40;
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

            /*var projDef = new ProjDef("arrow");
            projDef.Model = m;
            projDef.Velocity = 0.0003f;
            projDef.Create();*/

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

            /*projDef = new ProjDef("bolt");
            projDef.Model = m;
            projDef.Velocity = 0.0003f;
            projDef.Create();*/

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


            // HOSE
            m = new ModelDef("ITAR_Prisoner", "ItAr_Prisoner.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_Prisoner");
            itemDef.Name = "Malaks letzte Hose";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.Protection = 5;
            itemDef.VisualChange = "Armor_Prisoner.asc";
            itemDef.Model = m;
            itemDef.Create();

            // SCHWERER AST
            m = new ModelDef("ItMw_1h_Bau_Mace", "ItMw_010_1h_Club_01.3DS");
            m.Create();
            itemDef = new ItemDef("ItMw_1h_Bau_Mace");
            itemDef.Name = "Sehr schwerer Ast";
            itemDef.ItemType = ItemTypes.Wep1H;
            itemDef.Material = ItemMaterials.Wood;
            itemDef.Model = m;
            itemDef.Damage = 10;
            itemDef.Range = 40;
            itemDef.Create();
            
            // ORK WAFFEN
            m = new ModelDef("krush_pach", "ItMw_2H_OrcAxe_02.3DS");
            m.Create();
            itemDef = new ItemDef("krush_pach");
            itemDef.Name = "Krush Pach";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 50;
            itemDef.Range = 80;
            itemDef.Create();

            m = new ModelDef("orc_sword", "ItMw_2H_OrcSword_02.3DS");
            m.Create();
            itemDef = new ItemDef("orc_sword");
            itemDef.Name = "Orkisches Kriegsschwert";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 45;
            itemDef.Range = 100;
            itemDef.Create();

            // Miliz
            m = new ModelDef("ITAR_miliz_s", "ItAr_MIL_M.3DS");
            m.Create();
            itemDef = new ItemDef("ITAR_miliz_s");
            itemDef.Name = "Schwere Milizrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_MIL_M.asc";
            itemDef.Protection = 30;
            itemDef.Model = m;
            itemDef.Create();

            // Ritter
            m = new ModelDef("ITAR_ritter", "ItAr_Pal_M.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_ritter");
            itemDef.Name = "Ritterrüstung";
            itemDef.Material = ItemMaterials.Metal;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Pal_M.asc";
            itemDef.Protection = 40;
            itemDef.Model = m;
            itemDef.Create();


            // Tempel
            m = new ModelDef("ITAR_bandit_m", "ItAr_Bdt_M.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_bandit_m");
            itemDef.Name = "mittlere Banditenrüstung";
            itemDef.Material = ItemMaterials.Leather;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Bdt_M.asc";
            itemDef.Protection = 30;
            itemDef.Model = m;
            itemDef.Create();

            m = new ModelDef("grober_2h", "ItMw_035_2h_sld_sword_01.3DS");
            m.Create();
            itemDef = new ItemDef("grober_2h");
            itemDef.Name = "Grober Zweihänder";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Material = ItemMaterials.Metal;
            itemDef.Model = m;
            itemDef.Damage = 50;
            itemDef.Range = 100;
            itemDef.Create();

            m = new ModelDef("ITAR_pal_skel", "ItAr_Pal_H.3ds");
            m.Create();
            itemDef = new ItemDef("ITAR_pal_skel");
            itemDef.Name = "Alte Paladinrüstung";
            itemDef.Material = ItemMaterials.Metal;
            itemDef.ItemType = ItemTypes.Armor;
            itemDef.VisualChange = "Armor_Pal_Skeleton.asc";
            itemDef.Protection = 40;
            itemDef.Model = m;
            itemDef.Create();
        }

        #endregion

        #region Jump Anis

        void AddJumpAnis(ModelDef model)
        {
            model.AddAniJob(new ScriptAniJob("jump_fwd", "t_Stand_2_Jump"));
            model.AddAniJob(new ScriptAniJob("jump_run", "t_RunL_2_Jump"));
            model.AddAniJob(new ScriptAniJob("jump_up", "t_Stand_2_JumpUp"));
        }

        #endregion

        #region Climb Anis

        void AddClimbAnis(ModelDef model)
        {
            var ani1 = new ScriptAniJob("climb_low", "t_Stand_2_JumpUpLow", new ScriptAni(0, 4));
            var ani2 = new ScriptAniJob("climb_low1", "s_JumpUpLow", new ScriptAni(0, 1));
            var ani3 = new ScriptAniJob("climb_low2", "t_JumpUpLow_2_Stand", new ScriptAni(0, 9));
            
            model.AddAniJob(ani1);
            model.AddAniJob(ani2);
            model.AddAniJob(ani3);

            ani1.NextAni = ani2;
            ani2.NextAni = ani3;

            ani1 = new ScriptAniJob("climb_mid", "t_Stand_2_JumpUpMid", new ScriptAni(0, 9));
            ani2 = new ScriptAniJob("climb_mid1", "s_JumpUpMid", new ScriptAni(0, 1));
            ani3 = new ScriptAniJob("climb_mid2", "t_JumpUpMid_2_Stand", new ScriptAni(0, 20));

            model.AddAniJob(ani1);
            model.AddAniJob(ani2);
            model.AddAniJob(ani3);

            ani1.NextAni = ani2;
            ani2.NextAni = ani3;
            
            ani1 = new ScriptAniJob("climb_high", "t_Jump_2_Hang", new ScriptAni(0, 17));
            ani2 = new ScriptAniJob("climb_high1", "s_hang", new ScriptAni(0, 1));
            ani3 = new ScriptAniJob("climb_high2", "t_Hang_2_Stand", new ScriptAni(0, 25));

            model.AddAniJob(ani1);
            model.AddAniJob(ani2);
            model.AddAniJob(ani3);

            ani1.NextAni = ani2;
            ani2.NextAni = ani3;
        }

        #endregion

        #region Fist Animations

        void AddFistAnis(ModelDef model)
        {
            #region Draw

            // Draw Fists
            ScriptAniJob aniJob1 = new ScriptAniJob("drawfists_part0", "t_Run_2_Fist", new ScriptAni(0, 3));
            aniJob1.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 2);
            ScriptAniJob aniJob2 = new ScriptAniJob("drawfists_part1", "s_Fist", new ScriptAni(0, 1));
            ScriptAniJob aniJob3 = new ScriptAniJob("drawfists_part2", "t_Fist_2_FistRun", new ScriptAni(0, 3));

            model.AddAniJob(aniJob1);
            model.AddAniJob(aniJob2);
            model.AddAniJob(aniJob3);

            aniJob1.NextAni = aniJob2;
            aniJob2.NextAni = aniJob3;

            // Draw Fists running
            ScriptAniJob aniJob = new ScriptAniJob("drawfists_running", "t_Move_2_FistMove", new ScriptAni(0, 14));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 5);
            aniJob.Layer = 2;

            model.AddAniJob(aniJob);

            // Undraw fists
            aniJob1 = new ScriptAniJob("undrawfists_part0", "t_FistRun_2_Fist", new ScriptAni(0, 3));
            aniJob1.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 2);
            aniJob2 = new ScriptAniJob("undrawfists_part1", "s_Fist", new ScriptAni(0, 1));
            aniJob3 = new ScriptAniJob("undrawfists_part2", "t_Fist_2_Run", new ScriptAni(0, 3));

            model.AddAniJob(aniJob1);
            model.AddAniJob(aniJob2);
            model.AddAniJob(aniJob3);

            aniJob1.NextAni = aniJob2;
            aniJob2.NextAni = aniJob3;

            // Undraw Fists running
            aniJob = new ScriptAniJob("undrawfists_running", "t_FistMove_2_Move", new ScriptAni(0, 14));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 5);
            aniJob.Layer = 2;

            model.AddAniJob(aniJob);

            #endregion

            #region Fighting

            // Fight Animations
            aniJob = new ScriptAniJob("fistattack_fwd0", "s_FistAttack", new ScriptAni(0, 15));
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Combo, 9);
            model.AddAniJob(aniJob);

            aniJob = new ScriptAniJob("fistattack_fwd1", "s_FistAttack", new ScriptAni(15, 29));
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Hit, 9);
            model.AddAniJob(aniJob);

            aniJob = new ScriptAniJob("fistattack_run", "t_FistAttackMove", new ScriptAni(0, 29));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Hit, 19);
            model.AddAniJob(aniJob);

            aniJob = new ScriptAniJob("fist_parade", "t_FistParade_0", new ScriptAni(0, 12));
            model.AddAniJob(aniJob);

            aniJob = new ScriptAniJob("fist_jumpback", "t_FistParadeJumpB", new ScriptAni(0, 12));
            model.AddAniJob(aniJob);

            #endregion
        }

        #endregion

        #region 1H Anis

        void Add1HAnis(ModelDef model)
        {
            var ov1 = new ScriptOverlay("1HST1", "Humans_1hST1"); model.AddOverlay(ov1);
            var ov2 = new ScriptOverlay("1HST2", "Humans_1hST2"); model.AddOverlay(ov2);

            #region Draw

            // Draw 1h
            ScriptAniJob aniJob1 = new ScriptAniJob("draw1h_part0", "t_Run_2_1h");
            model.AddAniJob(aniJob1);
            aniJob1.SetDefaultAni(new ScriptAni(0, 2) { { SpecialFrame.Draw, 2 } });
            aniJob1.AddOverlayAni(new ScriptAni(0, 3) { { SpecialFrame.Draw, 3 } }, ov1);
            aniJob1.AddOverlayAni(new ScriptAni(0, 1) { { SpecialFrame.Draw, 1 } }, ov2);

            ScriptAniJob aniJob2 = new ScriptAniJob("draw1h_part1", "s_1h");
            model.AddAniJob(aniJob2);
            aniJob2.SetDefaultAni(new ScriptAni(0, 1));
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov1);
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov2);
            aniJob1.NextAni = aniJob2;

            ScriptAniJob aniJob3 = new ScriptAniJob("draw1h_part2", "t_1h_2_1hRun");
            model.AddAniJob(aniJob3);
            aniJob3.SetDefaultAni(new ScriptAni(0, 12));
            aniJob3.AddOverlayAni(new ScriptAni(0, 6), ov1);
            aniJob2.AddOverlayAni(new ScriptAni(0, 8), ov2);
            aniJob2.NextAni = aniJob3;

            // Draw 1h running
            ScriptAniJob aniJob = new ScriptAniJob("draw1h_running", "t_Move_2_1hMove", new ScriptAni(0, 24));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 6);
            model.AddAniJob(aniJob);

            // Undraw 1h
            aniJob1 = new ScriptAniJob("undraw1h_part0", "t_1hRun_2_1h");
            model.AddAniJob(aniJob1);
            aniJob1.SetDefaultAni(new ScriptAni(0, 12) { { SpecialFrame.Draw, 12 } });
            aniJob1.AddOverlayAni(new ScriptAni(0, 6) { { SpecialFrame.Draw, 6 } }, ov1);
            aniJob1.AddOverlayAni(new ScriptAni(0, 8) { { SpecialFrame.Draw, 8 } }, ov2);

            aniJob2 = new ScriptAniJob("undraw1h_part1", "s_1h");
            model.AddAniJob(aniJob2);
            aniJob2.SetDefaultAni(new ScriptAni(0, 1));
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov1);
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov2);
            aniJob1.NextAni = aniJob2;

            aniJob3 = new ScriptAniJob("undraw1h_part2", "t_1h_2_Run");
            model.AddAniJob(aniJob3);
            aniJob3.SetDefaultAni(new ScriptAni(0, 2));
            aniJob3.AddOverlayAni(new ScriptAni(0, 3), ov1);
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov2);
            aniJob2.NextAni = aniJob3;

            // Undraw 1h running
            aniJob = new ScriptAniJob("undraw1h_running", "t_1hMove_2_Move", new ScriptAni(0, 24));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 18);
            model.AddAniJob(aniJob);

            #endregion

            #region Fighting

            // Fwd attack 1
            ScriptAniJob job = new ScriptAniJob("1hattack_fwd0", "s_1hattack");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 23) { { SpecialFrame.Hit, 6 }, { SpecialFrame.Combo, 15 } });
            job.AddOverlayAni(new ScriptAni(0, 33) { { SpecialFrame.Hit, 4 }, { SpecialFrame.Combo, 14 } }, ov1);
            job.AddOverlayAni(new ScriptAni(0, 29) { { SpecialFrame.Hit, 4 }, { SpecialFrame.Combo, 10 } }, ov2);

            // fwd combo 2
            job = new ScriptAniJob("1hattack_fwd1", "s_1hattack");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(26, 40) { { SpecialFrame.Hit, 7 } });
            job.AddOverlayAni(new ScriptAni(33, 68) { { SpecialFrame.Hit, 4 }, { SpecialFrame.Combo, 15 } }, ov1);
            job.AddOverlayAni(new ScriptAni(33, 60) { { SpecialFrame.Hit, 3 }, { SpecialFrame.Combo, 9 } }, ov2);

            // fwd combo 3
            job = new ScriptAniJob("1hattack_fwd2", "s_1hattack");
            model.AddAniJob(job);
            job.AddOverlayAni(new ScriptAni(68, 103) { { SpecialFrame.Hit, 6 }, { SpecialFrame.Combo, 17 } }, ov1);
            job.AddOverlayAni(new ScriptAni(65, 92) { { SpecialFrame.Hit, 8 }, { SpecialFrame.Combo, 13 } }, ov2);

            // fwd combo 4
            job = new ScriptAniJob("1hattack_fwd3", "s_1hattack");
            model.AddAniJob(job);
            job.AddOverlayAni(new ScriptAni(103, 120) { { SpecialFrame.Hit, 7 } }, ov1);
            job.AddOverlayAni(new ScriptAni(97, 113) { { SpecialFrame.Hit, 10 } }, ov2);

            // left attack
            job = new ScriptAniJob("1hattack_left", "t_1HAttackL");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 30) { { SpecialFrame.Hit, 5 }, { SpecialFrame.Combo, 15 } });
            job.AddOverlayAni(new ScriptAni(0, 24) { { SpecialFrame.Hit, 4 }, { SpecialFrame.Combo, 10 } }, ov1);
            job.AddOverlayAni(new ScriptAni(0, 20) { { SpecialFrame.Hit, 3 }, { SpecialFrame.Combo, 8 } }, ov2);

            // right attack
            job = new ScriptAniJob("1hattack_right", "t_1HAttackR");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 30) { { SpecialFrame.Hit, 5 }, { SpecialFrame.Combo, 15 } });
            job.AddOverlayAni(new ScriptAni(0, 24) { { SpecialFrame.Hit, 4 }, { SpecialFrame.Combo, 10 } }, ov1);
            job.AddOverlayAni(new ScriptAni(0, 20) { { SpecialFrame.Hit, 3 }, { SpecialFrame.Combo, 8 } }, ov2);

            // run attack
            job = new ScriptAniJob("1hattack_run", "t_1HAttackMove");
            job.Layer = 2;
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 29) { { SpecialFrame.Hit, 16 } });

            // parades
            job = new ScriptAniJob("1h_parade0", "t_1HParade_0");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 15));

            job = new ScriptAniJob("1h_parade1", "t_1HParade_0_A2");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 15));

            job = new ScriptAniJob("1h_parade2", "t_1HParade_0_A3");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 15));

            // dodge
            job = new ScriptAniJob("1h_dodge", "t_1HParadeJumpB");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 14));

            #endregion
        }

        #endregion

        #region 2H Anis

        void Add2hAnis(ModelDef model)
        {
            var ov1 = new ScriptOverlay("2HST1", "Humans_2hST1"); model.AddOverlay(ov1);
            var ov2 = new ScriptOverlay("2HST2", "Humans_2hST2"); model.AddOverlay(ov2);

            #region Draw

            // Draw 2h
            ScriptAniJob aniJob1 = new ScriptAniJob("draw2h_part0", "t_Run_2_2h");
            model.AddAniJob(aniJob1);
            aniJob1.SetDefaultAni(new ScriptAni(0, 4) { { SpecialFrame.Draw, 4 } });
            aniJob1.AddOverlayAni(new ScriptAni(0, 4) { { SpecialFrame.Draw, 4 } }, ov1);
            aniJob1.AddOverlayAni(new ScriptAni(0, 3) { { SpecialFrame.Draw, 3 } }, ov2);

            ScriptAniJob aniJob2 = new ScriptAniJob("draw2h_part1", "s_2h");
            model.AddAniJob(aniJob2);
            aniJob2.SetDefaultAni(new ScriptAni(0, 1));
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov1);
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov2);
            aniJob1.NextAni = aniJob2;

            ScriptAniJob aniJob3 = new ScriptAniJob("draw2h_part2", "t_2h_2_2hRun");
            model.AddAniJob(aniJob3);
            aniJob3.SetDefaultAni(new ScriptAni(0, 15));
            aniJob3.AddOverlayAni(new ScriptAni(0, 15), ov1);
            aniJob2.AddOverlayAni(new ScriptAni(0, 10), ov2);
            aniJob2.NextAni = aniJob3;

            // Draw 2h running
            ScriptAniJob aniJob = new ScriptAniJob("draw2h_running", "t_Move_2_2hMove", new ScriptAni(0, 24));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 6);
            model.AddAniJob(aniJob);

            // Undraw 2h
            aniJob1 = new ScriptAniJob("undraw2h_part0", "t_2hRun_2_2h");
            model.AddAniJob(aniJob1);
            aniJob1.SetDefaultAni(new ScriptAni(0, 15) { { SpecialFrame.Draw, 15 } });
            aniJob1.AddOverlayAni(new ScriptAni(0, 15) { { SpecialFrame.Draw, 15 } }, ov1);
            aniJob1.AddOverlayAni(new ScriptAni(0, 10) { { SpecialFrame.Draw, 10 } }, ov2);

            aniJob2 = new ScriptAniJob("undraw2h_part1", "s_2h");
            model.AddAniJob(aniJob2);
            aniJob2.SetDefaultAni(new ScriptAni(0, 1));
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov1);
            aniJob2.AddOverlayAni(new ScriptAni(0, 1), ov2);
            aniJob1.NextAni = aniJob2;

            aniJob3 = new ScriptAniJob("undraw2h_part2", "t_2h_2_Run");
            model.AddAniJob(aniJob3);
            aniJob3.SetDefaultAni(new ScriptAni(0, 4));
            aniJob3.AddOverlayAni(new ScriptAni(0, 4), ov1);
            aniJob2.AddOverlayAni(new ScriptAni(0, 3), ov2);
            aniJob2.NextAni = aniJob3;

            // Undraw 2h running
            aniJob = new ScriptAniJob("undraw2h_running", "t_2hMove_2_Move", new ScriptAni(0, 24));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 18);
            model.AddAniJob(aniJob);

            #endregion

            #region Fighting

            // Fwd attack 1
            ScriptAniJob job = new ScriptAniJob("2hattack_fwd0", "s_2hattack");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 31) { { SpecialFrame.Hit, 8 }, { SpecialFrame.Combo, 14 } });
            job.AddOverlayAni(new ScriptAni(0, 35) { { SpecialFrame.Hit, 6 }, { SpecialFrame.Combo, 16 } }, ov1);
            job.AddOverlayAni(new ScriptAni(0, 34) { { SpecialFrame.Hit, 5 }, { SpecialFrame.Combo, 13 } }, ov2);

            // fwd combo 2
            job = new ScriptAniJob("2hattack_fwd1", "s_2hattack");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(31, 50) { { SpecialFrame.Hit, 6 } });
            job.AddOverlayAni(new ScriptAni(40, 75) { { SpecialFrame.Hit, 6 }, { SpecialFrame.Combo, 16 } }, ov1);
            job.AddOverlayAni(new ScriptAni(39, 75) { { SpecialFrame.Hit, 4 }, { SpecialFrame.Combo, 12 } }, ov2);

            // fwd combo 3
            job = new ScriptAniJob("2hattack_fwd2", "s_2hattack");
            model.AddAniJob(job);
            job.AddOverlayAni(new ScriptAni(80, 114) { { SpecialFrame.Hit, 6 }, { SpecialFrame.Combo, 16 } }, ov1);
            job.AddOverlayAni(new ScriptAni(79, 118) { { SpecialFrame.Hit, 9 }, { SpecialFrame.Combo, 17 } }, ov2);

            // fwd combo 4
            job = new ScriptAniJob("2hattack_fwd3", "s_2hattack");
            model.AddAniJob(job);
            job.AddOverlayAni(new ScriptAni(124, 146) { { SpecialFrame.Hit, 12 } }, ov2);

            // left attack
            job = new ScriptAniJob("2hattack_left", "t_2hAttackL");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 36) { { SpecialFrame.Hit, 6 }, { SpecialFrame.Combo, 18 } });
            job.AddOverlayAni(new ScriptAni(0, 28) { { SpecialFrame.Hit, 5 }, { SpecialFrame.Combo, 14 } }, ov1);
            job.AddOverlayAni(new ScriptAni(0, 26) { { SpecialFrame.Hit, 5 }, { SpecialFrame.Combo, 14 } }, ov2);

            // right attack
            job = new ScriptAniJob("2hattack_right", "t_2hAttackR");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 36) { { SpecialFrame.Hit, 6 }, { SpecialFrame.Combo, 18 } });
            job.AddOverlayAni(new ScriptAni(0, 29) { { SpecialFrame.Hit, 5 }, { SpecialFrame.Combo, 14 } }, ov1);
            job.AddOverlayAni(new ScriptAni(0, 26) { { SpecialFrame.Hit, 5 }, { SpecialFrame.Combo, 14 } }, ov2);

            // run attack
            job = new ScriptAniJob("2hattack_run", "t_2hAttackMove");
            job.Layer = 2;
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 23) { { SpecialFrame.Hit, 12 } });

            // parades
            job = new ScriptAniJob("2h_parade0", "t_2hParade_0");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 15));

            job = new ScriptAniJob("2h_parade1", "t_2hParade_0_A2");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 15));

            job = new ScriptAniJob("2h_parade2", "t_2hParade_0_A3");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 15));

            // dodge
            job = new ScriptAniJob("2h_dodge", "t_2hParadeJumpB");
            model.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 24));

            #endregion
        }

        #endregion

        #region Minecrawler

        public void AddCrawlers()
        {
            // HUMAN MODEL
            ModelDef m = new ModelDef("crawler", "crawler.mds");
            m.SetAniCatalog(new Sumpfkraut.Visuals.AniCatalogs.NPCCatalog());

            var aniJob = new ScriptAniJob("fistattack_fwd0", "s_FistAttack", new ScriptAni(0, 20));
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Hit, 9);
            m.AddAniJob(aniJob);

            aniJob = new ScriptAniJob("fistattack_run", "t_FistAttackMove", new ScriptAni(0, 29));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Hit, 16);
            m.AddAniJob(aniJob);

            aniJob = new ScriptAniJob("fist_parade", "t_FistParade_0", new ScriptAni(0, 29));
            m.AddAniJob(aniJob);

            aniJob = new ScriptAniJob("fist_jumpback", "t_FistParadeJumpB", new ScriptAni(0, 29));
            m.AddAniJob(aniJob);

            m.Radius = 180;
            m.Height = 180;
            m.FistRange = 80;
            m.Create();

            // NPCs
            NPCDef npcDef = new NPCDef("minecrawler");
            npcDef.Name = "Minecrawler";
            npcDef.Model = m;
            npcDef.BodyMesh = "Crw_Body";
            npcDef.BodyTex = 0;
            npcDef.HeadMesh = "";
            npcDef.HeadTex = 0;
            npcDef.Create();

            npcDef = new NPCDef("minecrawler_warrior");
            npcDef.Name = "Minecrawler-Krieger";
            npcDef.Model = m;
            npcDef.BodyMesh = "Cr2_Body";
            npcDef.BodyTex = 0;
            npcDef.HeadMesh = "";
            npcDef.HeadTex = 0;
            npcDef.Create();
        }

        #endregion
        
        #region Orcs

        public void AddOrcs()
        {
            ModelDef m = new ModelDef("orc", "orc.mds");
            m.SetAniCatalog(new Sumpfkraut.Visuals.AniCatalogs.NPCCatalog());

            #region Draw

            // Draw 2h
            ScriptAniJob aniJob1 = new ScriptAniJob("draw2h_part0", "t_Run_2_2h");
            m.AddAniJob(aniJob1);
            aniJob1.SetDefaultAni(new ScriptAni(0, 5) { { SpecialFrame.Draw, 5 } });

            ScriptAniJob aniJob2 = new ScriptAniJob("draw2h_part1", "s_2h");
            m.AddAniJob(aniJob2);
            aniJob2.SetDefaultAni(new ScriptAni(0, 1));
            aniJob1.NextAni = aniJob2;

            ScriptAniJob aniJob3 = new ScriptAniJob("draw2h_part2", "t_2h_2_2hRun");
            m.AddAniJob(aniJob3);
            aniJob3.SetDefaultAni(new ScriptAni(0, 12));
            aniJob2.NextAni = aniJob3;

            // Draw 2h running
            ScriptAniJob aniJob = new ScriptAniJob("draw2h_running", "t_Move_2_2hMove", new ScriptAni(0, 20));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 7);
            m.AddAniJob(aniJob);

            // Undraw 2h
            aniJob1 = new ScriptAniJob("undraw2h_part0", "t_2hRun_2_2h");
            m.AddAniJob(aniJob1);
            aniJob1.SetDefaultAni(new ScriptAni(0, 12) { { SpecialFrame.Draw, 12 } });

            aniJob2 = new ScriptAniJob("undraw2h_part1", "s_2h");
            m.AddAniJob(aniJob2);
            aniJob2.SetDefaultAni(new ScriptAni(0, 1));
            aniJob1.NextAni = aniJob2;

            aniJob3 = new ScriptAniJob("undraw2h_part2", "t_2h_2_Run");
            m.AddAniJob(aniJob3);
            aniJob3.SetDefaultAni(new ScriptAni(0, 5));
            aniJob2.NextAni = aniJob3;

            // Undraw 2h running
            aniJob = new ScriptAniJob("undraw2h_running", "t_2hMove_2_Move", new ScriptAni(0, 20));
            aniJob.Layer = 2;
            aniJob.DefaultAni.SetSpecialFrame(SpecialFrame.Draw, 13);
            m.AddAniJob(aniJob);

            #endregion

            #region Fighting

            // Fwd attack 1
            ScriptAniJob job = new ScriptAniJob("2hattack_fwd0", "s_2hattack");
            m.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 24) { { SpecialFrame.Hit, 6 }, { SpecialFrame.Combo, 11 } });

            // fwd combo 2
            job = new ScriptAniJob("2hattack_fwd1", "s_2hattack");
            m.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(25, 70) { { SpecialFrame.Hit, 20 } });

            // left attack
            job = new ScriptAniJob("2hattack_left", "t_2hAttackL");
            m.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 24) { { SpecialFrame.Hit, 3 }, { SpecialFrame.Combo, 8 } });

            // right attack
            job = new ScriptAniJob("2hattack_right", "t_2hAttackR");
            m.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 24) { { SpecialFrame.Hit, 3 }, { SpecialFrame.Combo, 8 } });

            // run attack
            job = new ScriptAniJob("2hattack_run", "t_2hAttackMove");
            job.Layer = 2;
            m.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 19) { { SpecialFrame.Hit, 13 } });

            // parades
            job = new ScriptAniJob("2h_parade0", "t_2hParade_0");
            m.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 30));

            // dodge
            job = new ScriptAniJob("2h_dodge", "t_2hParadeJumpB");
            m.AddAniJob(job);
            job.SetDefaultAni(new ScriptAni(0, 14));

            #endregion

            m.Radius = 100;
            m.Height = 200;
            m.Create();

            // NPCs
            NPCDef npcDef = new NPCDef("orc_warrior");
            npcDef.Name = "Ork-Krieger";
            npcDef.Model = m;
            npcDef.BodyMesh = "Orc_BodyWarrior";
            npcDef.BodyTex = 0;
            npcDef.HeadMesh = "Orc_HeadWarrior";
            npcDef.HeadTex = 0;
            npcDef.Create();

            npcDef = new NPCDef("orc_elite");
            npcDef.Name = "Ork-Elite";
            npcDef.Model = m;
            npcDef.BodyMesh = "Orc_BodyElite";
            npcDef.BodyTex = 0;
            npcDef.HeadMesh = "Orc_HeadWarrior";
            npcDef.HeadTex = 0;
            npcDef.Create();
        }

        #endregion

        void AddBowAnis(ModelDef model)
        {
            /*var ov1 = new ScriptOverlay("BowT1", "Humans_BowT1"); model.AddOverlay(ov1);
            var ov2 = new ScriptOverlay("BowT2", "Humans_BowT2"); model.AddOverlay(ov2);

            // Weapon drawing

            ScriptAniJob aniJob = new ScriptAniJob("drawbow");
            aniJob.BaseAniJob.ID = (int)SetAnis.DrawBow;
            aniJob.AniName = "drawBow";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(ScriptAni.NewDrawAni(7000000, 2600000));
            aniJob.AddOverlayAni(ScriptAni.NewDrawAni(6000000, 2330000), ov1);
            aniJob.AddOverlayAni(ScriptAni.NewDrawAni(5300000, 2310000), ov2);

            aniJob = new ScriptAniJob("drawbowrun");
            aniJob.BaseAniJob.ID = (int)SetAnis.DrawBowRun;
            aniJob.AniName = "T_MOVE_2_BOWMOVE";
            model.AddAniJob(aniJob);
            var ani = ScriptAni.NewDrawAni(7200000, 2650000); ani.Layer = 2; aniJob.SetDefaultAni(ani);

            aniJob = new ScriptAniJob("undrawbow");
            aniJob.BaseAniJob.ID = (int)SetAnis.UndrawBow;
            aniJob.AniName = "undrawBow";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(ScriptAni.NewDrawAni(6500000, 4400000));
            aniJob.AddOverlayAni(ScriptAni.NewDrawAni(5500000, 3670000), ov1);
            aniJob.AddOverlayAni(ScriptAni.NewDrawAni(4800000, 2990000), ov2);

            aniJob = new ScriptAniJob("undrawbowrun");
            aniJob.BaseAniJob.ID = (int)SetAnis.UndrawBowRun;
            aniJob.AniName = "T_BOWMOVE_2_MOVE";
            model.AddAniJob(aniJob);
            ani = ScriptAni.NewDrawAni(7200000, 4550000); ani.Layer = 2; aniJob.SetDefaultAni(ani);


            // AIMING

            aniJob = new ScriptAniJob("bowaim");
            aniJob.BaseAniJob.ID = (int)SetAnis.BowAim;
            aniJob.AniName = "t_BowWalk_2_BowAim";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(new ScriptAni(4000000));
            aniJob.AddOverlayAni(new ScriptAni(4000000), ov1);
            aniJob.AddOverlayAni(new ScriptAni(4000000), ov2);
            
            // RELOADING

            aniJob = new ScriptAniJob("bowreload");
            aniJob.BaseAniJob.ID = (int)SetAnis.BowReload;
            aniJob.AniName = "t_BowReload";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(new ScriptAni(11200000));
            aniJob.AddOverlayAni(new ScriptAni(10400000), ov1);
            aniJob.AddOverlayAni(new ScriptAni(8800000), ov2);

            // LOWERING

            aniJob = new ScriptAniJob("bowlower");
            aniJob.BaseAniJob.ID = (int)SetAnis.BowLower;
            aniJob.AniName = "t_BowAim_2_BowWalk";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(new ScriptAni(4000000));
            aniJob.AddOverlayAni(new ScriptAni(4000000), ov1);
            aniJob.AddOverlayAni(new ScriptAni(4000000), ov2);*/
        }

        void AddXBowAnis(ModelDef model)
        {
            /*var ov1 = new ScriptOverlay("XBowT1", "Humans_CBowT1"); model.AddOverlay(ov1);
            var ov2 = new ScriptOverlay("XBowT2", "Humans_CBowT2"); model.AddOverlay(ov2);

            // Weapon drawing

            ScriptAniJob aniJob = new ScriptAniJob("drawxbow");
            aniJob.BaseAniJob.ID = (int)SetAnis.DrawXBow;
            aniJob.AniName = "drawXBow";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(ScriptAni.NewDrawAni(15000000, 2700000));
            aniJob.AddOverlayAni(ScriptAni.NewDrawAni(13300000, 2660000), ov1);
            aniJob.AddOverlayAni(ScriptAni.NewDrawAni(11700000, 2260000), ov2);

            aniJob = new ScriptAniJob("drawxbowrun");
            aniJob.BaseAniJob.ID = (int)SetAnis.DrawXBowRun;
            aniJob.AniName = "T_MOVE_2_CBOWMOVE";
            model.AddAniJob(aniJob);
            var ani = ScriptAni.NewDrawAni(15200000, 2660000); ani.Layer = 2; aniJob.SetDefaultAni(ani);

            aniJob = new ScriptAniJob("undrawxbow");
            aniJob.BaseAniJob.ID = (int)SetAnis.UndrawXBow;
            aniJob.AniName = "undrawXBow";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(ScriptAni.NewDrawAni(14500000, 12300000));
            aniJob.AddOverlayAni(ScriptAni.NewDrawAni(12800000, 10140000), ov1);
            aniJob.AddOverlayAni(ScriptAni.NewDrawAni(11200000, 8940000), ov2);

            aniJob = new ScriptAniJob("undrawxbowrun");
            aniJob.BaseAniJob.ID = (int)SetAnis.UndrawXBowRun;
            aniJob.AniName = "T_CBOWMOVE_2_MOVE";
            model.AddAniJob(aniJob);
            ani = ScriptAni.NewDrawAni(15200000, 12540000); ani.Layer = 2; aniJob.SetDefaultAni(ani);


            // AIMING

            aniJob = new ScriptAniJob("xbowaim");
            aniJob.BaseAniJob.ID = (int)SetAnis.XBowAim;
            aniJob.AniName = "t_CBowWalk_2_CBowAim";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(new ScriptAni(1600000));
            aniJob.AddOverlayAni(new ScriptAni(2000000), ov1);
            aniJob.AddOverlayAni(new ScriptAni(2000000), ov2);
            
            // RELOADING

            aniJob = new ScriptAniJob("Xbowreload");
            aniJob.BaseAniJob.ID = (int)SetAnis.XBowReload;
            aniJob.AniName = "t_CBowReload";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(new ScriptAni(12800000));
            aniJob.AddOverlayAni(new ScriptAni(11600000), ov1);
            aniJob.AddOverlayAni(new ScriptAni(9200000), ov2);

            // LOWERING

            aniJob = new ScriptAniJob("xbowlower");
            aniJob.BaseAniJob.ID = (int)SetAnis.XBowLower;
            aniJob.AniName = "t_CBowAim_2_CBowWalk";
            model.AddAniJob(aniJob);

            aniJob.SetDefaultAni(new ScriptAni(1600000));
            aniJob.AddOverlayAni(new ScriptAni(2000000), ov1);
            aniJob.AddOverlayAni(new ScriptAni(2000000), ov2);*/
        }
    }
}

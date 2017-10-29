using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Utilities.Threading;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.AI
{

    public class TestingAI : ExtendedObject
    {

        public static List<string> ArmorCodes = new List<string>() {
            "itar_schatten", "itar_Garde",
            "itar_bandit", "itar_söldner"
        };

        public static List<string> WeaponCodes_1H = new List<string>() {
            "1hschwert", "1haxt",
            "2hschwert", "2haxt"
        };

        public static List<string> WeaponCodes_2H = new List<string>() {
            "2hschwert", "2haxt"
        };

        public static List<string> MeleeOverlayCodes = new List<string>() {
            "1HS2", "2HS1",
        };

        public static List<string> WeaponCodes_Bow = new List<string>() {
            "itrw_longbow",
        };

        public static List<string> WeaponCodes_Crossbow = new List<string>() {
            "itrw_crossbow",
        };

        public static List<string> AmmoCodes_Bow = new List<string>() {
            "itrw_arrow"
        };

        public static List<string> AmmoCodes_Crossbow = new List<string>() {
            "itrw_bolt",
        };



        public static void Test ()
        {
            //Stopwatch outerSW = new Stopwatch();

            //int i = 0;
            //int lapses = 1000000;

            //int tempInt;
            //object tempObject;
            //SimpleAI.AIAgent tempAIAgent;

            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    tempInt = 999;
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds);

            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    tempObject = new object();
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds);

            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    tempAIAgent = new SimpleAI.AIAgent();
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds);


            //Stopwatch outerSW = new Stopwatch();
            //int lapses = 1000000;
            //int i = 0;

            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds
            //    + "ms / lapse" + ((double) outerSW.ElapsedMilliseconds / lapses));

            //object lockObj = new object();
            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    lock (lockObj) { }
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds
            //    + "ms / lapse" + ((double) outerSW.ElapsedMilliseconds / lapses));


            //SimpleAI.AIAgent agentBlack = new SimpleAI.AIAgent();
            //object bla = agentBlack.AIPersonality.Bla;
            //bla = null;
            //Log.Logger.Log(">>>> " + bla);


            //List<int> a = new List<int> { 0, 1, 2, 3, 4 };
            //List<int> b = new List<int> { 2, 3, 4, 5, 6 };
            //IEnumerable<int> c = a.Union(b);
            //List<int> d = c.ToList();

            //for (int i = 0; i < d.Count; i++)
            //{
            //    Log.Logger.Log(d[i]);
            //}


            //Stopwatch sw = Stopwatch.StartNew();
            //System.Threading.Thread.Sleep(100);
            //sw.Stop();

            //Log.Logger.Log(sw.ElapsedTicks);
            //Log.Logger.Log(sw.Elapsed.Ticks);
            //Log.Logger.Log(TimeSpan.TicksPerMillisecond);
            //Log.Logger.Log(sw.Elapsed.Ticks / TimeSpan.TicksPerMillisecond);

            //int loops = 0;
            //for (int i = 0; i < 100; i++)
            //{
            //    if (loops == 20) { break; }
            //    if ((i / 5) != 0) { i--; }
            //    loops++;
            //    Log.Logger.Log(loops + ": " + i);
            //}




            //AIManager.InitStatic();
            //AIManager aiManager01 = new AIManager(true, false, new TimeSpan(0, 0, 0, 0, 500));
            //aiManager01.SetObjName("aiManager01");
            //aiManager01.Start();


            //List<NPCDef> dummyDefinitions = new List<NPCDef>();
            //List<NPCInst> dummyInstances = new List<NPCInst>();
            //List<int> dummyIDs = new List<int>();

            //int spawnAmount = 10;
            //int indexDigits = (int) Math.Floor(Math.Log10(50) + 1);
            //Types.Vec3f[] spawnRange = new Types.Vec3f[] {
            //    new Types.Vec3f(-2000f, 1000f, -2000f),
            //    new Types.Vec3f(2000f, 1000f, 2000f)};
            //Types.Vec3f spawnRangeDiff = spawnRange[1] - spawnRange[0];

            //Random random = new Random();

            //NPCDef tempDef = null;
            //for (int i = 0; i < spawnAmount; i++)
            //{
            //    tempDef = CreateDummyDef("NPCDef_" + i.ToString("D" + indexDigits), random);
            //    if (tempDef != null)
            //    {
            //        dummyDefinitions.Add(tempDef);
            //        dummyIDs.Add(i);
            //    }
            //}

            //NPCInst tempInst = null;
            //Types.Vec3f spawnLocation;

            //AIAgent aiAgent;
            //AIMemory aiMemory;
            //SimpleAI.AIRoutines.SimpleAIRoutine aiRoutine;
            //SimpleAI.AIPersonalities.SimpleAIPersonality aiPersonality;

            //for (int i = 0; i < dummyIDs.Count; i++)
            //{
            //    spawnLocation = new Types.Vec3f(
            //        (float) (spawnRange[0].X + (random.NextDouble() * spawnRangeDiff.X)),
            //        (float) (spawnRange[0].Y + (random.NextDouble() * spawnRangeDiff.Y)),
            //        (float) (spawnRange[0].Z + (random.NextDouble() * spawnRangeDiff.Z)));
            //    tempInst = CreateDummyInst(dummyDefinitions[i], "NPCInst_" + dummyIDs[i].ToString("D" + indexDigits),
            //        spawnLocation, random);
            //    if (tempInst != null)
            //    {
            //        dummyInstances.Add(tempInst);

            //        aiMemory = new AIMemory();
            //        aiRoutine = new SimpleAI.AIRoutines.SimpleAIRoutine();
            //        aiPersonality = new SimpleAI.AIPersonalities.SimpleAIPersonality(20000f, 1f);
            //        aiPersonality.Init(aiMemory, aiRoutine);
            //        aiPersonality.SetObjName(tempInst.GetObjName());
            //        aiAgent = new AIAgent(new List<VobInst> { tempInst }, aiPersonality);
            //        aiAgent.SetObjName(tempInst.GetObjName());
            //        aiManager01.SubscribeAIAgent(aiAgent);
            //        MakeLogStatic(typeof(TestingAI), "Created dummy-npc " + tempInst.GetObjName());
            //    }
            //}




            PrintStatic(typeof(TestingAI), "-----------------------------------");

            //PrintStatic(typeof(TestingAI), 
            //    ((WorldSystem.WorldInst) npcInst01.BaseInst.World.ScriptObject). == null);


            //Runnable runTest = new Runnable(false, new TimeSpan(0, 0, 1), false);
            //runTest.OnRun += delegate (Runnable sender)
            //{
            //    WorldSystem.WorldInst theWorld = WorldSystem.WorldInst.Current;
            //    if (theWorld == null) { PrintStatic(typeof(TestingAI), "theWorld == null"); }
            //    else
            //    {

            //        PrintStatic(typeof(TestingAI), theWorld.BaseWorld);
            //        PrintStatic(typeof(TestingAI), theWorld.BaseWorld);
            //    }
            //};
            //runTest.Start();

            //string testJson = Newtonsoft.Json.JsonConvert.SerializeObject(new TestClass(999, 777));
            ////Log.Logger.Log(testJson);
            //TestClass testClass = Newtonsoft.Json.JsonConvert.DeserializeObject<TestClass>(testJson);
            //Log.Logger.Log(testClass.ToString());
            //Log.Logger.Log(testClass.PrivInt.ToString());
            //Log.Logger.Log(testClass.pubInt.ToString());
        }



        public static T RandomEnumValue<T> ()
        {
            var v = Enum.GetValues (typeof (T));
            return (T) v.GetValue (new Random ().Next(v.Length));
        }

        public static NPCDef CreateDummyDef (string name, Random random)
        {
            random = random ?? new Random();
            NPCDef npcDef = null;
            Visuals.ModelDef model;
            if (Visuals.ModelDef.TryGetModel("human", out model))
            {
                npcDef = new NPCDef(name);
                npcDef.Name = name;
                npcDef.Model = model;
                npcDef.BodyMesh = HumBodyMeshs.HUM_BODY_NAKED0.ToString();
                //npcDef.BodyMesh = RandomEnumValue<Enumeration.HumBodyMeshs>().ToString();
                npcDef.BodyTex = (int) RandomEnumValue<HumBodyTexs>();
                npcDef.HeadMesh = RandomEnumValue<HumHeadMeshs>().ToString();
                npcDef.HeadTex = (int) RandomEnumValue<HumHeadTexs>();
                npcDef.Create();

                //Log.Logger.Log(name);
                //Log.Logger.Log(npcDef.BodyTex);
                //Log.Logger.Log(npcDef.HeadMesh);
                //Log.Logger.Log(npcDef.HeadTex);
            }
            else
            {
                MakeLogWarningStatic(typeof(TestingAI), "Failed to generate dummy definition in ai tests!");
            }
            
            return npcDef;
        }

        public static NPCInst CreateDummyInst (NPCDef npcDef, string name, Types.Vec3f spawnPosition, Random random)
        {
            random = random ?? new Random();

            int guild = random.Next(0, 2);
            int armorType = random.Next(0, 4);
            int meleeType = random.Next(0, 4);
            int rangedType = random.Next(0, 2);

            NPCInst npcInst = null;
            npcInst = new NPCInst(npcDef);
            npcInst.SetObjName(name);
            npcInst.BaseInst.SetNeedsClientGuide(true);

            ItemInst armor = null, weapon = null, rangeWeapon = null, ammo = null;
            int ammoAmount;
            ScriptOverlay overlay = null;
            bool overlayExists = false;

            armor = new ItemInst(ItemDef.Get<ItemDef>( ArmorCodes[random.Next(0, ArmorCodes.Count)] ));

            // melee weapon
            if (random.Next(0, 2) == 0)
            {
                // 1H-weapon
                weapon = new ItemInst(ItemDef.Get<ItemDef>( WeaponCodes_1H[random.Next(0, WeaponCodes_1H.Count)] ));
                overlayExists = npcInst.ModelDef.TryGetOverlay("1HST2", out overlay);
            }
            else
            {
                // 2H-weapon
                weapon = new ItemInst(ItemDef.Get<ItemDef>( WeaponCodes_2H[random.Next(0, WeaponCodes_2H.Count)] ));
                overlayExists = npcInst.ModelDef.TryGetOverlay("2HST1", out overlay);
            }

            // ranged weapon
            if (random.Next(0, 2) == 0)
            {
                // bow
                rangeWeapon = new ItemInst(ItemDef.Get<ItemDef>( WeaponCodes_Bow[random.Next(0, WeaponCodes_Bow.Count)] ));
                ammo = new ItemInst(ItemDef.Get<ItemDef>( AmmoCodes_Bow[random.Next(0, AmmoCodes_Bow.Count)] ));
            }
            else
            {
                // crossbow
                rangeWeapon = new ItemInst(ItemDef.Get<ItemDef>( WeaponCodes_Crossbow[random.Next(0, WeaponCodes_Crossbow.Count)] ));
                ammo = new ItemInst(ItemDef.Get<ItemDef>( AmmoCodes_Crossbow[random.Next(0, AmmoCodes_Crossbow.Count)] ));
            }

            ammoAmount = random.Next(10, 500 + 1);
            ammo.BaseInst.SetAmount(ammoAmount);

            npcInst.Inventory.AddItem(weapon);
            npcInst.EquipItem(weapon); // 1 = DrawnWeapon

            npcInst.Inventory.AddItem(rangeWeapon);
            npcInst.EquipItem(rangeWeapon);

            npcInst.Inventory.AddItem(ammo);
            npcInst.EquipItem(ammo);

            npcInst.Inventory.AddItem(armor);
            npcInst.EquipItem(armor);

            if (overlayExists) { npcInst.ModelInst.ApplyOverlay(overlay); }

            npcInst.Spawn(WorldSystem.WorldInst.List[0], 
                spawnPosition, new Types.Vec3f(1f, 0f, 0f));

            return npcInst;
        }




        //public class TestClass
        //{

        //    private int privInt;
        //    public int PrivInt { get { return privInt; } }

        //    [Newtonsoft.Json.JsonIgnore]
        //    public int pubInt;

        //    public TestClass (int privInt, int pubInt)
        //    {
        //        this.privInt = privInt;
        //        this.pubInt = pubInt;
        //    }

        //}
        
    }

}

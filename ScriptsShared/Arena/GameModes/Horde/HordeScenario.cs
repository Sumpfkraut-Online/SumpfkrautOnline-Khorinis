using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    class HordeScenario : GameScenario
    {
        public class Item
        {
            public string ItemDef;
            public Vec3f Position;
            public Angles Angles;

            public Item(string itemDef, float x, float y, float z, float pitch = 0, float yaw = 0, float roll = 0)
            {
                this.ItemDef = itemDef;
                this.Position = new Vec3f(x, y, z);
                this.Angles = new Angles(pitch, yaw, roll);
            }
        }

        public class Pair
        {
            public NPCClass Enemy;
            public float CountScale;

            public Pair(NPCClass enemyClass, float countScale = 1)
            {
                this.Enemy = enemyClass;
                this.CountScale = countScale;
            }
        }

        public class Group
        {
            public Pair[] npcs;
            public Vec3f Position;
            public float Range;
            public float Yaw;

            public Group(float x, float y, float z, float range, params Pair[] npcs) : this(x, y, z, range, 0, npcs)
            {
            }

            public Group(float x, float y, float z, float range, float yaw, params Pair[] npcs)
            {
                this.Position = new Vec3f(x, y, z);
                this.Range = range;
                this.npcs = npcs;
                this.Yaw = yaw;
            }
        }

        public class Stand
        {
            public Barrier[] Barriers;

            public Vec3f Position;
            public float Range;

            public long Duration;

            public float EnemyCountPerGroup; // how many enemies per group
            public int EnemyGroupsPerSpawn; // how many groups per spawn wave
            public long EnemySpawnInterval; // 
            public Pair[] Enemies; // enemy + probability
            public Vec3f[] EnemySpawns;

            public NPCClass Boss;

            public string SFXStart;
            public string SFXLoop;
            public string SFXStop;

            public string[] Messages;
        }

        public override GameMode GetMode() { return HordeMode.Instance; }

        public Item[] Items;

        public NPCClass[] PlayerClasses;

        public Group[] Enemies;
        public Stand[] Stands;

        public Group[] AmbientNPCs;

        public Vec3f[] Respawns;


        public static void Init()
        {
#if false
            #region h_pass

            var pass_OrcScout = new NPCClass()
            {
                Definition = "orc_scout",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("krush_pach"),
                }
            };

            var pass_OrcWarrior = new NPCClass()
            {
                Definition = "orc_warrior",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("krush_pach"),
                }
            };

            var pass_OrcElite = new NPCClass()
            {
                Definition = "orc_elite",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("orc_sword"),
                }
            };


            var pass_Miliz = new NPCClass()
            {
                Name = "Miliz",
                Definition = null,
                Protection = 0,
                Damage = 0,
                HP = 100,
                Overlays = new string[] { "1HST1" },
                ItemDefs = new NPCClass.InvItem[]
                {
                      new NPCClass.InvItem("1hschwert"),
                      new NPCClass.InvItem("ITAR_miliz_s"),
                      new NPCClass.InvItem("light_xbow"),
                      new NPCClass.InvItem("itrw_bolt", 20),
                },
                Guild = Allegiance.MIL,
            };

            var pass_Ritter = new NPCClass()
            {
                Name = "Ritter",
                Definition = null,
                Protection = 50,
                Damage = 0,
                HP = 100,
                Overlays = new string[] { "2HST1" },
                ItemDefs = new NPCClass.InvItem[]
                {
                      new NPCClass.InvItem("2hschwert"),
                      new NPCClass.InvItem("ITAR_ritter"),
                      new NPCClass.InvItem("light_xbow"),
                      new NPCClass.InvItem("itrw_bolt", 20),
                },
                Guild = Allegiance.PAL,
            };

            scenarios.Add(new HordeScenario()
            {
                Name = "h_pass",
                WorldPath = "H_PASS.ZEN",
                SpawnPos = new PosAng(7695, 6509, 42836, 0),
                SpawnRange = 100,

                SpawnWorld = "H_PASS_SPAWN.zen",
                SpawnWorldPos = new Vec3f(31195, 4538, -40437),
                SpawnWorldRange = 1000,

                SpecPoint = new PosAng(31195, 4538, -40437, 0),
                FightDuration = 30 * TimeSpan.TicksPerMinute,
                WorldTime = new WorldTime(0, 22),
                WorldTimeScale = 5,
                WorldBarrier = 0,
                WorldWeather = -1,
                PlayerClasses = new NPCClass[]
                {
                    pass_Miliz,
                    pass_Ritter,
                },

                Enemies = new Group[]
                {                
                    // 1. SEKTION, bis Steindurchgang vor verlassener Mine                
                    new Group(8094, 5626, 36860, 600, new Pair(pass_OrcScout, 0.3f)), // plattform
                    new Group(5581, 5510, 35969, 400, new Pair(pass_OrcScout, 0.5f)), // see
                    new Group(4227, 6145, 31128, 500, new Pair(pass_OrcScout, 1.0f)), // steindurchgang
                
                    // 2. SEKTION, bis Weg am Hang, bei Drachenjägern
                    new Group(4102, 5888, 27445, 500, new Pair(pass_OrcScout, 1.0f)), // verlassene Mine
                    new Group( 678, 6235, 27186, 500, new Pair(pass_OrcWarrior, 0.2f), // bei teleport rune
                                                      new Pair(pass_OrcScout, 0.3f)),
                    new Group(1555, 2900, 21881, 700, new Pair(pass_OrcWarrior, 0.3f), // toter paladin
                                                      new Pair(pass_OrcScout, 1.0f)),
                
                    // 3. SEKTION, bis Brücke am Fluss
                    new Group(2370, 1665, 18719, 200, new Pair(pass_OrcScout, 0.2f)), // weg am hang 1
                    new Group(-516, -280, 14668, 200, new Pair(pass_OrcWarrior, 0.2f)), // weg am hang 2
                    new Group(-5903, -900, 14530, 1500, new Pair(pass_OrcWarrior, 0.5f), // bei drax / jaegern
                                                        new Pair(pass_OrcScout, 2.0f)),
                    new Group(-11240, -705, 12303, 1500, new Pair(pass_OrcWarrior, 0.75f), // vor brücke
                                                         new Pair(pass_OrcScout, 2.0f)),
                    new Group(-11003, -820, 9127, 140, new Pair(pass_OrcElite, 0.2f)), // auf brücke
                },

                Items = new Item[]
                {
                    new Item("hptrank", 2892.88354f, 6425.26074f, 31699.8496f), // austauschstelle brücke
                    new Item("hptrank", 5053.02588f, 5824.37646f, 25956.3828f), // verlassener minenschacht


                    new Item("hptrank", -346.506165f, 6248.29785f, 27447.5215f), // teleport rune
                    new Item("hptrank", -351.038818f, 6247.33643f, 27468.8926f),
                    new Item("hptrank", -329.380737f, 6246.375f, 27458.5488f),

                    new Item("hptrank", 847.420288f, 2719.80615f, 21308.3008f), // toter paladin

                    new Item("hptrank", -6164.2998f, -641.647034f, 16228.6006f), // drax / jäger
                
                    new Item("hptrank", -7673.05566f, -534.731506f, 11694.4043f), // hügel vor brücke
                    new Item("hptrank", -7691.25537f, -534.557861f, 11677.5244f),

                    new Item("hptrank", -9071.97656f, -1346.44824f, 6379.68262f), // baum hinter brücke

                    new Item("hptrank", -13201.7236f, -795.982117f, 5463.70215f), // höhle hinter brücke
                    new Item("hptrank", -13217.3105f, -795.981567f, 5470.14404f),

                    new Item("hptrank", -3203.54468f, -650.505127f, 3661.7583f), // verbranntes haus vor tor

                    new Item("hptrank", -6117.55029f, -998.722168f, 720.570923f), // verbranntes haus an mauer
                },

                Stands = new Stand[]
                {
                    // Vor Burgtor
                    new Stand()
                    {
                        Barriers = new Barrier[] { new Barrier("gate", -3069, -496, 1800, 0, -2.793f, 0) },
                        Position = new Vec3f(-3106, 100, 1912),
                        Range = 2000,
                        EnemyCountPerGroup = 1,
                        EnemyGroupsPerSpawn = 2,
                        EnemySpawnInterval = 15 * TimeSpan.TicksPerSecond,
                        Duration = 180 * TimeSpan.TicksPerSecond,
                        SFXStart = "TRUMPET_01.WAV",
                        SFXLoop = "GATE_LOOP.WAV",
                        SFXStop = "GATE_STOP.WAV",
                        Messages = new string[]
                        {
                            "\"Wartet eine Sekunde, wir öffnen die Tore.\" Du hörst Orkrufe aus der Ferne",
                            "\"Das könnte etwas länger dauern, das Tor hängt.\"",
                            "\"Verdammte Winde!\"",
                            "\"Ist nur noch eine Sache von Sekunden\"",
                            "\"Bei Innos, der Hebel ist abgebrochen. Gebt uns einen Augenblick\"",
                            "\"Na los! Irgendwer soll bei Engor mir einen Hebel besorgen!\"",
                            "\"Alles klar dort unten?\"",
                            "\"Brutus, helf mal mit deinen fetten Armen aus!\"",
                            "\"Tor ist offen!\"",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(-9010, -830, 2986),
                             new Vec3f(-9618, -660, 1485),
                             new Vec3f(-8196, -510, 412),
                             new Vec3f(-1546, -1010, 7224),
                             new Vec3f(163, -1130, 6784),
                             new Vec3f(289, -960, 4472),
                        },
                        Enemies = new Pair[]
                        {
                            new Pair(pass_OrcScout, 1.0f),
                            new Pair(pass_OrcWarrior, 0.8f),
                            new Pair(pass_OrcElite, 0.5f),
                        }
                    }
                },

                AmbientNPCs = new Group[]
                {                
                    // hinter Burgtor            
                    new Group(-2844, -230, 1227, 400, 0.4244f, new Pair(pass_Miliz, 6),
                                                               new Pair(pass_Ritter, 4)),
                },

                Respawns = new Vec3f[]
                {
                    new Vec3f(692.55f, 2824.994f, 21342.08f),
                    new Vec3f(-8501.721f, -1131.424f, 12540.28f),
                    new Vec3f(-3193.676f, -552.6367f, 3412.632f),
                },
            });

            /*section.bridges = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-11011, -947, 9555), Angles = new Angles(0.000, 1.607, 0.122) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-11007, -948, 9339), Angles = new Angles(0.000, 1.716, 0.000) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10994, -977, 9052), Angles = new Angles(-0.017, 1.714, 0.070) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10969, -984, 8762), Angles = new Angles(0.000, 1.716, 0.000) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10948, -1002, 8471), Angles = new Angles(0.000, 1.716, 0.000) },
            };*/

            #endregion

            #region h_irdorath

            var ird_OrcScout = new NPCClass()
            {
                Definition = "orc_scout",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("krush_pach"),
                }
            };

            var ird_OrcWarrior = new NPCClass()
            {
                Definition = "orc_warrior",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("krush_pach"),
                }
            };

            var ird_OrcElite = new NPCClass()
            {
                Definition = "orc_elite",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("orc_sword"),
                }
            };

            var ird_draconian = new NPCClass()
            {
                Definition = "draconian",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("krush_pach"),
                }
            };

            var ird_skeleton_l = new NPCClass()
            {
                Definition = "skeleton",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("grobes_schwert"),
                }
            };

            var ird_skeleton_m = new NPCClass()
            {
                Definition = "skeleton",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("leichter_zweihaender"),
                }
            };

            var ird_Paladin = new NPCClass()
            {
                Name = "Paladin",
                Definition = null,
                Protection = 50,
                Damage = 0,
                HP = 100,
                Overlays = new string[] { "1HST1" },
                ItemDefs = new NPCClass.InvItem[]
                {
                      new NPCClass.InvItem("2hschwert"),
                      new NPCClass.InvItem("ITAR_pal_h"),
                      new NPCClass.InvItem("light_xbow"),
                      new NPCClass.InvItem("itrw_bolt", 20),
                },
                Guild = Allegiance.PAL,
            };

            var ird_Ritter = new NPCClass()
            {
                Name = "Ritter",
                Definition = null,
                Protection = 50,
                Damage = 0,
                HP = 100,
                Overlays = new string[] { "2HST1" },
                ItemDefs = new NPCClass.InvItem[]
                {
                      new NPCClass.InvItem("2hschwert"),
                      new NPCClass.InvItem("ITAR_ritter"),
                      new NPCClass.InvItem("light_xbow"),
                      new NPCClass.InvItem("itrw_bolt", 20),
                },
                Guild = Allegiance.PAL,
            };

            scenarios.Add(new HordeScenario()
            {
                Name = "h_irdorath",
                WorldPath = "H_IRDORATH.ZEN",
                SpawnPos = new PosAng(-9982, -63, -14087, 0),
                SpawnRange = 1000,

                SpawnWorld = "H_IRDORATH_SPAWN.zen",
                SpawnWorldPos = new Vec3f(-9982, -63, -14087),
                SpawnWorldRange = 1000,

                SpecPoint = new PosAng(-9982, -63, -14087, 0),
                FightDuration = 30 * TimeSpan.TicksPerMinute,
                WorldTime = new WorldTime(0, 18),
                WorldTimeScale = 5,
                WorldBarrier = 0,
                WorldWeather = -1,
                PlayerClasses = new NPCClass[]
                {
                    ird_Paladin,
                    ird_Ritter,
                },

                Enemies = new Group[]
                {
                    new Group(-16447, 1060, -12366, 600, new Pair(ird_OrcScout, 0.5f)), // mitte anfang
                    new Group(-17270, 1400, -9920, 600, new Pair(ird_OrcScout, 0.5f)), // anfang 1
                    new Group(-19723, 1600, -11040, 600, new Pair(ird_OrcScout, 0.5f)), // anfang 2, Lagerfeuer
                    
                    // 2 Lagerfeuer
                    new Group(-19803, 1400, -7682, 600, new Pair(ird_OrcScout, 1.0f)), //
                    new Group(-21369, 1400, -8746, 600, new Pair(ird_OrcScout, 0.5f)), //

                    // höhle anfang
                    new Group(-22043, 1550, -3822, 1000, new Pair(ird_OrcScout, 2.0f),
                                                         new Pair(ird_OrcWarrior, 0.1f)), //
                    // oberst raum
                    new Group(-25079, 2100, -1966, 400, new Pair(ird_OrcWarrior, 0.75f)), // lagerfeuer

                    
                    // echsenhöhle
                    new Group(-20937, 2350, 4491, 1000, new Pair(ird_draconian, 0.75f)), // 
                    new Group(-19817, 2350, 6635, 1000, new Pair(ird_draconian, 0.75f)), // 
                    new Group(-20877, 2350, 8147, 1000, new Pair(ird_draconian, 0.75f)), // 

                    // eingang hinter feuerdrache
                    new Group(-20680.33f, 2150, 14910.56f, 1000, new Pair(ird_skeleton_l, 2.0f), 
                                                        new Pair(ird_skeleton_m, 0.5f)),
                    new Group(-20046.33f, 2150, 14856.05f, 1000, new Pair(ird_skeleton_l, 2.0f),
                                                        new Pair(ird_skeleton_m, 0.5f)),
                    new Group(-19311.63f, 2150, 14927.82f, 1000, new Pair(ird_skeleton_l, 2.0f),
                                                        new Pair(ird_skeleton_m, 0.5f)),
                    new Group(-18767.63f, 2150, 14878.88f, 1000, new Pair(ird_skeleton_l, 2.0f),
                                                        new Pair(ird_skeleton_m, 0.5f)),
                    new Group(-18189.92f, 2150, 14899.71f, 1000, new Pair(ird_skeleton_l, 2.0f),
                                                        new Pair(ird_skeleton_m, 0.5f)),
                    new Group(-17515.37f, 2150, 14923.89f, 1000, new Pair(ird_skeleton_l, 2.0f),
                                                        new Pair(ird_skeleton_m, 0.5f)),

                    // halle vor rätseltür / Mario
                    new Group(-17440.56f, 2450, 19284.09f, 500, new Pair(ird_skeleton_l, 2.0f),
                                                        new Pair(ird_skeleton_m, 0.5f)),
                    new Group(-17454.3f, 2450, 20348.74f, 500, new Pair(ird_skeleton_l, 2.0f),
                                                        new Pair(ird_skeleton_m, 0.5f)),

                },

                Items = new Item[]
                {
                    //new Item("hptrank", 2892.88354f, 6425.26074f, 31699.8496f), // 
                },

                Stands = new Stand[]
                {
                    // OrkOberst
                    new Stand()
                    {
                        Barriers = new Barrier[] { new Barrier("irdorathwall", -25209.3164f, 2067.10156f, -828.122559f, 0, -3f, 0) },
                        Position = new Vec3f(-25098, 2100, -1296),
                        Range = 1500,
                        EnemyCountPerGroup = 0,
                        EnemyGroupsPerSpawn = 0,
                        EnemySpawnInterval = TimeSpan.TicksPerDay,
                        Duration = TimeSpan.TicksPerDay,
                        SFXStart = "ORC_WARN01.WAV",
                        SFXLoop = null,
                        SFXStop = "STONE_SMALL_LOOP.WAV",
                        Boss = new NPCClass()
                        {
                            Definition = "orc_oberst",
                            ItemDefs = new NPCClass.InvItem[]
                            {
                                new NPCClass.InvItem("itmw_schlachtaxt"),
                            }
                        },
                        Messages = new string[]
                        {
                            "Fremder tragen Ulumulu, dann Fremder nicht sterben!",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(-9010, -830, 2986),
                             new Vec3f(-9618, -660, 1485),
                             new Vec3f(-8196, -510, 412),
                             new Vec3f(-1546, -1010, 7224),
                             new Vec3f(163, -1130, 6784),
                             new Vec3f(289, -960, 4472),
                        },
                        Enemies = new Pair[]
                        {
                           // new Pair(pass_OrcScout, 1.0f),
                            //new Pair(pass_OrcWarrior, 0.8f),
                            //new Pair(pass_OrcElite, 0.5f),
                        }
                    },      
                    
                    
                    // brücke, feuerdrache
                    new Stand()
                    {
                        Barriers = new Barrier[] { new Barrier("bridge", -23596, 1990, 11816, 0, 3.13392f, 0, true) },
                        Position = new Vec3f(-22476, 2400, 8822),
                        Range = 1500,
                        EnemyCountPerGroup = 0,
                        EnemyGroupsPerSpawn = 0,
                        EnemySpawnInterval = TimeSpan.TicksPerDay,
                        Duration = TimeSpan.TicksPerDay,
                        SFXStart = "DEM_WARN01.WAV",
                        SFXLoop = null,
                        SFXStop = "STONE_LOOP.WAV",
                        Boss = new NPCClass()
                        {
                            HP = 1000,
                            Definition = "dragon_fire",
                        },
                        Messages = new string[]
                        {
                            "Fremder tragen Ulumulu, dann Fremder nicht sterben!",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(-9010, -830, 2986),
                             new Vec3f(-9618, -660, 1485),
                             new Vec3f(-8196, -510, 412),
                             new Vec3f(-1546, -1010, 7224),
                             new Vec3f(163, -1130, 6784),
                             new Vec3f(289, -960, 4472),
                        },
                        Enemies = new Pair[]
                        {
                           // new Pair(pass_OrcScout, 1.0f),
                            //new Pair(pass_OrcWarrior, 0.8f),
                            //new Pair(pass_OrcElite, 0.5f),
                        }
                    },

                    
                    // Schattenlord
                    new Stand()
                    {
                        Barriers = new Barrier[] { new Barrier("door", -19314.959f, 2258.65771f, 18021.9355f, 0, -3.133395f, 0) },
                        Position = new Vec3f(-20355.73f, 2407.706f, 17552.18f),
                        Range = 800,
                        EnemyCountPerGroup = 0.3f,
                        EnemyGroupsPerSpawn = 2,
                        EnemySpawnInterval = 10 * TimeSpan.TicksPerSecond,
                        Duration = TimeSpan.TicksPerDay,
                        SFXStart = "MYSTERY_06.WAV",
                        SFXLoop = null,
                        SFXStop = "MYSTERY_05.WAV",
                        Boss = new NPCClass()
                        {
                            Definition = "skeleton_lord",
                            ItemDefs = new NPCClass.InvItem[]
                            {
                                new NPCClass.InvItem("2hschwert"),
                                new NPCClass.InvItem("ITAR_pal_skel"),
                            }
                        },
                        Messages = new string[]
                        {
                            "Fremder tragen Ulumulu, dann Fremder nicht sterben!",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(-20198.07f, 2357.699f, 18026.83f),
                             new Vec3f(-19897.05f, 2357.757f, 17999.86f),
                             new Vec3f(-19912.54f, 2357.789f, 17088.37f),
                             new Vec3f(-20210.05f, 2357.763f, 17092.79f),
                        },
                        Enemies = new Pair[]
                        {
                            new Pair(ird_skeleton_l, 1.0f),
                            new Pair(ird_skeleton_m, 0.1f),
                        }
                    },            
                    
                    // rätseltür
                    new Stand()
                    {
                        Barriers = new Barrier[] { new Barrier("door_puzzle_left", -17632.3965f, 2435.19922f, 22742.8184f, 0, 0, 0),
                                                   new Barrier("door_puzzle_right", -17292.7227f, 2435.19922f, 22742.8184f, 0, 0, 0)},
                        Position = new Vec3f(-17486.96f, 2427.709f, 21958.92f),
                        Range = 800,
                        EnemyCountPerGroup = 0.3f,
                        EnemyGroupsPerSpawn = 4,
                        EnemySpawnInterval = 5 * TimeSpan.TicksPerSecond,
                        Duration = 30 * TimeSpan.TicksPerSecond,
                        SFXStart = "MYSTERY_06.WAV",
                        SFXLoop = null,
                        SFXStop = "MYSTERY_05.WAV",
                        Boss = null,
                        Messages = new string[]
                        {
                            "Fremder tragen Ulumulu, dann Fremder nicht sterben!",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(-19324, 2600, 23416),
                             new Vec3f(-19499, 2200, 20677),
                             new Vec3f(-15414, 2200, 20619),
                             new Vec3f(-15432, 2600, 23181),
                        },
                        Enemies = new Pair[]
                        {
                            new Pair(ird_skeleton_l, 1.0f),
                            new Pair(ird_skeleton_m, 0.1f),
                        }
                    },      
                       
                    // letztes Tor
                    new Stand()
                    {
                        Barriers = new Barrier[] { new Barrier("redeye", -17428.9434f, 2596.05835f, 28675.0059f, 0, 0, 0),
                                                   new Barrier("redeye", -17481.6855f, 2593.81982f, 28674.1699f, 0, 0, 0),
                                                   new Barrier("bigdoor_head_right", -17456.8281f, 2612.23511f, 28760.1406f, 0, 0, 0),
                                                   new Barrier("bigdoor_head_left", -17456.8281f, 2612.23511f, 28760.1406f, 0, 0, 0),
                                                   new Barrier("bigdoor_right", -17042.5f, 2611.92554f, 28812.6738f, 0, 0, 0),
                                                   new Barrier("bigdoor_left", -17870f, 2611.92554f, 28812.6738f, 0, 0, 0),},
                        Position = new Vec3f(-17473, 2300, 28468),
                        Range = 400,
                        EnemyCountPerGroup = 0,
                        EnemyGroupsPerSpawn = 0,
                        EnemySpawnInterval = TimeSpan.TicksPerDay,
                        Duration = 11 * TimeSpan.TicksPerSecond,
                        SFXStart = "LASTDOOREVENT.WAV",
                        SFXLoop = null,
                        SFXStop = "STONE_START.WAV",
                        Boss = null,
                        Messages = new string[]
                        {
                            "Fremder tragen Ulumulu, dann Fremder nicht sterben!",
                        },
                        EnemySpawns = new Vec3f[]
                        {

                        },
                        Enemies = new Pair[]
                        {

                        }
                    },  


                    // untoter drache
                    new Stand()
                    {
                        Barriers = new Barrier[] { },
                        Position = new Vec3f(-17573, 3400, 34149),
                        Range = 1500,
                        EnemyCountPerGroup = 2,
                        EnemyGroupsPerSpawn = 2,
                        EnemySpawnInterval = 10 * TimeSpan.TicksPerSecond,
                        Duration = TimeSpan.TicksPerDay,
                        SFXStart = "MYSTERY_07.WAV",
                        SFXLoop = null,
                        SFXStop = null,
                        Boss = new NPCClass()
                        {
                            HP = 2000,
                            Definition = "dragon_undead",
                        },
                        Messages = new string[]
                        {
                            "Fremder tragen Ulumulu, dann Fremder nicht sterben!",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(-20347, 3200, 33691),
                             new Vec3f(-17434, 3200, 36373),
                             new Vec3f(-14717, 3200, 33632),
                        },
                        Enemies = new Pair[]
                        {
                            new Pair(ird_skeleton_l, 1.0f),
                            new Pair(ird_skeleton_m, 0.1f),
                        }
                    }
                },

                AmbientNPCs = new Group[]
                {

                },

                Respawns = new Vec3f[]
                {
                    new Vec3f(692.55f, 2824.994f, 21342.08f),
                    new Vec3f(-8501.721f, -1131.424f, 12540.28f),
                    new Vec3f(-3193.676f, -552.6367f, 3412.632f),
                },
            });

            #endregion
#endif
            #region h_oldmine

            var oldmine_gardist = new NPCClass()
            {
                Name = "Gardist",
                Definition = null,
                Protection = 50,
                Damage = 0,
                HP = 100,
                Overlays = new string[] { "humans_torch", "1HST2"  },
                ItemDefs = new NPCClass.InvItem[]
                {
                      new NPCClass.InvItem("1hschwert"),
                      new NPCClass.InvItem("torch_burning"),
                      new NPCClass.InvItem("ITAR_garde_h"),
                      new NPCClass.InvItem("war_xbow"),
                      new NPCClass.InvItem("itrw_bolt", 20),
                },
                Guild = Allegiance.MIL,
            };

            var oldmine_templer = new NPCClass()
            {
                Name = "Templer",
                Definition = null,
                Protection = 50,
                Damage = 0,
                HP = 100,
                Overlays = new string[] { "2HST2" },
                ItemDefs = new NPCClass.InvItem[]
                {
                      new NPCClass.InvItem("leichter_zweihaender"),
                      new NPCClass.InvItem("ITAR_templer"),
                      new NPCClass.InvItem("light_xbow"),
                      new NPCClass.InvItem("itrw_bolt", 10),
                },
                Guild = Allegiance.SECT,
            };

            var oldmine_crawler = new NPCClass()
            {
                Definition = "minecrawler",
            };

            var oldmine_crawler_w = new NPCClass()
            {
                Definition = "minecrawler_warrior",
            };

            scenarios.Add(new HordeScenario()
            {
                Name = "h_oldmine",
                WorldPath = "H_oldmine.ZEN",
                SpawnPos = new PosAng(5298, -8500, -5475, 0),
                SpawnRange = 100,
                SpawnBarriers = new Barrier[] { new Barrier("gate", 5801.86377f, -8865.28027f, -5538.98682f, 0, 1.5f, 0) },

                SpecPoint = new PosAng(5298, -8500, -5475, 0),
                FightDuration = 30 * TimeSpan.TicksPerMinute,
                WorldTimeScale = 0,
                WorldBarrier = 0,
                WorldWeather = 0,
                PlayerClasses = new NPCClass[]
                {
                    oldmine_gardist,
                    oldmine_templer,
                },

                Enemies = new Group[]
                {                             
                    new Group(9644, -7400, -4503, 400, new Pair(oldmine_crawler, 1)), // anfang
                    new Group(8467, -7000, -2633, 400, new Pair(oldmine_crawler, 1)), // anfang2
                    new Group(5572, -7000, -75, 400, new Pair(oldmine_crawler, 0.5f),
                                                     new Pair(oldmine_crawler_w, 0.3f)), // nach durchgang
                    new Group(9561, -7800, 2385, 400, new Pair(oldmine_crawler, 0.5f),
                                                     new Pair(oldmine_crawler_w, 0.3f)), // vor königin eingang
                },

                Items = new Item[]
                {
                    //new Item("hptrank", 2892.88354f, 6425.26074f, 31699.8496f), // austauschstelle brücke
                },

                Stands = new Stand[]
                {
                    // Vor Tor
                    new Stand()
                    {
                        Barriers = new Barrier[] { },
                        Position = new Vec3f(5298, -8500, -5475),
                        Range = 2000,
                        EnemyCountPerGroup = 0.5f,
                        EnemyGroupsPerSpawn = 2,
                        EnemySpawnInterval = 10 * TimeSpan.TicksPerSecond,
                        Duration = 30 * TimeSpan.TicksPerSecond,
                        SFXStart = "GATE_STOP.WAV",
                        SFXLoop = "",
                        SFXStop = "CRW_PERCEPTION01.WAV",
                        Messages = new string[]
                        {
                            "Tor!",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(7385.34229f, -7975.97852f, -5381.13379f),
                        },
                        Enemies = new Pair[]
                        {
                            new Pair(oldmine_crawler, 1.0f),
                        }
                    },

                    // Hinter Tor
                    new Stand()
                    {
                        Barriers = new Barrier[] { },
                        Position = new Vec3f(9644, -7400, -4503),
                        Range = 500,
                        EnemyCountPerGroup = 0.000001f,
                        EnemyGroupsPerSpawn = 2,
                        EnemySpawnInterval = 20 * TimeSpan.TicksPerSecond,
                        Duration = 10 * TimeSpan.TicksPerSecond,
                        SFXStart = "CRW_WARN01.WAV",
                        SFXLoop = "",
                        SFXStop = "CRW_PERCEPTION01.WAV",
                        Messages = new string[]
                        {
                            "yes",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(10384.0029f, -7521.979f, -3835.53467f),
                             new Vec3f(9994.76855f, -7063.48389f, -3043.97192f),
                        },
                        Enemies = new Pair[]
                        {
                            new Pair(oldmine_crawler, 1.0f),
                        }
                    },
                    
                    // Königin
                    new Stand()
                    {
                        Barriers = new Barrier[] { },
                        Position = new Vec3f(11086, -9000, 7008),
                        Range = 500,
                        EnemyCountPerGroup = 0.5f,
                        EnemyGroupsPerSpawn = 2,
                        EnemySpawnInterval = 1000000 * TimeSpan.TicksPerSecond,
                        Duration = TimeSpan.TicksPerDay,
                        SFXStart = "CRW_WARN01.WAV",
                        SFXLoop = "AMBIENTCRAWLERSCREAM.WAV",
                        SFXStop = "CRW_HURT02.WAV",
                        Boss = new NPCClass()
                        {
                            Definition = "crawler_queen",
                            HP = 3000,
                        },
                        Messages = new string[]
                        {
                            "Königin",
                        },
                        EnemySpawns = new Vec3f[]
                        {
                             new Vec3f(7385.34229f, -7975.97852f, -5381.13379f),
                        },
                        Enemies = new Pair[]
                        {
                            new Pair(oldmine_crawler, 1.0f),
                        }
                    }
                },

                AmbientNPCs = new Group[]
                {                
                    // hinter Burgtor            
                    //new Group(-2844, -230, 1227, 400, 0.4244f, new Pair(pass_Miliz, 6),
                    //                                           new Pair(pass_Ritter, 4)),
                },

                Respawns = new Vec3f[]
                {
                    new Vec3f(692.55f, 2824.994f, 21342.08f),
                    new Vec3f(-8501.721f, -1131.424f, 12540.28f),
                    new Vec3f(-3193.676f, -552.6367f, 3412.632f),
                },
            });

            #endregion
        }
    }
}

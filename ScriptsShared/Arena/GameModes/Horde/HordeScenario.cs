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

        public class Barrier
        {
            public Vec3f Position;
            public Angles Angles;
            public string Definition;
            public bool AddAfterEvent;

            public Barrier(string vobDef, float x, float y, float z, float pitch, float yaw, float roll, bool addAfterEvent = false)
            {
                this.Definition = vobDef;
                this.Position = new Vec3f(x, y, z);
                this.Angles = new Angles(pitch, yaw, roll);
                this.AddAfterEvent = addAfterEvent;
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

        public Vec3f SpawnPos;
        public float SpawnRange;
        public Barrier[] SpawnBarriers;

        public Item[] Items;

        public NPCClass[] PlayerClasses;

        public Group[] Enemies;
        public Stand[] Stands;

        public Group[] AmbientNPCs;

        public Vec3f[] Respawns;


        public static void Init()
        {
            #region h_pass

            var OrcScout = new NPCClass()
            {
                Definition = "orc_scout",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("krush_pach"),
                }
            };

            var OrcWarrior = new NPCClass()
            {
                Definition = "orc_warrior",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("krush_pach"),
                }
            };

            var OrcElite = new NPCClass()
            {
                Definition = "orc_elite",
                ItemDefs = new NPCClass.InvItem[]
                {
                    new NPCClass.InvItem("orc_sword"),
                }
            };


            var Miliz = new NPCClass()
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

            var Ritter = new NPCClass()
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
                SpawnPos = new Vec3f(7695, 6509, 42836),
                SpawnRange = 100,
                SpecPoint = new PosAng(7583, 6759, 42869, -0.1745329f, 2.446952f, 0),
                FightDuration = 30 * TimeSpan.TicksPerMinute,
                WorldTime = new WorldTime(0, 22),
                WorldTimeScale = 5,
                WorldBarrier = 0,
                WorldWeather = -1,
                PlayerClasses = new NPCClass[]
                {
                    Miliz,
                    Ritter,
                },

                SpawnBarriers = new Barrier[]
                {
                    new Barrier("invwall", 6551, 6447, 40913, 0, 3.032f, 0),
                    new Barrier("trollpalisade", 6555, 6544, 40671, 0.313f, 3.054f, 0.035f),
                },

                Enemies = new Group[]
                {                
                    // 1. SEKTION, bis Steindurchgang vor verlassener Mine                
                    new Group(8094, 5626, 36860, 600, new Pair(OrcScout, 0.3f)), // plattform
                    new Group(5581, 5510, 35969, 400, new Pair(OrcScout, 0.5f)), // see
                    new Group(4227, 6145, 31128, 500, new Pair(OrcScout, 1.0f)), // steindurchgang
                
                    // 2. SEKTION, bis Weg am Hang, bei Drachenjägern
                    new Group(4102, 5888, 27445, 500, new Pair(OrcScout, 1.0f)), // verlassene Mine
                    new Group( 678, 6235, 27186, 500, new Pair(OrcWarrior, 0.2f), // bei teleport rune
                                                      new Pair(OrcScout, 0.3f)),
                    new Group(1555, 2900, 21881, 700, new Pair(OrcWarrior, 0.3f), // toter paladin
                                                      new Pair(OrcScout, 1.0f)),
                
                    // 3. SEKTION, bis Brücke am Fluss
                    new Group(2370, 1665, 18719, 200, new Pair(OrcScout, 0.2f)), // weg am hang 1
                    new Group(-516, -280, 14668, 200, new Pair(OrcWarrior, 0.2f)), // weg am hang 2
                    new Group(-5903, -900, 14530, 1500, new Pair(OrcWarrior, 0.5f), // bei drax / jaegern
                                                        new Pair(OrcScout, 2.0f)),
                    new Group(-11240, -705, 12303, 1500, new Pair(OrcWarrior, 0.75f), // vor brücke
                                                         new Pair(OrcScout, 2.0f)),
                    new Group(-11003, -820, 9127, 140, new Pair(OrcElite, 0.2f)), // auf brücke
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
                            new Pair(OrcScout, 1.0f),
                            new Pair(OrcWarrior, 0.8f),
                            new Pair(OrcElite, 0.5f),
                        }
                    }
                },

                AmbientNPCs = new Group[]
                {                
                    // hinter Burgtor            
                    new Group(-2844, -230, 1227, 400, 0.4244f, new Pair(Miliz, 6),
                                                               new Pair(Ritter, 4)),
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
        }
    }
}

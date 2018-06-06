using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Utilities;

namespace GUC.Scripts.Arena
{
    partial class HordeDef
    {
        public string Name;
        public string WorldPath;

        public Vec3f SpawnPos;
        public float SpawnRange;
        public HordeBarrier[] SpawnBarriers;

        public HordeItem[] Items;

        public HordeClassDef[] Classes;

        public HordeGroup[] Enemies;
        public HordeStand[] Stands;

        public PosAng SpecPA;
        
        static Dictionary<string, HordeDef> defs = new Dictionary<string, HordeDef>();
        public static HordeDef GetDef(string defName) { return defs.ContainsKey(defName) ? defs[defName] : null; }
        public static IEnumerable<HordeDef> GetAll() { return defs.Values; }

        static HordeDef()
        {
            #region h_pass

            HordeDef def = new HordeDef()
            {
                Name = "h_pass",
                WorldPath = "H_PASS.ZEN",
                SpawnPos = new Vec3f(7695, 6509, 42836),
                SpawnRange = 100,

                SpecPA = new PosAng(7695, 6509, 42836),
            };

            def.Classes = new HordeClassDef[]
            {
                new HordeClassDef()
                {
                    Name = "Miliz",
                    Equipment = new List<string>() { "ITAR_miliz_s", "1hschwert", "light_xbow" },
                    NeedsBolts = true,
                },
                new HordeClassDef()
                {
                    Name = "Ritter",
                    Equipment = new List<string>() { "ITAR_ritter", "2hschwert" },
                    NeedsBolts = false,
                },
             };

            // INTRO SEKTION, EINGANG PASS
            def.SpawnBarriers = new HordeBarrier[]
            {
                new HordeBarrier("invwall", new Vec3f(6551, 6447, 40913), new Angles(0, 3.032f, 0)),
                new HordeBarrier("trollpalisade", new Vec3f(6555, 6544, 40671), new Angles(0.313, 3.054, 0.035)),
            };

            var OrcScout = new HordeEnemy("orc_scout", "krush_pach");
            var OrcWarrior = new HordeEnemy("orc_warrior", "krush_pach");
            var OrcElite = new HordeEnemy("orc_elite", "orc_sword");

            def.Enemies = new HordeGroup[]
            {
                // 1. SEKTION, bis Steindurchgang vor verlassener Mine                
                new HordeGroup(8094, 5626, 36860, 600, new HordePair(OrcScout, 0.3f)), // plattform
                new HordeGroup(5581, 5510, 35969, 400, new HordePair(OrcScout, 0.5f)), // see
                new HordeGroup(4227, 6145, 31128, 500, new HordePair(OrcScout, 1.0f)), // steindurchgang
                
                // 2. SEKTION, bis Weg am Hang, bei Drachenjägern
                new HordeGroup(4102, 5888, 27445, 500, new HordePair(OrcScout, 1.0f)), // verlassene Mine
                new HordeGroup( 678, 6235, 27186, 500, new HordePair(OrcWarrior, 0.2f), // bei teleport rune
                                                       new HordePair(OrcScout, 0.3f)),
                new HordeGroup(1555, 2900, 21881, 700, new HordePair(OrcWarrior, 0.3f), // toter paladin
                                                       new HordePair(OrcScout, 1.0f)),
                
                // 3. SEKTION, bis Brücke am Fluss
                new HordeGroup(2370, 1665, 18719, 200, new HordePair(OrcScout, 0.2f)), // weg am hang 1
                new HordeGroup(-516, -280, 14668, 200, new HordePair(OrcWarrior, 0.2f)), // weg am hang 2
                new HordeGroup(-5903, -900, 14530, 1500, new HordePair(OrcWarrior, 0.5f), // bei drax / jaegern
                                                       new HordePair(OrcScout, 2.0f)),
                new HordeGroup(-11240, -705, 12303, 1500, new HordePair(OrcWarrior, 0.75f), // vor brücke
                                                       new HordePair(OrcScout, 2.0f)),
                new HordeGroup(-11003, -820, 9127, 140, new HordePair(OrcElite, 0.2f)), // auf brücke
            };

            def.Stands = new HordeStand[]
            {
                // LETZTE SEKTION, vor Burgtor
                new HordeStand()
                {
                    Barriers = new HordeBarrier[] { new HordeBarrier("gate", new Vec3f(-3069, -496, 1800),  new Angles(0, -2.793, 0)) },
                    Position = new Vec3f(-3352, -500, 2491),
                    Range = 1000,
                    MaxEnemies = 3,
                    Duration = 180,
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
                        "\"Brutus, helf mal mit deinen fetten Arme aus!\"",
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
                    Enemies = new HordePair[]
                    {
                        new HordePair(OrcScout, 0.5f),
                        new HordePair(OrcWarrior, 0.8f),
                        new HordePair(OrcElite, 1.0f),
                    }
                },
            };

            /*section.bridges = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-11011, -947, 9555), Angles = new Angles(0.000, 1.607, 0.122) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-11007, -948, 9339), Angles = new Angles(0.000, 1.716, 0.000) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10994, -977, 9052), Angles = new Angles(-0.017, 1.714, 0.070) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10969, -984, 8762), Angles = new Angles(0.000, 1.716, 0.000) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10948, -1002, 8471), Angles = new Angles(0.000, 1.716, 0.000) },
            };*/

            def.Items = new HordeItem[]
            {
                new HordeItem("hptrank", 2892.88354f, 6425.26074f, 31699.8496f), // austauschstelle brücke
                new HordeItem("hptrank", 5053.02588f, 5824.37646f, 25956.3828f), // verlassener minenschacht


                new HordeItem("hptrank", -346.506165f, 6248.29785f, 27447.5215f), // teleport rune
                new HordeItem("hptrank", -351.038818f, 6247.33643f, 27468.8926f),
                new HordeItem("hptrank", -329.380737f, 6246.375f, 27458.5488f),

                new HordeItem("hptrank", 847.420288f, 2719.80615f, 21308.3008f), // toter paladin

                new HordeItem("hptrank", -6164.2998f, -641.647034f, 16228.6006f), // drax / jäger
                
                new HordeItem("hptrank", -7673.05566f, -534.731506f, 11694.4043f), // hügel vor brücke
                new HordeItem("hptrank", -7691.25537f, -534.557861f, 11677.5244f),

                new HordeItem("hptrank", -9071.97656f, -1346.44824f, 6379.68262f), // baum hinter brücke

                new HordeItem("hptrank", -13201.7236f, -795.982117f, 5463.70215f), // höhle hinter brücke
                new HordeItem("hptrank", -13217.3105f, -795.981567f, 5470.14404f),

                new HordeItem("hptrank", -3203.54468f, -650.505127f, 3661.7583f), // verbranntes haus vor tor

                new HordeItem("hptrank", -6117.55029f, -998.722168f, 720.570923f), // verbranntes haus an mauer
            };

            defs.Add(def.Name, def);

            #endregion
        }
    }
}

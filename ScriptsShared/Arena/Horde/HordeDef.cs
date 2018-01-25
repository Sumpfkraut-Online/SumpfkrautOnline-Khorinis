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

        public List<HordeClassDef> Classes;

        public List<HordeSection> Sections = new List<HordeSection>();

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
            };

            def.Classes = new List<HordeClassDef>()
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
            HordePath section = new HordePath()
            {
                SpawnPos = new Vec3f(7695, 6509, 42836),
                SpawnRange = 100,
                SecsTillNext = 3,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
                FinishedMessage = "Ihr bereitet euch darauf vor, den Weg zur Burg freizukämpfen.",
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "invwall", Position = new Vec3f(6551, 6447, 40913), Angles = new Angles(0.000, 3.032, 0.000) },
                new HordeBarrier() { Definition = "trollpalisade", Position = new Vec3f(6555, 6544, 40671), Angles = new Angles(0.313, 3.054, 0.035) },
            };
            section.groups = new List<HordeGroup>();
            def.Sections.Add(section);

            // 1. SEKTION, bis Steindurchgang vor verlassener Mine
            section = new HordePath()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 3,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
                FinishedMessage = "Ihr beginnt die Palisade einzureissen.",
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "invwall", Position = new Vec3f(4389, 6038, 30560), Angles = new Angles(0.000, -2.832, 0.000) },
                new HordeBarrier() { Definition = "trollpalisade", Position = new Vec3f(4413, 6067, 30325), Angles = new Angles(0.226, -2.840, -0.017) },
            };
            section.groups = new List<HordeGroup>()
            {
                new HordeGroup() { Position = new Vec3f(8094, 5626, 36860), Range = 600, npcs = new HordeEnemy[]
                    { // plattform
                        new HordeEnemy("orc_scout", 0.3f, "krush_pach"),
                    }
                },
                new HordeGroup() { Position = new Vec3f(5581, 5510, 35969), Range = 400, npcs = new HordeEnemy[]
                    { // see
                        new HordeEnemy("orc_scout", 0.5f, "krush_pach"),
                    },
                },
                new HordeGroup() { Position = new Vec3f(4227, 6145, 31128), Range = 500, npcs = new HordeEnemy[]
                    { // vor barrikaden, steindurchgang
                        new HordeEnemy("orc_scout", 1, "krush_pach"),
                    },
                },
            };
            def.Sections.Add(section);

            // 2. SEKTION, bis Weg am Hang, bei Drachenjägern
            section = new HordePath()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 3,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
                FinishedMessage = "Ihr beginnt die Palisade einzureissen.",
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "invwall", Position = new Vec3f(2989, 1915, 19515), Angles = new Angles(0.000, 2.945, 0.000) },
                new HordeBarrier() { Definition = "trollpalisade", Position = new Vec3f(2655, 2004, 19422), Angles = new Angles(0.000, 2.976, 0.000) },
            };

            section.groups = new List<HordeGroup>()
            {
                new HordeGroup() { Position = new Vec3f(4603, 6076, 28529), Range = 400, npcs = new HordeEnemy[]
                    { // verlassene mine
                        new HordeEnemy("orc_scout", 1, "krush_pach" ),
                    }
                },
                new HordeGroup() { Position = new Vec3f(4102, 5888, 27445), Range = 500, npcs = new HordeEnemy[]
                    { // verlassene mine 2
                        new HordeEnemy("orc_scout", 1, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(678, 6235, 27186), Range = 500, npcs = new HordeEnemy[]
                    { // bei teleport rune
                        new HordeEnemy("orc_warrior", 0.2f, "krush_pach" ),
                        new HordeEnemy("orc_scout", 0.3f, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(1555, 2900, 21881), Range = 700, npcs = new HordeEnemy[]
                    { // vor barrikade, toter paladin
                        new HordeEnemy("orc_warrior", 0.3f, "krush_pach" ),
                        new HordeEnemy("orc_scout", 1, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(3531, 2400, 20153), Range = 200, npcs = new HordeEnemy[]
                    { // vor barrikade, abgrund
                        new HordeEnemy("orc_scout", 0.2f, "krush_pach" ),
                    },
                },
            };
            def.Sections.Add(section);

            // 3. SEKTION, bis Brücke am Fluss
            section = new HordePath()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 3,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
                FinishedMessage = "Ihr beginnt eine Brücke zu bauen.",
            };

            section.bridges = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-11011, -947, 9555), Angles = new Angles(0.000, 1.607, 0.122) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-11007, -948, 9339), Angles = new Angles(0.000, 1.716, 0.000) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10994, -977, 9052), Angles = new Angles(-0.017, 1.714, 0.070) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10969, -984, 8762), Angles = new Angles(0.000, 1.716, 0.000) },
                new HordeBarrier() { Definition = "planks", Position = new Vec3f(-10948, -1002, 8471), Angles = new Angles(0.000, 1.716, 0.000) },
            };
            section.groups = new List<HordeGroup>()
            {
                new HordeGroup() { Position = new Vec3f(2370, 1665, 18719), Range = 200, npcs = new HordeEnemy[]
                    { // weg am hang 1
                        new HordeEnemy( "orc_scout", 0.2f, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(709, 880, 17312), Range = 200, npcs = new HordeEnemy[]
                    { // weg am hang 2
                        new HordeEnemy( "orc_warrior", 0.2f, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(-516, -280, 14668), Range = 200, npcs = new HordeEnemy[]
                    { // weg am hang 3
                        new HordeEnemy("orc_warrior", 0.2f, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(-5903, -900, 14530), Range = 1500, npcs = new HordeEnemy[]
                    { // bei drax / jaegern
                        new HordeEnemy("orc_warrior", 0.5f, "krush_pach" ),
                        new HordeEnemy( "orc_scout", 2, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(-7665, -435, 11474), Range = 500, npcs = new HordeEnemy[]
                    { // auf huegel
                        new HordeEnemy( "orc_warrior", 0.25f, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(-11240, -705, 12303), Range = 1500, npcs = new HordeEnemy[]
                    { // vor brücke
                        new HordeEnemy( "orc_warrior", 0.75f, "krush_pach" ),
                        new HordeEnemy( "orc_scout", 2, "krush_pach" ),
                    },
                },
                new HordeGroup() { Position = new Vec3f(-10994, -890, 9907), Range = 140, npcs = new HordeEnemy[]
                    { // vor barrikade, auf brücke
                        new HordeEnemy( "orc_elite", 0.2f, "orc_sword" ),
                    },
                },
            };
            def.Sections.Add(section);

            // LETZTE SEKTION, bis Burgtor
            var endSection = new HordeHill()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 30,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
                FinishedMessage = "Ziel erreicht.",
            };

            endSection.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "gate", Position = new Vec3f(-3069, -496, 1800), Angles = new Angles(0.000, -2.793, 0.000) },
            };

            endSection.npcTarget = new Vec3f(-4307, -690, 3687);
            endSection.npcSpawns = new Vec3f[]
            {
                 new Vec3f(-9010, -830, 2986),
                 new Vec3f(-9618, -660, 1485),
                 new Vec3f(-8196, -510, 412),
                 new Vec3f(-1546, -1010, 7224),
                 new Vec3f(163, -1130, 6784),
                 new Vec3f(289, -960, 4472),
            };

            endSection.groups = new List<HordeEnemy[]>()
            {
                new HordeEnemy[] { new HordeEnemy( "orc_scout", 1.5f, "krush_pach" ) },
                new HordeEnemy[] { new HordeEnemy( "orc_scout", 2f, "krush_pach" ), new HordeEnemy( "orc_warrior", 0.5f, "krush_pach" ) },
                new HordeEnemy[] { new HordeEnemy( "orc_warrior", 2f, "krush_pach" ), new HordeEnemy( "orc_warrior", 2f, "krush_pach" ) },
                new HordeEnemy[] { new HordeEnemy( "orc_scout", 1f, "krush_pach" ), new HordeEnemy( "orc_warrior", 1f, "krush_pach" ), new HordeEnemy( "orc_elite", 0.3f, "orc_sword") },
                new HordeEnemy[] { new HordeEnemy( "orc_scout", 1.5f, "krush_pach" ), new HordeEnemy( "orc_warrior", 1f, "krush_pach" ), new HordeEnemy( "orc_elite", 0.8f, "orc_sword") },
            };

            def.Sections.Add(endSection);



            defs.Add(def.Name, def);

            #endregion
        }
    }
}

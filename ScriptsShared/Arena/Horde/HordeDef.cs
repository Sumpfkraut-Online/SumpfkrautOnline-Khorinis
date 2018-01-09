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
             };

            // INTRO SEKTION, EINGANG PASS
            HordeSection section = new HordeSection()
            {
                SpawnPos = new Vec3f(7695, 6509, 42836),
                SpawnRange = 100,
                SecsTillNext = 30,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
                FinishedMessage = "Test und so",
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(6414, 6426, 41209), Angles = new Angles(0.162, -2.997, 0.042) },
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(6877, 6446, 41255), Angles = new Angles(0.156, -3.039, 0.035) },
            };
            section.groups = new List<HordeGroup>();
            def.Sections.Add(section);

            // 1. SEKTION, bis Steindurchgang vor verlassener Mine
            section = new HordeSection()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 30,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(4584, 6042, 30454), Angles = new Angles(0.211, -2.902, -0.040) },
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(4160, 6027, 30361), Angles = new Angles(0.175, -2.888, 0.052) },
            };
            section.groups = new List<HordeGroup>()
            {
                new HordeGroup() { Position = new Vec3f(8094, 5626, 36860), Range = 600, npcs = new List<string, float>()
                    { // plattform
                        { "orc_scout", 0.3f },
                    }
                },
                new HordeGroup() { Position = new Vec3f(5581, 5510, 35969), Range = 400, npcs = new List<string, float>()
                    { // see
                        { "orc_scout", 0.5f },
                    },
                },
                new HordeGroup() { Position = new Vec3f(4227, 6145, 31128), Range = 500, npcs = new List<string, float>()
                    { // vor barrikaden, steindurchgang
                        { "orc_scout", 1 },
                    },
                },
            };
            def.Sections.Add(section);

            // 2. SEKTION, bis Weg am Hang, bei Drachenjägern
            section = new HordeSection()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 30,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(2940, 1839, 19263), Angles = new Angles(0.175, 2.977, -0.017) },
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(2499, 1837, 19337), Angles = new Angles(0.192, 2.980, 0.035) },
            };

            section.groups = new List<HordeGroup>()
            {
                new HordeGroup() { Position = new Vec3f(4603, 6076, 28529), Range = 400, npcs = new List<string, float>()
                    { // verlassene mine
                        { "orc_scout", 1 },
                    }
                },
                new HordeGroup() { Position = new Vec3f(4102, 5888, 27445), Range = 500, npcs = new List<string, float>()
                    { // verlassene mine 2
                        { "orc_scout", 1 },
                    },
                },
                new HordeGroup() { Position = new Vec3f(678, 6235, 27186), Range = 500, npcs = new List<string, float>()
                    { // bei teleport rune
                        { "orc_warrior", 0.2f },
                        { "orc_scout", 0.3f },
                    },
                },
                new HordeGroup() { Position = new Vec3f(1555, 2900, 21881), Range = 700, npcs = new List<string, float>()
                    { // vor barrikade, toter paladin
                        { "orc_warrior", 0.3f },
                        { "orc_scout", 1 },
                    },
                },
                new HordeGroup() { Position = new Vec3f(3531, 2400, 20153), Range = 200, npcs = new List<string, float>()
                    { // vor barrikade, abgrund
                        { "orc_scout", 0.2f },
                    },
                },
            };
            def.Sections.Add(section);

            // 3. SEKTION, bis Brücke am Fluss
            section = new HordeSection()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 30,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(-10966, -925, 9085), Angles = new Angles(0.000, 2.980, 0.000) },
            };
            section.groups = new List<HordeGroup>()
            {
                new HordeGroup() { Position = new Vec3f(2370, 1665, 18719), Range = 200, npcs = new List<string, float>()
                    { // weg am hang 1
                        { "orc_scout", 0.2f },
                    },
                },
                new HordeGroup() { Position = new Vec3f(709, 880, 17312), Range = 200, npcs = new List<string, float>()
                    { // weg am hang 2
                        { "orc_scout", 0.2f },
                    },
                },
                new HordeGroup() { Position = new Vec3f(-516, -280, 14668), Range = 200, npcs = new List<string, float>()
                    { // weg am hang 3
                        { "orc_scout", 0.2f },
                    },
                },
                new HordeGroup() { Position = new Vec3f(-5903, -900, 14530), Range = 1500, npcs = new List<string, float>()
                    { // bei drax / jaegern
                        { "orc_warrior", 0.5f },
                        { "orc_scout", 2 },
                    },
                },
                new HordeGroup() { Position = new Vec3f(-7665, -435, 11474), Range = 500, npcs = new List<string, float>()
                    { // auf huegel
                        { "orc_warrior", 0.25f },
                    },
                },
                new HordeGroup() { Position = new Vec3f(-11240, -705, 12303), Range = 1500, npcs = new List<string, float>()
                    { // vor brücke
                        { "orc_warrior", 0.75f },
                        { "orc_scout", 2 },
                    },
                },
                new HordeGroup() { Position = new Vec3f(-10975, -810, 9333), Range = 150, npcs = new List<string, float>()
                    { // vor barrikade, auf brücke
                        { "orc_elite", 0.2f },
                    },
                },
            };
            def.Sections.Add(section);

            // LETZTE SEKTION, bis Burgtor
            section = new HordeSection()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 30,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "gate", Position = new Vec3f(-3069, -496, 1800), Angles = new Angles(0.000, -2.793, 0.000) },
            };
            section.groups = new List<HordeGroup>()
            {
                new HordeGroup() { Position = new Vec3f(0, 400, 0), Range = 400, npcs = new List<string, float>()
                    {
                        { "skeleton", 3 },
                        { "skeleton_lord", 1 }
                    }
                }
            };
            def.Sections.Add(section);



            defs.Add(def.Name, def);

            #endregion
        }
    }
}

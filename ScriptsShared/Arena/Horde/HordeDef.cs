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

            HordeSection section = new HordeSection()
            {
                SpawnPos = new Vec3f(0, 400, 0),
                SpawnRange = 500,
                SecsTillNext = 1000,
                SpecPos = new Vec3f(0, 400, 0),
                SpecAng = new Angles(0, 0, 0),
            };

            section.barriers = new List<HordeBarrier>()
            {
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(6414, 6426, 41209), Angles = new Angles(0.162, -2.997, 0.042) },
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(6877, 6446, 41255), Angles = new Angles(0.156, -3.039, 0.035) },
            };
            section.groups = new List<HordeGroup>();
            def.Sections.Add(section);

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
                new HordeBarrier() { Definition = "barrier", Position = new Vec3f(0, 200, -100) }
            };
            section.groups = new List<HordeGroup>()
            {
                new HordeGroup() { Position = new Vec3f(0, 200, 0), Range = 200, npcs = new Utilities.List<string, int>()
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

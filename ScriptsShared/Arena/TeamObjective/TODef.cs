using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;
using GUC.Utilities;
using GUC.Log;

namespace GUC.Scripts.Arena
{
    enum TOPhases
    {
        None,
        Warmup,
        Battle,
        Finish
    }

    class TODef
    {
        string worldPath;
        public string WorldPath { get { return this.worldPath; } }

        string name;
        public string Name { get { return this.name; } }

        int duration = 5;
        /// <summary> In Minutes </summary>
        public int Duration { get { return duration; } }

        float maxWorldDistance = float.MaxValue;
        public float MaxWorldDistance { get { return maxWorldDistance; } }

        float maxHeight = float.MaxValue;
        public float MaxHeight { get { return maxHeight; } }

        float maxDepth = float.MinValue;
        public float MaxDepth { get { return maxDepth; } }

        int scoreToWin;
        public int ScoreToWin { get { return scoreToWin; } }

        ValueTuple<Vec3f, Vec3f> specPos;
        public ValueTuple<Vec3f, Vec3f> SpecPos { get { return this.specPos; } }

        List<TOTeamDef> teams = new List<TOTeamDef>();
        public ReadOnlyList<TOTeamDef> Teams { get { return teams; } }

        #region AddTeams

        void AddTeam(string name, List<Vec3f, Vec3f> spawnPoints, List<TOClassDef> classDefs)
        {
            this.AddTeam(name, spawnPoints, classDefs, ColorRGBA.White);
        }

        void AddTeam(string name, List<Vec3f, Vec3f> spawnPoints, List<TOClassDef> classDefs, ColorRGBA teamColor)
        {
            teams.Add(new TOTeamDef(name, spawnPoints, classDefs, teamColor));
        }

        #endregion

        #region Static Collection
        static Dictionary<string, TODef> objectives = new Dictionary<string, TODef>(StringComparer.OrdinalIgnoreCase);
        public static TODef TryGet(string name)
        {
            TODef result;
            if (objectives.TryGetValue(name, out result))
                return result;
            return null;
        }
        public static IEnumerable<TODef> GetAll()
        {
            return objectives.Values;
        }
        #endregion

        static TODef()
        {
            try
            {
                #region TDM Burg

                var to = new TODef();
                to.name = "tdm_burg";
                to.worldPath = "G1-OLDCAMP.ZEN";
                to.duration = 1; // 5 min
                to.scoreToWin = 100;
                to.maxDepth = -400;
                to.specPos = new ValueTuple<Vec3f, Vec3f>(new Vec3f(-6489, -480, 3828), new Vec3f(0.910f, -0.063f, -0.409f));

                // TEAM ALTES LAGER
                var spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(3326.211f, 248.1061f, 1184.516f), new Vec3f(-0.6263769f, 0f, -0.7795203f) },
                    { new Vec3f(2990.146f, 248.1345f, 1262.833f), new Vec3f(-0.03111971f, 0f, -0.9995157f) },
                    { new Vec3f(3504.037f, 368.1083f, 930.0696f), new Vec3f(-0.8308224f, 0f, -0.5565377f) },
                    { new Vec3f(3403.248f, 248.1132f, 545.0029f), new Vec3f(-0.9917087f, 0f, 0.1285065f) },
                    { new Vec3f(1969.982f, 248.1036f, 1537.685f), new Vec3f(0.4323488f, 0f, -0.9017066f) },
                    { new Vec3f(2255.204f, 248.103f, 1487.472f), new Vec3f(0.05611284f, 0f, -0.9984244f) },
                    { new Vec3f(1990.504f, 248.1207f, 1184.365f), new Vec3f(0.4038123f, 0f, -0.9148418f) },
                };
                var npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Gardist", null, new List<string, int>()
                    {
                        { "2hschwert", 1 }, { "itar_garde", 1}
                    },
                    new List<string>() { "2HST1"}),
                    new TOClassDef("Schatten", null, new List<string, int>()
                    {
                        { "1hschwert", 1 }, { "itar_schatten", 1}
                    },
                    new List<string>() { "1HST1"}),
                };
                to.AddTeam("Team Gomez", spawnPoints, npcDefs, ColorRGBA.Red);

                // TEAM NEUES LAGER
                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(-4017.24f, -284.7278f, -2731.3f), new Vec3f(0.2280677f, 0f, 0.9736453f) },
                    { new Vec3f(-4018.768f, -261.8669f, -2362.408f), new Vec3f(-0.07004708f, 0f, 0.9975438f) },
                    { new Vec3f(-3538.857f, -191.6633f, -2032.22f), new Vec3f(-0.7315525f, 0f, 0.6817852f) },
                    { new Vec3f(-4627.698f, -280.9207f, -2213.329f), new Vec3f(0.7590815f, 0f, 0.6509956f) },
                    { new Vec3f(-4207.483f, -284.7982f, -2516.898f), new Vec3f(0.1561466f, 0f, 0.987734f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Söldner", null, new List<string, int>()
                    {
                        { "2haxt", 1 }, { "itar_söldner", 1}
                    },
                    new List<string>() { "2HST2"}),
                    new TOClassDef("Bandit", null, new List<string, int>()
                    {
                        { "1haxt", 1 }, { "itar_bandit", 1}
                    },
                    new List<string>() { "1HST2"}),
                };
                to.AddTeam("Tetriandoch", spawnPoints, npcDefs, ColorRGBA.Blue);

                objectives.Add(to.name, to);
                #endregion
                
                #region TDM Mine

                to = new TODef();
                to.name = "tdm_mine";
                to.worldPath = "G1-OLDMINE.ZEN";
                to.duration = 1; // 5 min
                to.scoreToWin = 100;
                to.maxHeight = -6800;

                // TEAM ALTES LAGER
                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(1960.628f, -8533.601f, -703.8072f), new Vec3f(0.8753238f, 0f, -0.4835372f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Templer", null, new List<string, int>()
                    {
                        { "leichter_zweihaender", 1 }, { "itar_templer", 1}
                    },
                    new List<string>() { "2HST2"}),

                    new TOClassDef("Gardist", null, new List<string, int>()
                    {
                        { "grobes_schwert", 1 }, { "itar_garde_l", 1}
                    }, 
                    new List<string>() { "2HST1"}),
                };
                to.AddTeam("Team Gomez", spawnPoints, npcDefs, ColorRGBA.Red);

                // TEAM MINECRAWLER
                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(5185.921f, -8559.107f, -5453.461f), new Vec3f(-0.9968022f, 0f, 0.07990983f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Minecrawler-Krieger", "minecrawler_warrior"),
                    new TOClassDef("Minecrawler", "minecrawler"),
                };
                to.AddTeam("Schachtcrew", spawnPoints, npcDefs, ColorRGBA.White);

                objectives.Add(to.name, to);
                #endregion

                #region TDM Pass

                to = new TODef();
                to.name = "tdm_pass";
                to.worldPath = "G2-PASS.ZEN";
                to.duration = 1; // 5 min
                to.scoreToWin = 100;
                
                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(4083.011f, 6138.141f, 30684.28f), new Vec3f(-0.3092942f, 0f, 0.9509664f) },
                    { new Vec3f(4432.577f, 6145.912f, 30784.88f), new Vec3f(-0.7146769f, 0f, 0.6994548f) },
                    { new Vec3f(4515.723f, 6214.495f, 31135.37f), new Vec3f(-0.733928f, 0f, 0.6792273f) },
                    { new Vec3f(4226.368f, 6138.119f, 30956.85f), new Vec3f(-0.4133703f, 0f, 0.910563f) },
                    { new Vec3f(3931.663f, 6138.209f, 30855.82f), new Vec3f(-0.4323492f, 0f, 0.9017063f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Ritter", null, new List<string, int>()
                    {
                        { "2hschwert", 1 }, { "itar_ritter", 1}
                    },
                    new List<string>() { "2HST1"}),
                    new TOClassDef("Miliz", null, new List<string, int>()
                    {
                        { "1hschwert", 1 }, { "itar_miliz_s", 1}
                    },
                    new List<string>() { "1HST1"}),
                };
                to.AddTeam("Paladine", spawnPoints, npcDefs, ColorRGBA.White);
                
                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(8183.059f, 6707.462f, 42760.34f), new Vec3f(-0.9383943f, 0f, -0.3455663f) },
                    { new Vec3f(7968.539f, 6534.874f, 43064.72f), new Vec3f(-0.7239747f, 0f, -0.6898266f) },
                    { new Vec3f(7353.229f, 6600.91f, 42973.42f), new Vec3f(-0.3515739f, 0f, -0.9361602f) },
                    { new Vec3f(7752.142f, 6716.781f, 43286.46f), new Vec3f(-0.4845557f, 0f, -0.8747605f) },
                    { new Vec3f(7539.996f, 6562.836f, 42778.45f), new Vec3f(-0.6868766f, 0f, -0.726774f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Ork-Elite", "orc_elite", new List<string, int>()
                    {
                        { "orc_sword", 1 }
                    }),
                    new TOClassDef("Ork-Krieger", "orc_warrior", new List<string, int>()
                    {
                        { "krush_pach", 1 }
                    }),
                };
                to.AddTeam("Passorks", spawnPoints, npcDefs, ColorRGBA.Blue);

                objectives.Add(to.name, to);
                #endregion

                #region TDM Temple

                to = new TODef();
                to.name = "tdm_temple";
                to.worldPath = "ADDON-TEMPLE.ZEN";
                to.duration = 1; // 5 min
                to.scoreToWin = 100;

                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(-19961.6875f, -2855.99927f, -15386.585f), new Vec3f(0f, 0f, 1f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Bandit2", null, new List<string, int>()
                    {
                        { "2hschwert", 1 }, { "itar_bandit", 1}
                    },
                    new List<string>() { "2HST1"}),
                    new TOClassDef("Bandit", null, new List<string, int>()
                    {
                        { "1hschwert", 1 }, { "itar_bandit", 1}
                    },
                    new List<string>() { "1HST1"}),
                };
                to.AddTeam("Ravens Banditen", spawnPoints, npcDefs, ColorRGBA.White);

                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(-22149.0176f, -3007.20508f, -26048.7871f), new Vec3f(0f, 0f, 1f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Skelett", null, new List<string, int>()
                    {
                        { "orc_sword", 1 }
                    }),
                    new TOClassDef("Zombie", null, new List<string, int>()
                    {
                        { "krush_pach", 1 }
                    }),
                };
                to.AddTeam("Armee der Untoten", spawnPoints, npcDefs, ColorRGBA.Blue);

                objectives.Add(to.name, to);
                #endregion
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}

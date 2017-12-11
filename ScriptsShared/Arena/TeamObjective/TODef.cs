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

        ValueTuple<Vec3f, Angles> specPos;
        public ValueTuple<Vec3f, Angles> SpecPos { get { return this.specPos; } }

        List<TOTeamDef> teams = new List<TOTeamDef>();
        public ReadOnlyList<TOTeamDef> Teams { get { return teams; } }

        #region AddTeams

        void AddTeam(string name, List<Vec3f, Angles> spawnPoints, List<TOClassDef> classDefs)
        {
            this.AddTeam(name, spawnPoints, classDefs, ColorRGBA.White);
        }

        void AddTeam(string name, List<Vec3f, Angles> spawnPoints, List<TOClassDef> classDefs, ColorRGBA teamColor)
        {
            teams.Add(new TOTeamDef(name, spawnPoints, classDefs, teamColor));
        }

        #endregion

        #region Static Collection
        static Dictionary<string, TODef> objectives = new Dictionary<string, TODef>(StringComparer.OrdinalIgnoreCase);
        public static TODef TryGet(string name)
        {
            if (objectives.TryGetValue(name, out TODef result))
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
                to.duration = 10; // 5 min
                to.scoreToWin = 50;
                to.maxDepth = -400;
                to.specPos = new ValueTuple<Vec3f, Angles>(new Vec3f(-2442.949f, 676.9498f, 412.3001f), new Angles(-0.04537845f, -0.8307772f, 0f));

                // TEAM ALTES LAGER
                var spawnPoints = new List<Vec3f, Angles>()
                {
                    { new Vec3f(3326.211f, 248.1775f, 1184.516f), new Angles(0f, 2.523746f, 0f) },
                    { new Vec3f(2996.751f, 248.1097f, 1348.412f), new Angles(0f, 2.9147f, 0f) },
                    { new Vec3f(3485.681f, 368.1476f, 963.361f), new Angles(0f, 1.912882f, 0f) },
                    { new Vec3f(2278.622f, 248.1326f, 1518.721f), new Angles(0f, -2.911208f, 0f) },
                    { new Vec3f(2036.856f, 248.1774f, 1372.729f), new Angles(0f, -2.565633f, 0f) },
                    { new Vec3f(1895.824f, 248.1548f, 864.6687f), new Angles(0f, -1.731365f, 0f) },
                };
                var npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Gardist", null, new List<string, int>()
                    {
                        { "2hschwert", 1 }, { "itar_garde", 1 }, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    },
                    new List<string>() { "2HST1"}),
                    new TOClassDef("Schatten", null, new List<string, int>()
                    {
                        { "1hschwert", 1 }, { "itar_schatten", 1 }, { "itrw_longbow", 1}, { "itrw_arrow", 50 }
                    },
                    new List<string>() { "1HST1"}),
                };
                to.AddTeam("Team Gomez", spawnPoints, npcDefs, ColorRGBA.Red);

                // TEAM NEUES LAGER
                spawnPoints = new List<Vec3f, Angles>()
                {
                    { new Vec3f(-4017.539f, -289.0983f, -2818.167f), new Angles(0f, 0.02443461f, 0f) },
                    { new Vec3f(-3741.954f, -282.7443f, -2828.874f), new Angles(0f, 0.09773839f, 0f) },
                    { new Vec3f(-3501.845f, -281.3712f, -2854.833f), new Angles(0f, 0.2164207f, 0f) },
                    { new Vec3f(-4204.289f, -302.1991f, -2865.264f), new Angles(0f, -0.1675519f, 0f) },
                    { new Vec3f(-4414.396f, -308.5389f, -2741.288f), new Angles(0f, -0.4572764f, 0f) },
                    { new Vec3f(-4682.539f, -294.4325f, -2370.788f), new Angles(0f, -0.677188f, 0f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Söldner", null, new List<string, int>()
                    {
                        { "2haxt", 1 }, { "itar_söldner", 1}, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    },
                    new List<string>() { "2HST1"}),
                    new TOClassDef("Bandit", null, new List<string, int>()
                    {
                        { "1haxt", 1 }, { "itar_bandit", 1}, { "itrw_longbow", 1}, { "itrw_arrow", 50 }
                    },
                    new List<string>() { "1HST1"}),
                };
                to.AddTeam("Tetriandoch", spawnPoints, npcDefs, ColorRGBA.Blue);

                objectives.Add(to.name, to);
                #endregion

                #region TDM Mine

                to = new TODef();
                to.name = "tdm_mine";
                to.worldPath = "G1-OLDMINE.ZEN";
                to.duration = 10; // 5 min
                to.scoreToWin = 50;
                to.maxHeight = -6800;
                to.maxWorldDistance = 8635;
                to.specPos = new ValueTuple<Vec3f, Angles>(new Vec3f(1478.838f, -9251.064f, -6242.642f), new Angles(0.1151917f, -1.291543f, 0f));


                // TEAM ALTES LAGER
                spawnPoints = new List<Vec3f, Angles>()
                {
                    { new Vec3f(1647.881f, -7898.187f, -2613.269f), new Angles(0f, 0f, 0f) },
                    { new Vec3f(1842.845f, -7897.371f, -2325.282f), new Angles(0f, 1.37532f, 0f) },
                    { new Vec3f(3441.214f, -7475.371f, -2124.406f), new Angles(0f, 1.43466f, 0f) },
                    { new Vec3f(2990.72f, -7441.649f, -2360.442f), new Angles(0f, 0.8063417f, 0f) },
                    { new Vec3f(2485.762f, -7425.826f, -2425.143f), new Angles(0f, -0.2303839f, 0f) },
                    { new Vec3f(1916.954f, -7273.945f, -2269.227f), new Angles(0f, -1.528909f, 0f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Templer", null, new List<string, int>()
                    {
                        { "leichter_zweihaender", 1 }, { "itar_templer", 1}
                    },
                    new List<string>() { "2HST1"}),

                    new TOClassDef("Gardist", null, new List<string, int>()
                    {
                        { "grobes_schwert", 1 }, { "itar_garde_l", 1}
                    },
                    new List<string>() { "1HST1"}),
                };
                to.AddTeam("Team Alte Mine", spawnPoints, npcDefs, ColorRGBA.Red);

                // TEAM MINECRAWLER
                spawnPoints = new List<Vec3f, Angles>()
                {
                    { new Vec3f(4240.823f, -8092.504f, 5476.157f), new Angles(-0.1134214f, -1.923348f, 0f) },
                    { new Vec3f(4326.87f, -8130.521f, 4848.703f), new Angles(-0.1358788f, -1.483522f, 0) },
                    { new Vec3f(-527.3607f, -8292.744f, 5760.481f), new Angles(0.002349615f, -2.275903f, 0) },
                    { new Vec3f(-271.3119f, -8320.184f, 5945.153f), new Angles(0.03162932f, -2.600533f, 0) },
                    { new Vec3f(5399.01f, -8575.909f, -5303.596f), new Angles(-0.0418613f, 1.413725f, 0) },
                    { new Vec3f(5369.831f, -8570.879f, -5632.852f), new Angles(-0.0684464f, 1.253157f, 0) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Klauen-Minecrawler", "minecrawler", null, null, prot:35, dam:50),
                    new TOClassDef("Panzer-Minecrawler", "minecrawler_warrior", null, null, prot:40, dam:45),
                };
                to.AddTeam("Schachtcrew", spawnPoints, npcDefs, ColorRGBA.Orange);

                objectives.Add(to.name, to);
                #endregion

                #region TDM Pass

                to = new TODef();
                to.name = "tdm_pass";
                to.worldPath = "G2-PASS.ZEN";
                to.duration = 10; // 5 min
                to.scoreToWin = 50;
                to.maxWorldDistance = 10650;
                to.maxHeight = 950;
                to.specPos = new ValueTuple<Vec3f, Angles>(new Vec3f(3725.298f, -735.0935f, 2743.517f), new Angles(-0.06981301f, 0.9250249f, 0f));

                spawnPoints = new List<Vec3f, Angles>()
                {
                    { new Vec3f(3069.88f, 148.2805f, 9595.466f), new Angles(0f, 2.01411f, 0f) },
                    { new Vec3f(3278.937f, 196.152f, 9662.452f), new Angles(0f, 2.031563f, 0f) },
                    { new Vec3f(2932.983f, 10.72608f, 9838.522f), new Angles(0f, 2.432988f, 0f) },
                    { new Vec3f(2686.674f, -7.369984f, 9910.67f), new Angles(0f, 2.44346f, 0f) },
                    { new Vec3f(2493.167f, 168.4053f, 10023.55f), new Angles(0f, 2.455878f, 0f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Ritter", null, new List<string, int>()
                    {
                        { "2hschwert", 1 }, { "itar_ritter", 1}, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    },
                    new List<string>() { "2HST2", "XbowT1"}),
                    new TOClassDef("Miliz", null, new List<string, int>()
                    {
                        { "1hschwert", 1 }, { "itar_miliz_s", 1}, { "heavy_xbow", 1}, { "itrw_bolt", 50 }
                    },
                    new List<string>() { "1HST2", "XbowT2"}),
                };
                to.AddTeam("Paladintrupp", spawnPoints, npcDefs, ColorRGBA.Red);

                spawnPoints = new List<Vec3f, Angles>()
                {
                    { new Vec3f(-1181.993f, -437.5759f, -2589.357f), new Angles(0f, 0.5689777f, 0f) },
                    { new Vec3f(-1033.761f, -438.4826f, -2530.26f), new Angles(0f, 0.4817112f, 0f) },
                    { new Vec3f(-766.6343f, -425.4779f, -2489.346f), new Angles(0f, 0.6946411f, 0f) },
                    { new Vec3f(-628.7674f, -355.7959f, -2276.756f), new Angles(0f, 0.959931f, 0f) },
                    { new Vec3f(-558.9549f, -192.2555f, -2474.483f), new Angles(0f, 0.9305235f, 0f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Ork-Elite", "orc_elite", new List<string, int>()
                    {
                        { "orc_sword", 1 }, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    }, prot:40),
                    new TOClassDef("Ork-Krieger", "orc_warrior", new List<string, int>()
                    {
                        { "krush_pach", 1 }, { "heavy_xbow", 1}, { "itrw_bolt", 50 }
                    }, prot:35),
                };
                to.AddTeam("Grünfellwacht", spawnPoints, npcDefs, ColorRGBA.Green);

                objectives.Add(to.name, to);
                #endregion

                #region TDM Temple

                to = new TODef();
                to.name = "tdm_temple";
                to.worldPath = "ADDON-TEMPLE.ZEN";
                to.duration = 10; // 5 min
                to.scoreToWin = 50;
                to.maxDepth = -2100;
                to.maxHeight = 400;
                to.maxWorldDistance = 7400;
                to.specPos = new ValueTuple<Vec3f, Angles>(new Vec3f(2012.767f, -195.6564f, 4388.974f), new Angles(-0.153589f, 2.816961f, 0f));

                spawnPoints = new List<Vec3f, Angles>()
                {
                    { new Vec3f(2036.342f, -332.4806f, 6549.103f), new Angles(0f, 2.813471f, 0f) },
                    { new Vec3f(1911.383f, -309.5511f, 6635.414f), new Angles(0f, 2.771583f, 0f) },
                    { new Vec3f(1714.081f, -261.7822f, 6643.623f), new Angles(0f, 2.960078f, 0f) },
                    { new Vec3f(1898.435f, -343.3698f, 6514.929f), new Angles(0f, 2.712242f, 0f) },
                    { new Vec3f(2125.315f, -352.5587f, 6391.019f), new Angles(0f, 2.286381f, 0f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Grober Bandit", null, new List<string, int>()
                    {
                        { "grober_2h", 1 }, { "itar_bandit", 1}, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    },
                    new List<string>() { "2HST1"}, prot:5),
                    new TOClassDef("Leichter Bandit", null, new List<string, int>()
                    {
                        { "grobes_schwert", 1 }, { "itar_bandit_m", 1}, { "itrw_longbow", 1}, { "itrw_arrow", 50 }
                    },
                    new List<string>() { "1HST1"}),
                };
                to.AddTeam("Ravens Späher", spawnPoints, npcDefs, ColorRGBA.White);

                spawnPoints = new List<Vec3f, Angles>()
                {
                    { new Vec3f(528.394f, -361.0737f, -2002.128f), new Angles(0f, -0.02827433f, 0f) },
                    { new Vec3f(271.3965f, -360.2425f, -1860.179f), new Angles(0f, -0.3354523f, 0f) },
                    { new Vec3f(32.50935f, -351.4849f, -1953.05f), new Angles(0f, -0.1469567f, 0f) },
                    { new Vec3f(-201.5549f, -358.1647f, -1899.285f), new Angles(0f, -0.3040364f, 0f) },
                    { new Vec3f(-442.3943f, -357.4809f, -1887.496f), new Angles(0f, -0.6740463f, 0f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Schattenlord", "skeleton_lord", new List<string, int>()
                    {
                        { "2hschwert", 1 }, { "itar_pal_skel", 1 }, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    }, new List<string>() { "2HST1"}),
                    new TOClassDef("Skelett-Späher", "skeleton", new List<string, int>()
                    {
                        { "grobes_schwert", 1 }, { "itrw_longbow", 1}, { "itrw_arrow", 50 }
                    },
                    new List<string>() { "1HST1"}, prot:35),
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

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
                to.duration = 10; // 5 min
                to.scoreToWin = 50;
                to.maxDepth = -400;
                to.specPos = new ValueTuple<Vec3f, Vec3f>(new Vec3f(-2442.949f, 676.9498f, 412.3001f), new Vec3f(0.8977975f, -0.2387571f, -0.3700744f));

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
                        { "2hschwert", 1 }, { "itar_garde", 1 }, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    }),
                    new TOClassDef("Schatten", null, new List<string, int>()
                    {
                        { "1hschwert", 1 }, { "itar_schatten", 1 }, { "itrw_longbow", 1}, { "itrw_arrow", 50 }
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
                        { "2haxt", 1 }, { "itar_söldner", 1}, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    }),
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
                to.specPos = new ValueTuple<Vec3f, Vec3f>(new Vec3f(1478.838f, -9251.064f, -6242.642f), new Vec3f(0.9542055f, 0.128796f, 0.2700137f));


                // TEAM ALTES LAGER
                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(1647.881f, -7898.143f, -2613.269f), new Vec3f(-0.6195504f, 0f, 0.7849569f) },
                    { new Vec3f(1805.787f, -7897.12f, -2394.196f), new Vec3f(-0.9250993f, 0f, 0.3797255f) },
                    { new Vec3f(1588.374f, -7897.783f, -2407.296f), new Vec3f(-0.6727971f, 0f, 0.739827f) },
                    { new Vec3f(3293.175f, -7472.599f, -2125.258f), new Vec3f(-0.9746961f, 0f, 0.2235342f) },
                    { new Vec3f(3005.945f, -7445.056f, -2313.669f), new Vec3f(-0.8597034f, 0f, 0.5107936f) },
                    { new Vec3f(2663.932f, -7429.853f, -2337.578f), new Vec3f(-0.2195615f, 0f, 0.9755987f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Templer", null, new List<string, int>()
                    {
                        { "leichter_zweihaender", 1 }, { "itar_templer", 1}
                    }),

                    new TOClassDef("Gardist", null, new List<string, int>()
                    {
                        { "grobes_schwert", 1 }, { "itar_garde_l", 1}
                    },
                    new List<string>() { "2HST1"}),
                };
                to.AddTeam("Team Alte Mine", spawnPoints, npcDefs, ColorRGBA.Red);

                // TEAM MINECRAWLER
                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(4240.522f, -8093.314f, 5473.961f), new Vec3f(0.949815f, -0.09644543f, -0.2975734f) },
                    { new Vec3f(4307.686f, -8118.625f, 4750.24f), new Vec3f(0.9710852f, -0.2105983f, 0.1124366f) },
                    { new Vec3f(-467.942f, -8289.517f, 5695.204f), new Vec3f(0.7430767f, 0.01833591f, -0.668955f) },
                    { new Vec3f(-337.7988f, -8315.676f, 5922.874f), new Vec3f(0.5503281f, 0.02844109f, -0.834464f) },
                    { new Vec3f(5252.079f, -8558.768f, -5922.848f), new Vec3f(-0.6680197f, -0.1467312f, 0.729534f) },
                    { new Vec3f(5301.65f, -8578.755f, -5195.669f), new Vec3f(-0.8782461f, -0.04097912f, -0.47645f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Panzer-Minecrawler", "minecrawler_warrior", null, null, 40, 45),
                    new TOClassDef("Minecrawler", "minecrawler", null, null, 30, 55),
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
                to.specPos = new ValueTuple<Vec3f, Vec3f>(new Vec3f(3725.298f, -735.0935f, 2743.517f), new Vec3f(-0.8221546f, -0.04884968f, 0.567166f));

                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(3069.88f, 148.3073f, 9595.466f), new Vec3f(-0.8589599f, 0f, -0.512043f) },
                    { new Vec3f(2793.139f, -30.78938f, 9802.241f), new Vec3f(-0.6293203f, 0f, -0.777146f) },
                    { new Vec3f(2574.253f, -17.34783f, 9796.998f), new Vec3f(-0.6347303f, 0f, -0.7727338f) },
                    { new Vec3f(2677.734f, 2.666633f, 9594.524f), new Vec3f(-0.8625137f, 0f, -0.5060338f) },
                    { new Vec3f(2777.245f, 87.26538f, 9485.686f), new Vec3f(-0.9149597f, 0f, -0.4035454f) },
                    { new Vec3f(2927.429f, 166.2642f, 9413.211f), new Vec3f(-0.9177547f, 0f, -0.3971479f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Ritter", null, new List<string, int>()
                    {
                        { "2hschwert", 1 }, { "itar_ritter", 1}, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    },
                    new List<string>() { "2HST2"}),
                    new TOClassDef("Miliz", null, new List<string, int>()
                    {
                        { "1hschwert", 1 }, { "itar_miliz_s", 1}, { "heavy_xbow", 1}, { "itrw_bolt", 50 }
                    },
                    new List<string>() { "1HST2"}),
                };
                to.AddTeam("Paladintrupp", spawnPoints, npcDefs, ColorRGBA.Red);

                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(-1181.993f, -437.1606f, -2589.357f), new Vec3f(-0.4277521f, 0f, 0.9038962f) },
                    { new Vec3f(-914.4005f, -438.5191f, -2535.535f), new Vec3f(-0.6987274f, 0f, 0.7153881f) },
                    { new Vec3f(-731.5646f, -383.0498f, -2338.793f), new Vec3f(-0.7674448f, 0f, 0.6411151f) },
                    { new Vec3f(-989.1671f, -439.3947f, -2398.904f), new Vec3f(-0.6444571f, 0f, 0.7646406f) },
                    { new Vec3f(-1247.077f, -438.1329f, -2482.61f), new Vec3f(-0.4836642f, 0f, 0.8752537f) },
                    { new Vec3f(-579.5058f, -204.6773f, -2462.555f), new Vec3f(-0.8302556f, 0f, 0.5573828f) },
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
                    }, prot:30),
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
                to.specPos = new ValueTuple<Vec3f, Vec3f>(new Vec3f(2012.767f, -195.6564f, 4388.974f), new Vec3f(-0.3345688f, -0.1564352f, -0.9292989f));

                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(1816.431f, -305.0804f, 6607.576f), new Vec3f(-0.2385892f, 0f, -0.9711206f) },
                    { new Vec3f(2036.342f, -332.478f, 6549.103f), new Vec3f(-0.3387918f, 0f, -0.9408614f) },
                    { new Vec3f(2007.473f, -287.4689f, 6722.368f), new Vec3f(-0.2611216f, 0f, -0.9653059f) },
                    { new Vec3f(1922.155f, -365.8554f, 6435.443f), new Vec3f(-0.297985f, 0f, -0.9545705f) },
                    { new Vec3f(1708.69f, -320.4089f, 6512.574f), new Vec3f(-0.1691195f, 0f, -0.9855956f) },
                    { new Vec3f(2119.849f, -360.1476f, 6362.776f), new Vec3f(-0.4310881f, 0f, -0.9023099f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Grober Bandit", null, new List<string, int>()
                    {
                        { "2hschwert", 1 }, { "itar_bandit_m", 1}, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    },
                    new List<string>() { "2HST1"}),
                    new TOClassDef("Nicht-grober Bandit", null, new List<string, int>()
                    {
                        { "1hschwert", 1 }, { "itar_bandit", 1}, { "itrw_longbow", 1}, { "itrw_arrow", 50 }
                    },
                    new List<string>() { "1HST1"}),
                };
                to.AddTeam("Ravens Späher", spawnPoints, npcDefs, ColorRGBA.White);

                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(528.394f, -361.0391f, -2002.128f), new Vec3f(-0.328812f, 0f, 0.9443954f) },
                    { new Vec3f(264.5747f, -359.7288f, -1962.031f), new Vec3f(0.06981393f, 0f, 0.9975601f) },
                    { new Vec3f(-3.248788f, -358.9183f, -1906.212f), new Vec3f(0.1564911f, 0f, 0.9876794f) },
                    { new Vec3f(-261.0937f, -358.1917f, -1858.025f), new Vec3f(0.3939945f, 0f, 0.9191129f) },
                    { new Vec3f(-536.3656f, -359.1673f, -1847.327f), new Vec3f(0.6997041f, 0f, 0.7144328f) },
                    { new Vec3f(-821.115f, -364.3337f, -1905.385f), new Vec3f(0.7727699f, 0f, 0.6346863f) },
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Schattenlord", "skeleton", new List<string, int>()
                    {
                        { "grober_2h", 1 }, { "itar_pal_skel", 1 }, { "light_xbow", 1}, { "itrw_bolt", 20 }
                    }, new List<string>() { "2HST1"}),
                    new TOClassDef("Skelett-Krieger", "skeleton", new List<string, int>()
                    {
                        { "grobes_schwert", 1 }, { "itrw_longbow", 1}, { "itrw_arrow", 50 }
                    },
                    new List<string>() { "1HST1"}, prot:30),
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

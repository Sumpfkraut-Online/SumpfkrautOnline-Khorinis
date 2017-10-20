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
        string name;
        public string Name { get { return this.name; } }

        int duration;
        /// <summary> In Minutes </summary>
        public int Duration { get { return duration; } }

        int scoreToWin;
        public int ScoreToWin { get { return scoreToWin; } }

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
                to.duration = 2; // 5 min
                to.scoreToWin = 100;

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
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}

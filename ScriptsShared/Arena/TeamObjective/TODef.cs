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
                to.duration = 1; // 5 min
                to.scoreToWin = 100;

                // TEAM ALTES LAGER
                var spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(0, 1000, 0), new Vec3f(0,0,1) }
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
                    { new Vec3f(0, 1000, 0), new Vec3f(0,0,1) }
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

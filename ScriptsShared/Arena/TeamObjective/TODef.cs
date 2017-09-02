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
        public IEnumerable<TOTeamDef> Teams { get { return teams; } }

        void AddTeam(string name, List<Vec3f, Vec3f> spawnPoints, List<TOClassDef> classDefs)
        {
            teams.Add(new TOTeamDef(name, spawnPoints, classDefs));
        }

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
                    }),
                    new TOClassDef("Schatten", null, new List<string, int>()
                    {
                    }),
                };
                to.AddTeam("Team Gomez", spawnPoints, npcDefs);

                // TEAM NEUES LAGER
                spawnPoints = new List<Vec3f, Vec3f>()
                {
                    { new Vec3f(0, 1000, 0), new Vec3f(0,0,1) }
                };
                npcDefs = new List<TOClassDef>()
                {
                    new TOClassDef("Söldner", null, new List<string, int>()
                    {
                    }),
                    new TOClassDef("Bandit", null, new List<string, int>()
                    {
                    }),
                };
                to.AddTeam("Tetriandoch", spawnPoints, npcDefs);

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

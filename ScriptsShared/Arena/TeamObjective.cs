using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;
using GUC.Utilities;

namespace GUC.Scripts.Arena
{
    class TODef
    {
        public struct ClassDef
        {
            public string Name;
            public string NPCDef;
            public List<string, int> ItemDefs;
        }

        public struct TeamDef
        {
            public string Name;
            public List<Vec3f, Vec3f> SpawnPoints;
            public List<ClassDef> Classes;

            public void AddClass(ClassDef def)
            {
                if (Classes == null)
                    Classes = new List<ClassDef>();

                Classes.Add(def);
            }
        }

        TeamDef team1, team2;
        public TeamDef Team1 { get { return team1; } }
        public TeamDef Team2 { get { return team2; } }

        int duration;
        /// <summary> In Minutes </summary>
        public int Duration { get { return duration; } }

        int scoreToWin;
        public int ScoreToWin { get { return scoreToWin; } }

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

        static TODef()
        {
            #region TDM Burg

            var to = new TODef();
            to.duration = 5; // 5 min
            to.scoreToWin = 100;

            // TEAM ALTES LAGER
            to.team1.Name = "Team Gomez";
            to.team1.SpawnPoints = new List<Vec3f, Vec3f>()
            {
                { new Vec3f(0, 1000, 0), new Vec3f(0,0,1) }
            };

            // GARDIST
            var npc = new ClassDef();
            npc.Name = "Gardist";
            npc.ItemDefs = new List<string, int>()
            {

            };
            to.Team1.AddClass(npc);

            // SCHATTEN
            npc = new ClassDef();
            npc.Name = "Schatten";
            npc.ItemDefs = new List<string, int>()
            {

            };
            to.Team1.AddClass(npc);


            // TEAM NEUES LAGER
            to.team2.Name = "Tetriandoch";
            to.team2.SpawnPoints = new List<Vec3f, Vec3f>()
            {
                { new Vec3f(0, 1000, 0), new Vec3f(0, 0, 1) }
            };

            // SÖLDNER
            npc = new ClassDef();
            npc.Name = "Söldner";
            npc.ItemDefs = new List<string, int>()
            {

            };
            to.Team1.AddClass(npc);

            // BANDIT
            npc = new ClassDef();
            npc.Name = "Bandit";
            npc.ItemDefs = new List<string, int>()
            {

            };
            to.Team1.AddClass(npc);

            objectives.Add("tdm_burg", to);

            #endregion
        }
    }
}

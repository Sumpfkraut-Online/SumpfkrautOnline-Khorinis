using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;
using GUC.Utilities;

namespace GUC.Scripts.Arena
{
    class TOTeamDef
    {
        string name;
        public string Name { get { return name; } }

        List<Vec3f, Vec3f> spawnPoints;
        public IEnumerable<Vec3f, Vec3f> SpawnPoints { get { return spawnPoints; } }

        List<TOClassDef> classDefs;
        public IEnumerable<TOClassDef> ClassDefs { get { return classDefs; } }

        ColorRGBA color;
        public ColorRGBA Color { get { return color; } }

        public TOTeamDef(string name, List<Vec3f, Vec3f> spawnPoints, List<TOClassDef> classDefs) 
            : this(name, spawnPoints, classDefs, ColorRGBA.White)
        {
        }
        
        public TOTeamDef(string name, List<Vec3f, Vec3f> spawnPoints, List<TOClassDef> classDefs, ColorRGBA teamColor)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (spawnPoints == null || spawnPoints.Count == 0)
                throw new ArgumentNullException("spawnPoints");
            if (classDefs == null || classDefs.Count == 0)
                throw new ArgumentNullException("classDefs");

            this.name = name;
            this.spawnPoints = spawnPoints;
            this.classDefs = classDefs;
            this.color = teamColor;
        }
    }
}

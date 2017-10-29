using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Utilities;

namespace GUC.Scripts.Arena
{
    class TOClassDef
    {
        string name;
        public string Name { get { return this.name; } }

        string npcDef;
        public string NPCDef { get { return this.npcDef; } }

        List<string, int> itemDefs;
        public IEnumerable<string, int> ItemDefs { get { return itemDefs; } }

        List<string> overlays;
        public IEnumerable<string> Overlays { get { return overlays; } }

        int protection;
        public int Protection { get { return protection; } }

        int damage;
        public int Damage { get { return this.damage; } }

        public TOClassDef(string name, string npcDef, List<string, int> itemDefs = null, List<string> overlays = null, int prot = 0, int dam = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            this.name = name;
            this.npcDef = string.IsNullOrWhiteSpace(npcDef) ? null : npcDef;
            this.itemDefs = itemDefs ?? new List<string, int>();
            this.overlays = overlays ?? new List<string>();
            this.protection = prot;
            this.damage = dam;
        }
    }
}

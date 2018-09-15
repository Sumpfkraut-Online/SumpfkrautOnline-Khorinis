using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena.GameModes
{
    class NPCClass
    {
#if D_CLIENT
        public static NPCClass Hero;
#endif

        public string Name;
        public string Definition;

        public struct InvItem
        {
            public string DefName;
            public int Amount;

            public InvItem(string defName, int amount = 1)
            {
                this.DefName = defName;
                this.Amount = amount;
            }
        }

        public InvItem[] ItemDefs;
        public string[] Overlays;

        public int Protection;
        public int Damage;
        public int HP = 100;

        public Allegiance Guild;
    }
}

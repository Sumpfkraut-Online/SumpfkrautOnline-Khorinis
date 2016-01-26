using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public partial class VobCollection : VobObjCollection<uint, Vob>
    {
        public readonly VobObjDictionary<uint, NPC> Players = new VobObjDictionary<uint, NPC>();

        internal override void Add(Vob vob)
        {
            base.Add(vob);
            if (vob is NPC)
            {
                NPC npc = (NPC)vob;
                if (npc.IsPlayer)
                    Players.Add(npc);
            }
        }

        internal override void Remove(Vob vob)
        {
            base.Remove(vob);
            if (vob is NPC)
            {
                NPC npc = (NPC)vob;
                if (npc.IsPlayer)
                    Players.Remove(npc);
            }
        }
    }
}

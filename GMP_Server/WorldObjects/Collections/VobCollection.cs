using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Server.WorldObjects.Collections
{
    public class VobCollection : VobObjCollection<uint, Vob>
    {
        public readonly VobObjDictionary<uint, NPC> Players = new VobObjDictionary<uint, NPC>();

        internal VobCollection()
        {
            for (int i = 0; i < (int)VobType.Maximum; i++)
            {
                vobDicts[i] = new VobDictionary();
            }
        }

        internal override void Add(Vob vob)
        {
            base.Add(vob);
            if (vob is NPC)
            {
                NPC npc = (NPC)vob;
                if (npc.isPlayer)
                    Players.Add(npc);
            }
        }

        internal override void Remove(Vob vob)
        {
            base.Remove(vob);
            if (vob is NPC)
            {
                NPC npc = (NPC)vob;
                if (npc.isPlayer)
                    Players.Remove(npc);
            }
        }

        new public VobDictionary GetDict(VobType type)
        {
            return (VobDictionary)base.GetDict(type);
        }
    }
}

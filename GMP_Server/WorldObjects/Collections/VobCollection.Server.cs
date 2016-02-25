using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public partial class VobCollection
    {
        Dictionary<int, NPC> players = new Dictionary<int, NPC>();

        static List<int> freeIDs = new List<int>();
        static int idCount = 0;
        partial void CheckID(BaseVob vob)
        {
            if (vob.ID < 0 || vob.ID > MAX_VOBS) //search free ID
            {
                int id;

                while (freeIDs.Count > 0)
                {
                    id = freeIDs[0];
                    freeIDs.RemoveAt(0);
                    if (!vobs.ContainsKey(id)) // because ServerScripts can set IDs manually
                    {
                        vob.ID = id;
                        return;
                    }
                }

                while (true)
                    if (idCount >= MAX_VOBS)
                    {
                        throw new Exception("VobCollection reached maximum! " + MAX_VOBS);
                    }
                    else
                    {
                        id = idCount++;
                        if (!vobs.ContainsKey(id))
                        {
                            vob.ID = id;
                            return;
                        }
                    }
            }
        }

        partial void pAdd(BaseVob vob)
        {
            if (vob is NPC && ((NPC)vob).IsPlayer)
            {
                players.Add(vob.ID, (NPC)vob);
            }
        }

        partial void pRemove(BaseVob vob)
        {
            if (vob is NPC && ((NPC)vob).IsPlayer)
            {
                players.Remove(vob.ID);
            }
        }

        public void ForEachPlayer(Action<NPC> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach (NPC npc in players.Values)
                action(npc);
        }
    }
}

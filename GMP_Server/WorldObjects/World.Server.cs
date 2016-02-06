using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using GUC.Server.Network.Messages;
using GUC.Types;
using GUC.WorldObjects.Collections;
using GUC.Log;

namespace GUC.WorldObjects
{
    public partial class World
    {
        internal Dictionary<int, Dictionary<int, NetCell>> netCells = new Dictionary<int, Dictionary<int, NetCell>>();
        internal Dictionary<int, Dictionary<int, NPCCell>> npcCells = new Dictionary<int, Dictionary<int, NPCCell>>();

        partial void pSpawnVob(BaseVob vob)
        {
            throw new NotImplementedException();
        }

        partial void pDespawnVob(BaseVob vob)
        {
            throw new NotImplementedException();
        }
    }
}

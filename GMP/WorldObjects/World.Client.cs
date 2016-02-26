using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects
{
    public partial class World
    {
        public static World Current { get; internal set; }

        partial void pSpawnVob(BaseVob vob)
        {
            if (vob.ID == Client.Network.GameClient.HeroID)
            {
                Client.Network.GameClient.UpdateHeroControl();
            }
        }
    }
}

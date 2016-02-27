using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class World
    {
        public static World Current { get; internal set; }

        partial void pSpawnVob(BaseVob vob)
        {
            vob.gvob = vob.Instance.CreateVob();

            if (vob.ID == GameClient.Client.CharacterID)
            {
                GameClient.Client.UpdateHeroControl();
            }
        }
    }
}

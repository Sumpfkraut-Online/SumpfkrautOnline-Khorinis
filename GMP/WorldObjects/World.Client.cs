using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using RakNet;
using Gothic;

namespace GUC.WorldObjects
{
    public partial class World
    {
        public static World Current;

        partial void pAddVob(BaseVob vob)
        {
            vob.gvob = vob.Instance.CreateVob();

            if (vob.ID == GameClient.Client.CharacterID)
            {
                GameClient.Client.UpdateHeroControl();
            }

            oCGame.GetWorld().AddVob(vob.gVob);
        }

        partial void pDespawnVob(BaseVob vob)
        {
            oCGame.GetWorld().RemoveVob(vob.gVob);
        }

        #region Network Messages

        internal void SendConfirmation()
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.LoadWorldMessage);
            GameClient.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using RakNet;
using Gothic;
using Gothic.Objects;

namespace GUC.WorldObjects
{
    public partial class World
    {
        public static World Current;

        partial void pAddVob(BaseVob vob)
        {
            if (vob.ID == GameClient.Client.CharacterID && !Vobs.vobAddr.ContainsKey(oCNpc.GetPlayer().Address))
            {
                oCNpc.GetPlayer().Disable();
                oCGame.GetWorld().RemoveVob(oCNpc.GetPlayer());
                vob.gvob = vob.Instance.CreateVob(oCNpc.GetPlayer());
            }
            else
            {
                vob.gvob = vob.Instance.CreateVob();
            }

            oCGame.GetWorld().AddVob(vob.gVob);

            vob.SetPosition(vob.pos);
            vob.SetDirection(vob.dir);

            if (vob.ID == GameClient.Client.CharacterID)
            {
                GameClient.Client.UpdateHeroControl(vob);
            }
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

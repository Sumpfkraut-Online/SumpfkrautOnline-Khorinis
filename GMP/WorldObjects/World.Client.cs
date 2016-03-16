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

        Dictionary<int, BaseVob> vobAddr = new Dictionary<int, BaseVob>();

        public bool TryGetVobByAddress(int address, out BaseVob vob)
        {
            return vobAddr.TryGetValue(address, out vob);
        }

        public bool TryGetVobByAddress<T>(int address, out T vob) where T : BaseVob
        {
            BaseVob v;
            if (vobAddr.TryGetValue(address, out v))
            {
                if (v is T)
                {
                    vob = (T)v;
                    return true;
                }
            }
            vob = null;
            return false;
        }

        partial void pAddVob(BaseVob vob)
        {
            if (vob.ID == GameClient.Client.CharacterID && !vobAddr.ContainsKey(oCNpc.GetPlayer().Address))
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
            vobAddr.Add(vob.gvob.Address, vob);

            vob.SetPosition(vob.GetPosition());
            vob.SetDirection(vob.GetDirection());

            if (vob.ID == GameClient.Client.CharacterID)
            {
                GameClient.Client.UpdateHeroControl((NPC)vob);
            }
        }

        partial void pRemoveVob(BaseVob vob)
        {
            oCGame.GetWorld().RemoveVob(vob.gVob);
            vobAddr.Remove(vob.gvob.Address);
        }

        #region Network Messages

        internal void SendConfirmation()
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.LoadWorldMessage);
            GameClient.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.WorldObjects.Mobs;

namespace GUC.Server.Network.Messages.NpcCommands
{
    class NPCUpdateMessage : IMessage
    {

        public void Read(BitStream stream, Client client)
        {
            int playerID = 0, cF = 0;
            stream.Read(out playerID);
            stream.Read(out cF);

            NPC proto = (NPC)sWorld.VobDict[playerID];

            NPCChangedFlags changeFlags = (NPCChangedFlags)cF;
            
            //Equipment:
            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_ARMOR))
            {
              proto.Armor = UpdateItem(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_NW))
            {
              proto.Weapon = UpdateItem(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_RW))
            {
              proto.RangeWeapon = UpdateItem(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.WeaponMode))
            {
                int weaponMode = 0;
                stream.Read(out weaponMode);
                proto.WeaponMode = weaponMode;
            }

            for (int i = 0; i < 9; i++)
            {
                if (changeFlags.HasFlag((NPCChangedFlags)((int)NPCChangedFlags.SLOT1 << i)))
                {
                    int slotItemID = 0;
                    stream.Read(out slotItemID);

                    if (slotItemID > 0)
                    {
                        Item slotItem  = (Item)sWorld.VobDict[slotItemID];
                        //Item oldSlotItem = proto.Slots[i];

                        proto.Slots[i] = slotItem;
                    }
                    else
                        proto.Slots[i] = null;
                }
            }

            if (changeFlags.HasFlag(NPCChangedFlags.VOBFOCUS))
            {
              proto.FocusVob=UpdateVob(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ENEMYFOCUS))
            {
              proto.Enemy = (NPC)UpdateVob(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.MOBINTERACT))
            {
              proto.MobInter = (MobInter)UpdateVob(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISDEAD))
            {
              proto.IsDead = UpdateState(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISUNCONSCIOUS))
            {
              proto.IsUnconcious = UpdateState(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISSWIMMING))
            {
              proto.IsSwimming = UpdateState(stream);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.PORTALROOM))
            {
                String portalRoom = "";
                stream.Read(out portalRoom);
                proto.PortalRoom = portalRoom;
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ACTIVE_SPELL))
            {
                int vobID = 0;
                stream.Read(out vobID);

                if (vobID == 0)
                    proto.ActiveSpell = null;
                else
                    proto.ActiveSpell = (Item)sWorld.VobDict[vobID];
            }

            //Sending back:
            stream.ResetReadPointer();

            using (RakNetGUID guid = new RakNetGUID(client.guid))
            Program.server.ServerInterface.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, true);

        }

        private static bool UpdateState(RakNet.BitStream stream)
        {
          bool isX = false;
          stream.Read(out isX);
          return isX;
        }

        private static Vob UpdateVob(RakNet.BitStream stream)
        {
          int vobID = 0;
          stream.Read(out vobID);

          if (vobID == 0)
            return null;
          else
            return sWorld.VobDict[vobID];
        }

      private Item UpdateItem(RakNet.BitStream stream)
      {        
        int itemID = 0;
        stream.Read(out itemID);
        if (itemID > 0)
        {         
          return (Item)sWorld.VobDict[itemID];
        }
        else
          return null;
      }
    }
}

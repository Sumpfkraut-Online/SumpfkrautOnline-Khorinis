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

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int playerID = 0, cF = 0;
            stream.Read(out playerID);
            stream.Read(out cF);

            NPCProto proto = (NPCProto)sWorld.VobDict[playerID];

            NPCChangedFlags changeFlags = (NPCChangedFlags)cF;
            
            //Equipment:
            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_ARMOR))
            {
                Item armor = null;
                int armorID = 0;
                stream.Read(out armorID);
                if (armorID > 0)
                {
                    armor = (Item)sWorld.VobDict[armorID];
                    Item oldArmor = proto.Armor;

                    proto.Armor = armor;
                }
                else
                    proto.Armor = null;
            }

            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_NW))
            {
                Item weapon = null;
                int weaponID = 0;
                stream.Read(out weaponID);
                if (weaponID > 0)
                {
                    weapon = (Item)sWorld.VobDict[weaponID];
                    Item oldWeapon = proto.Weapon;

                    proto.Weapon = weapon;
                }
                else
                    proto.Weapon = null;
            }

            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_RW))
            {
                Item weapon = null;
                int weaponID = 0;
                stream.Read(out weaponID);
                if (weaponID > 0)
                {
                    weapon = (Item)sWorld.VobDict[weaponID];
                    Item oldWeapon = proto.RangeWeapon;

                    proto.RangeWeapon = weapon;
                }
                else
                    proto.RangeWeapon = null;
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
                        Item oldSlotItem = proto.Slots[i];

                        proto.Slots[i] = slotItem;
                    }
                    else
                        proto.Slots[i] = null;
                }
            }

            if (changeFlags.HasFlag(NPCChangedFlags.VOBFOCUS))
            {
                int vobID = 0;
                stream.Read(out vobID);

                if (vobID == 0)
                    proto.FocusVob = null;
                else
                    proto.FocusVob = sWorld.VobDict[vobID];
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ENEMYFOCUS))
            {
                int vobID = 0;
                stream.Read(out vobID);

                if (vobID == 0)
                    proto.Enemy = null;
                else
                    proto.Enemy = (NPCProto)sWorld.VobDict[vobID];
            }

            if (changeFlags.HasFlag(NPCChangedFlags.MOBINTERACT))
            {
                int vobID = 0;
                stream.Read(out vobID);

                if (vobID == 0)
                    proto.MobInter = null;
                else
                    proto.MobInter = (MobInter)sWorld.VobDict[vobID];
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISDEAD))
            {
                bool isdead = false;
                stream.Read(out isdead);
                proto.IsDead = isdead;
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISUNCONSCIOUS))
            {
                bool isuncon = false;
                stream.Read(out isuncon);
                proto.IsUnconcious = isuncon;
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISSWIMMING))
            {
                bool isswimming = false;
                stream.Read(out isswimming);
                proto.IsSwimming = isswimming;
            }

            if (changeFlags.HasFlag(NPCChangedFlags.PORTALROOM))
            {
                String portalRoom = "";
                stream.Read(out portalRoom);
                proto.PortalRoom = portalRoom;
            }

            //Sending back:
            stream.ResetReadPointer();

            Program.server.server.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, true);

        }
    }
}

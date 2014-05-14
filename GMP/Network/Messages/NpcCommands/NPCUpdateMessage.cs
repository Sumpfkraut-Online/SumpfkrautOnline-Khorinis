using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.Enumeration;
using Gothic.zClasses;
using WinApi;
using GUC.WorldObjects;
using RakNet;
using GUC.WorldObjects.Mobs;
using Gothic.zTypes;

namespace GUC.Network.Messages.NpcCommands
{
    class NPCUpdateMessage : IMessage
    {
        public static void Write(NPCProto proto)
        {
            if (proto.Address == 0)
                return;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, proto.Address);

            if (npc.AniCtrl.Address == 0)
                return;

            NPCChangedFlags changeFlags = 0;

            oCItem iArmor = npc.GetEquippedArmor();
            Item armor = null;

            oCItem iWeapon = npc.GetEquippedMeleeWeapon();
            Item weapon = null;

            oCItem iRangeWeapon = npc.GetEquippedRangedWeapon();
            Item rangeWeapon = null;

            Vob focusVobID = null;
            NPCProto enemyVobID = null;
            MobInter MobInterID = null;
            int mobInterAddress = npc.GetInteractMob().Address;

            if(npc.FocusVob.Address != 0 && sWorld.SpawnedVobDict.ContainsKey(npc.FocusVob.Address))
                focusVobID = sWorld.SpawnedVobDict[npc.FocusVob.Address];
            if (npc.Enemy.Address != 0 && sWorld.SpawnedVobDict.ContainsKey(npc.Enemy.Address))
                enemyVobID = (NPCProto)sWorld.SpawnedVobDict[npc.Enemy.Address];
            if (mobInterAddress != 0 && sWorld.SpawnedVobDict.ContainsKey(mobInterAddress))
                MobInterID = (MobInter)sWorld.SpawnedVobDict[mobInterAddress];


            Item selectedSpell = null;
            if (npc.MagBook != null && npc.MagBook.Address != 0)
            {
                oCItem spellItem = npc.MagBook.GetSpellItem(npc.MagBook.GetSelectedSpellNr());
                if (spellItem != null && spellItem.Address != 0)
                {
                    Vob spellVob = null;
                    sWorld.SpawnedVobDict.TryGetValue(spellItem.Address, out spellVob);
                    selectedSpell = (Item)spellVob;
                }
            }

            if (iArmor.Address != 0)
                armor = (Item)sWorld.SpawnedVobDict[iArmor.Address];
            if (iWeapon.Address != 0)
                weapon = (Item)sWorld.SpawnedVobDict[iWeapon.Address];
            if (iRangeWeapon.Address != 0)
                rangeWeapon = (Item)sWorld.SpawnedVobDict[iRangeWeapon.Address];

            int weaponMode = npc.WeaponMode;

            Item[] SlotItems = new Item[9];
            for (int i = 0; i < SlotItems.Length; i++)
            {
                oCItem slItem = npc.GetSlotItem(oCNpc.getSlotString(process, i));
                if (slItem.Address != 0 && sWorld.SpawnedVobDict.ContainsKey(slItem.Address))
                    SlotItems[i] = (Item)sWorld.SpawnedVobDict[slItem.Address];
            }


            bool isdead = false;
            if (npc.IsDead() > 0)
                isdead = true;
            bool isUnconscious = false;
            if (npc.IsUnconscious() > 0)
                isUnconscious = true;
            bool isSwimming = false;
            if (npc.AniCtrl.GetWalkModeString() == null ||npc.AniCtrl.GetWalkModeString().Trim().ToLower().Length == 0)
                isSwimming = true;

            zString str = npc.GetSectorNameVobIsIn();
            String portalRoom = "";
            if (str.Address != 0)
                portalRoom = str.Value.Trim().ToLower();




            //Setting ChangeFlags:
            if (proto.Armor != armor)
                changeFlags |= NPCChangedFlags.EQUIP_ARMOR;
            if (proto.Weapon != weapon)
                changeFlags |= NPCChangedFlags.EQUIP_NW;
            if (proto.RangeWeapon != rangeWeapon)
                changeFlags |= NPCChangedFlags.EQUIP_RW;
            if(proto.WeaponMode != weaponMode)
                changeFlags |= NPCChangedFlags.WeaponMode;
            for (int i = 0; i < SlotItems.Length; i++)
            {
                if (proto.Slots[i] != SlotItems[i])
                    changeFlags |= (NPCChangedFlags)((int)NPCChangedFlags.SLOT1 << i);
            }
            if (proto.FocusVob != focusVobID)
                changeFlags |= NPCChangedFlags.VOBFOCUS;
            if (proto.Enemy != enemyVobID)
                changeFlags |= NPCChangedFlags.ENEMYFOCUS;
            if (proto.MobInter != MobInterID)
                changeFlags |= NPCChangedFlags.MOBINTERACT;


            if (proto.IsDead != isdead)
                changeFlags |= NPCChangedFlags.ISDEAD;
            if (proto.IsUnconcious!= isUnconscious)
                changeFlags |= NPCChangedFlags.ISUNCONSCIOUS;
            if (proto.IsSwimming != isSwimming)
                changeFlags |= NPCChangedFlags.ISSWIMMING;

            if(!proto.PortalRoom.Equals(portalRoom))
                changeFlags |= NPCChangedFlags.PORTALROOM;
            if (selectedSpell != proto.ActiveSpell)
                changeFlags |= NPCChangedFlags.ACTIVE_SPELL;

            if (changeFlags == 0)
                return;
            //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "ChangeFlags: " + changeFlags, 0, "Client.cs", 0);
            //Writing Data:
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCUpdateMessage);
            stream.Write(proto.ID);
            stream.Write((int)changeFlags);

            //Equipment:
            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_ARMOR))
            {
                proto.Armor = armor;
                if (proto.Armor == null)
                    stream.Write(0);
                else
                    stream.Write(proto.Armor.ID);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_NW))
            {
                proto.Weapon = weapon;
                if (proto.Weapon == null)
                    stream.Write(0);
                else
                    stream.Write(proto.Weapon.ID);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.EQUIP_RW))
            {
                proto.RangeWeapon = rangeWeapon;
                if (proto.RangeWeapon == null)
                    stream.Write(0);
                else
                    stream.Write(proto.RangeWeapon.ID);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.WeaponMode))
            {
                proto.WeaponMode = weaponMode;
                stream.Write(weaponMode);
            }

            for (int i = 0; i < SlotItems.Length; i++)
            {
                if (changeFlags.HasFlag((NPCChangedFlags)((int)NPCChangedFlags.SLOT1 << i)))
                {
                    proto.Slots[i] = SlotItems[i];
                    if (SlotItems[i] == null)
                        stream.Write(0);
                    else
                        stream.Write(SlotItems[i].ID);
                }
            }

            if (changeFlags.HasFlag(NPCChangedFlags.VOBFOCUS))
            {
                proto.FocusVob = focusVobID;
                if (focusVobID == null)
                    stream.Write(0);
                else
                    stream.Write(focusVobID.ID);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ENEMYFOCUS))
            {
                proto.Enemy = enemyVobID;
                if (enemyVobID == null)
                    stream.Write(0);
                else
                    stream.Write(enemyVobID.ID);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.MOBINTERACT))
            {
                proto.MobInter = MobInterID;
                if (MobInterID == null)
                    stream.Write(0);
                else
                    stream.Write(MobInterID.ID);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISDEAD))
            {
                proto.IsDead = isdead;
                stream.Write(proto.IsDead);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISUNCONSCIOUS))
            {
                proto.IsUnconcious = isUnconscious;
                stream.Write(proto.IsUnconcious);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ISSWIMMING))
            {
                proto.IsSwimming = isSwimming;
                stream.Write(proto.IsSwimming);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.PORTALROOM))
            {
                proto.PortalRoom = portalRoom;
                stream.Write(proto.PortalRoom);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.ACTIVE_SPELL))
            {
                proto.ActiveSpell = selectedSpell;
                if (proto.ActiveSpell == null)
                    stream.Write(0);
                else
                    stream.Write(proto.ActiveSpell.ID);
            }
            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Getted ChangeFlags: " + changeFlags, 0, "Client.cs", 0);
            Program.client.client.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void Read(BitStream stream, Packet packet, Client client)
        {
            int playerID = 0, cF = 0;
            stream.Read(out playerID);
            stream.Read(out cF);

            NPCProto proto = (NPCProto)sWorld.VobDict[playerID];

            NPCChangedFlags changeFlags = (NPCChangedFlags)cF;
            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Getted ChangeFlags: " + changeFlags, 0, "Client.cs", 0);
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
                    proto.setArmor(armor);
                }
                else
                    proto.setArmor(null);
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
                    proto.setWeapon(weapon);
                }
                else
                    proto.setWeapon( null );
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
                    proto.setRangeWeapon(weapon);
                }
                else
                    proto.setRangeWeapon(null);
            }

            if (changeFlags.HasFlag(NPCChangedFlags.WeaponMode))
            {
                int weaponMode = 0;
                stream.Read(out weaponMode);

                proto.setWeaponMode(weaponMode);
            }

            for (int i = 0; i < 9; i++)
            {
                if (changeFlags.HasFlag((NPCChangedFlags)((int)NPCChangedFlags.SLOT1 << i)))
                {
                    int slotItemID = 0;
                    stream.Read(out slotItemID);

                    if (slotItemID > 0)
                    {
                        Item slotItem = (Item)sWorld.VobDict[slotItemID];
                        Item oldSlotItem = proto.Slots[i];

                        proto.Slots[i] = slotItem;
                    }
                    else
                        proto.Slots[i] = null;
                    proto.setSlotItem(i, proto.Slots[i]);
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

            if (changeFlags.HasFlag(NPCChangedFlags.ACTIVE_SPELL))
            {
                int vobID = 0;
                stream.Read(out vobID);

                if (vobID == 0)
                    proto.ActiveSpell = null;
                else
                    proto.ActiveSpell = (Item)sWorld.VobDict[vobID];
            }




        }
    }
}

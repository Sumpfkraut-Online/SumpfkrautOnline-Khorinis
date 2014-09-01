using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Mobs;

namespace GUC.Hooks
{
    public class hItemContainer
    {
        public static Int32 oCItemContainer_Remove_2(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                Process process = Process.ThisProcess();

                oCNpc player = oCNpc.Player(process);
                oCNpc stealNPC = oCNpc.StealNPC(process);
                oCItemContainer oIC = new oCItemContainer(process, process.ReadInt(address));
                oCItem item = new oCItem(process, process.ReadInt(address + 4));
                int amount = process.ReadInt(address + 8);

                oCMobContainer mC = new oCMobContainer(process, player.GetInteractMob().Address);
                if (oIC.Address == mC.ItemContainer.Address && (item.Amount == amount || item.Amount - amount <= 0))
                {
                    mC.Remove(item);
                }
                else if ((oIC.Address == process.ReadInt(0x00AB27E0) || oIC.Address == process.ReadInt(0x00AB27DC) ) && (item.Amount == amount || item.Amount - amount <= 0))
                {
                    oIC.Remove(item);
                    
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Item stolen from npc!", 0, "hItemContainer.cs", 0);
                }
                else
                {
                    item.Amount -= amount;
                }


                int containerID = 0;
                if (oIC.Address == mC.ItemContainer.Address)
                {
                    if (!sWorld.SpawnedVobDict.ContainsKey(mC.Address))
                        return 0;
                    Vob mobContainerVob = sWorld.SpawnedVobDict[mC.Address];
                    if (!(mobContainerVob is MobContainer))
                        return 0;
                    MobContainer mobContainer = (MobContainer)mobContainerVob;
                    containerID = mobContainer.ID;
                }
                else if (oIC.Address == process.ReadInt(0x00AB27E0) || oIC.Address == process.ReadInt(0x00AB27DC))
                {
                    if (!sWorld.SpawnedVobDict.ContainsKey(stealNPC.Address))
                        return 0;
                    Vob sVob = sWorld.SpawnedVobDict[stealNPC.Address];
                    if (!(sVob is NPCProto))
                        return 0;
                    NPCProto npc = (NPCProto)sVob;
                    containerID = npc.ID;
                }


                if (!sWorld.SpawnedVobDict.ContainsKey(item.Address))
                {
                    return 0;
                }

                Item it = (Item)sWorld.SpawnedVobDict[item.Address];

                BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkID.ContainerItemChangedMessage);
                stream.Write((byte)ContainerItemChanged.itemRemoved);
                stream.Write(Player.Hero.ID);
                stream.Write(containerID);
                stream.Write(it.ID);
                stream.Write(amount);
                Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                //stream.Write(item.ID);
                //stream.Write(amount);
                //Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                zERROR.GetZErr(process).Report(2, 'G', "Removed Item, Item: " + item.ObjectName.Value + " Found: " + sWorld.SpawnedVobDict.ContainsKey(item.Address) + " Amount: " + amount, 0, "Itemsynchro.cs", 0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.Source + ": " + ex.Message + " " + ex.StackTrace, 0, "hItemContainer.cs", 0);
            }
            return 0;
        }


        public static void unblockItemInsert(int times)
        {
            itemsUntilBlock = times;
            ItemContainerBlocked = false;

            Process process = Process.ThisProcess();
            process.Write(new byte[] { 0x51, 0x53, 0x55, 0x56, 0x8B }, Program.insertItemToList.oldFuncInNewFunc.ToInt32());
        }
        public static int itemsUntilBlock = 0;
        public static bool ItemContainerBlocked = true;
        public static Int32 oCItemContainer_Insert(String message)
        {
            try
            {
                Process process = Process.ThisProcess();
                if (!ItemContainerBlocked && itemsUntilBlock == 0)
                {
                    process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, Program.insertItemToList.oldFuncInNewFunc.ToInt32());
                    ItemContainerBlocked = true;
                }else if(itemsUntilBlock != 0){
                    itemsUntilBlock -= 1;
                }



                int address = Convert.ToInt32(message);
                

                oCNpc player = oCNpc.Player(process);
                oCItemContainer oIC = new oCItemContainer(process, process.ReadInt(address));
                oCItem item = new oCItem(process, process.ReadInt(address + 4));


                oCMobContainer mC = new oCMobContainer(process, player.GetInteractMob().Address);
                if (!sWorld.SpawnedVobDict.ContainsKey(mC.Address))
                    return 0;
                Vob mobContainerVob = sWorld.SpawnedVobDict[mC.Address];
                if (!(mobContainerVob is MobContainer))
                    return 0;
                MobContainer mobContainer = (MobContainer)mobContainerVob;
                if (!sWorld.SpawnedVobDict.ContainsKey(item.Address))//Multislot-Item
                {
                    
                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkID.ContainerItemChangedMessage);
                    stream.Write((byte)ContainerItemChanged.itemInsertedNew);
                    stream.Write(Player.Hero.ID);
                    stream.Write(mobContainerVob.ID);
                    stream.Write(ItemInstance.getIndex(item.ObjectName.Value.Trim()));
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                }
                else//Normal-Item:
                {
                    //mobContainer.addItem((Item)sWorld.SpawnedVobDict[item.Address]);

                    Item it = (Item)sWorld.SpawnedVobDict[item.Address];
                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkID.ContainerItemChangedMessage);
                    stream.Write((byte)ContainerItemChanged.itemInsertedOld);
                    stream.Write(Player.Hero.ID);
                    stream.Write(mobContainerVob.ID);
                    stream.Write(it.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                }

                



                zERROR.GetZErr(process).Report(2, 'G', "Insert Item, Item: " + sWorld.SpawnedVobDict.ContainsKey(process.ReadInt(address + 4)) + " | " + item.Address + " | " + item.ObjectName.Value + "| " + item.Name.Value + " | " + item.Visual.Value + " |" + " Amount: " + item.Amount, 0, "Itemsynchro.cs", 0);


            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.Source+": "+ex.Message+" "+ex.StackTrace, 0, "hItemContainer.cs", 0);

            }
            return 0;
        }


        public static Int32 StealContainer_setOwner(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            try
            {
                oCNpc player = new oCNpc(process, process.ReadInt(address + 4));
                unblockItemInsert(player.Inventory.ItemList.size());

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "hItemContainer.cs", 0);
            }
            //unblockItemInsert(1);
            return 0;
        }












        public static Int32 oCItemContainer_Remove(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                Process process = Process.ThisProcess();

                oCNpc player = oCNpc.Player(process);
                oCNpc stealNPC = oCNpc.StealNPC(process);
                oCItemContainer oIC = new oCItemContainer(process, process.ReadInt(address));
                int itemIndex = process.ReadInt(address + 4);
                int amount = process.ReadInt(address + 8);
                String itemName = zCParser.getParser(Process.ThisProcess()).GetSymbol(itemIndex).Name.Value;

                oCMobContainer mC = new oCMobContainer(process, player.GetInteractMob().Address);

                if (player.Inventory.Address != oIC.Address)//Use this only with the hero!
                {
                    return 0;
                }
                Item it = Player.Hero.HasItem(ItemInstance.getIndex(itemName));
                if (it == null)//Item was not found!
                {
                    return 0;
                }

                Player.Hero.removeItem(it, amount);
                player.RemoveFromInv(new oCItem(Process.ThisProcess(), it.Address), amount);



                BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkID.ItemRemovedByUsing);
                stream.Write(it.ID);
                stream.Write(amount);
                Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


                zERROR.GetZErr(process).Report(2, 'G', "XXXX-Removed Item, Item: " + itemName + " Amount: " + amount, 0, "Itemsynchro.cs", 0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.Source + ": " + ex.Message + " " + ex.StackTrace, 0, "hItemContainer.cs", 0);
            }
            return 0;
        }
    }
}

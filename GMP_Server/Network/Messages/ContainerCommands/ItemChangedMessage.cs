using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.WorldObjects.Mobs;

namespace GUC.Server.Network.Messages.ContainerCommands
{
    class ItemChangedMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            byte type = 0;
            int itemID = 0, playerID = 0, mobContainerID, amount = 0;
            stream.Read(out type);
            stream.Read(out playerID);
            stream.Read(out mobContainerID);
            stream.Read(out itemID);

            if (!sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Player ID was not found: " + playerID);
            NPCProto npc = (NPCProto)sWorld.VobDict[playerID];
            if (!sWorld.VobDict.ContainsKey(mobContainerID))
                throw new Exception("MobContainer ID was not found: " + mobContainerID);
            IContainer mobContainer = (IContainer)sWorld.VobDict[mobContainerID];


            ContainerItemChanged cic = (ContainerItemChanged)type;
            if (cic == ContainerItemChanged.itemRemoved)
            {
                stream.Read(out amount);
                if (!sWorld.VobDict.ContainsKey(itemID))
                    throw new Exception("Item ID was not found: "+itemID);
                
                Item item = (Item)sWorld.VobDict[itemID];
                

                if (item.ItemInstance.Flags.HasFlag(Flags.ITEM_MULTI))
                {
                    if (item.Amount - amount <= 0)
                    {
                        Item itemInInventory = npc.getItemByInstance(item.ItemInstance);
                        if (itemInInventory == null)
                        {
                            item.ScriptingProto.toContainer(npc.ScriptingNPC);
                        }
                        else
                        {
                            itemInInventory.ScriptingProto.Amount += amount;
                            item.ScriptingProto.Amount = 0;
                        }
                    }
                    else
                    {
                        Item itemInInventory = npc.HasItem(item.ItemInstance.ID);
                        if (itemInInventory == null)
                        {
                            npc.ScriptingNPC.addItem(item.ItemInstance.ScriptingProto, amount);
                            item.ScriptingProto.Amount -= amount;
                        }
                        else
                        {
                            itemInInventory.ScriptingProto.Amount += amount;
                            item.ScriptingProto.Amount -= amount;
                        }
                    }
                }
                else
                {
                    item.ScriptingProto.toContainer(npc.ScriptingNPC);
                    //npc.addItem(item);
                    //Todo: Send it to
                }
            }
            else if (cic == ContainerItemChanged.itemInsertedOld)
            {
                if (!sWorld.VobDict.ContainsKey(itemID))
                    throw new Exception("Item ID was not found: " + itemID);
                Item item = (Item)sWorld.VobDict[itemID];

                if (item.ItemInstance.Flags.HasFlag(Flags.ITEM_MULTI))
                {
                    Item gI = null;
                    List<Item> imList = null;
                    if (mobContainer is MobContainer)
                        imList = ((MobContainer)mobContainer).itemList;
                    else
                        imList = ((NPCProto)mobContainer).ItemList;
                    foreach (Item i in imList)
                    {
                        if (i.ItemInstance == item.ItemInstance)
                        {
                            gI = i;
                            break;
                        }
                    }
                    if (gI == null)
                        if(mobContainer is MobContainer)
                            item.ScriptingProto.toContainer((Scripting.Objects.Mob.MobContainer)((MobContainer)mobContainer).ScriptingVob);
                        else if (mobContainer is NPCProto)
                            item.ScriptingProto.toContainer(((NPCProto)mobContainer).ScriptingNPC);
                    else
                    {
                        gI.ScriptingProto.Amount += item.Amount;
                        item.ScriptingProto.Amount = 0;
                    }
                }
                else
                {
                    if(mobContainer is MobContainer)
                        item.ScriptingProto.toContainer((Scripting.Objects.Mob.MobContainer)((MobContainer)mobContainer).ScriptingVob);
                    else if (mobContainer is NPCProto)
                        item.ScriptingProto.toContainer(((NPCProto)mobContainer).ScriptingNPC);
                }
                
            }
            else if (cic == ContainerItemChanged.itemInsertedNew)
            {
                Console.WriteLine(cic);
                if (!ItemInstance.ItemInstanceDict.ContainsKey(itemID))
                    throw new Exception("Iteminstance ID was not found: " + itemID);
                ItemInstance item = (ItemInstance)ItemInstance.ItemInstanceDict[itemID];
                Console.WriteLine(item);
                Console.WriteLine(mobContainer);
                if(mobContainer is MobContainer)
                    ((Scripting.Objects.Mob.MobContainer)((MobContainer)mobContainer).ScriptingVob).addItem(item.ScriptingProto, 1);
                else if (mobContainer is NPCProto)
                    (((NPCProto)mobContainer).ScriptingNPC).addItem(item.ScriptingProto, 1);
                Console.WriteLine(npc);
                Item i = npc.getItemByInstance(item);
                if (i == null)
                    throw new Exception("NPC has not the item: "+item.ID);
                i.ScriptingProto.Amount -= 1;

            }
        }
    }
}

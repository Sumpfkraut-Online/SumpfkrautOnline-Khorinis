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
        public void Read(RakNet.BitStream stream, Client client)
        {
            byte type = 0;
            int itemID = 0, playerID = 0, mobContainerID, amount = 0;
            stream.Read(out type);
            stream.Read(out playerID);
            stream.Read(out mobContainerID);
            stream.Read(out itemID);

            if (!sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Player ID was not found: " + playerID);
            NPC npc = (NPC)sWorld.VobDict[playerID];
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
                            itemInInventory = item;
                            item.ScriptingProto.toContainer(npc.ScriptingNPC);
                        }
                        else
                        {
                            itemInInventory.ScriptingProto.Amount += amount;
                            item.ScriptingProto.Amount = 0;
                        }

                        UpdateMobContainer(amount, npc, mobContainer, itemInInventory);
                    }
                    else
                    {
                        Item itemInInventory = npc.HasItem(item.ItemInstance.ID);
                        if (itemInInventory == null)
                        {
                            itemInInventory = npc.ScriptingNPC.addItem(item.ItemInstance.ScriptingProto, amount).ProtoItem;
                            item.ScriptingProto.Amount -= amount;
                        }
                        else
                        {
                            itemInInventory.ScriptingProto.Amount += amount;
                            item.ScriptingProto.Amount -= amount;
                        }

                        UpdateMobContainer(amount, npc, mobContainer, itemInInventory);

                    }
                }
                else
                {
                    item.ScriptingProto.toContainer(npc.ScriptingNPC);

                    UpdateMobContainer(item.Amount, npc, mobContainer, item);                   
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
                    {
                        imList = ((MobContainer)mobContainer).itemList;
                    }
                    else
                    {
                        imList = ((NPC)mobContainer).ItemList;
                    }
                    foreach (Item i in imList)
                    {
                        if (i.ItemInstance == item.ItemInstance)
                        {
                            gI = i;
                            break;
                        }
                    }
                    if (gI == null)
                    {
                        if (mobContainer is MobContainer)
                        {
                            item.ScriptingProto.toContainer((Scripting.Objects.Mob.MobContainer)((MobContainer)mobContainer).ScriptingVob);
                        }
                        else if (mobContainer is NPC)
                        {
                            item.ScriptingProto.toContainer(((NPC)mobContainer).ScriptingNPC);
                        }

                        UpdateMobContainer(item.Amount, npc, mobContainer, item);
                        
                    }
                    else
                    {
                        int _amount = item.Amount;
                        gI.ScriptingProto.Amount += item.Amount;
                        item.ScriptingProto.Amount = 0;

                        UpdateMobContainer(_amount, npc, mobContainer, gI);
                    }
                }
                else
                {
                    if(mobContainer is MobContainer)
                        item.ScriptingProto.toContainer((Scripting.Objects.Mob.MobContainer)((MobContainer)mobContainer).ScriptingVob);
                    else if (mobContainer is NPC)
                        item.ScriptingProto.toContainer(((NPC)mobContainer).ScriptingNPC);

                    UpdateMobContainer(item.Amount, npc, mobContainer, item);
                    
                }
                
            }
            else if (cic == ContainerItemChanged.itemInsertedNew)
            {
                
                if (!ItemInstance.ItemInstanceDict.ContainsKey(itemID))
                    throw new Exception("Iteminstance ID was not found: " + itemID);
                ItemInstance item = (ItemInstance)ItemInstance.ItemInstanceDict[itemID];

                Item newItem = null;
                if(mobContainer is MobContainer)
                    newItem = ((Scripting.Objects.Mob.MobContainer)((MobContainer)mobContainer).ScriptingVob).addItem(item.ScriptingProto, 1).ProtoItem;
                else if (mobContainer is NPC)
                    newItem = (((NPC)mobContainer).ScriptingNPC).addItem(item.ScriptingProto, 1).ProtoItem;
                
                Item i = npc.getItemByInstance(item);
                if (i == null)
                    throw new Exception("NPC has not the item: "+item.ID);
                i.ScriptingProto.Amount -= 1;

                UpdateMobContainer(1, npc, mobContainer, newItem);
               
            }
        }

        private static void UpdateMobContainer(int amount, NPC npc, IContainer mobContainer, Item item)
        {
          if (mobContainer is MobContainer)
          {
            Scripting.Objects.Mob.MobContainer.isOnTakeItemMessage(
                (Scripting.Objects.Mob.MobContainer)((MobContainer)mobContainer).ScriptingVob,
                (Scripting.Objects.Character.Player)npc.ScriptingNPC, item.ScriptingProto, amount);
          }
        }
    }
}

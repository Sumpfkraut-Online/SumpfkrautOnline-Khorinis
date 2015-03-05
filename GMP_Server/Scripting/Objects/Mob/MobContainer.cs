using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using System.Collections;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripting.Objects.Mob
{
    public class MobContainer : MobLockable, IEnumerable
    {
        internal MobContainer(GUC.WorldObjects.Mobs.MobContainer vob)
            : base(vob)
        {
            
        }


        public MobContainer(String visual)
            : this(visual, null, null, null)
        { }
        public MobContainer(String visual, String focusName)
            : this(visual, focusName, null, null)
        { }
        public MobContainer(String visual, String focusName, ItemInstance[] items, int[] amounts)
            : this(visual, focusName, items, amounts, false, null, null, null, null, true, true)
        { }
        public MobContainer(String visual, String focusName, ItemInstance[] items, int[] amounts, bool cdDyn, bool cdStatic)
            : this(visual, focusName, items, amounts, false, null, null, null, null, cdDyn, cdStatic)
        { }
        public MobContainer(String visual, String focusName, ItemInstance[] items, int[] amounts, bool isLocked, ItemInstance keyInstance, String pickLockString)
            : this(visual, focusName, items, amounts, isLocked, keyInstance, pickLockString, null, null, true, true)
        { }
        public MobContainer(String visual, String focusName, ItemInstance[] items, int[] amounts, bool isLocked, ItemInstance keyInstance, String pickLockString, bool cdDyn, bool cdStatic)
            : this(visual, focusName, items, amounts, isLocked, keyInstance, pickLockString, null, null, cdDyn, cdStatic)
        { }
        public MobContainer(String visual, String focusName, ItemInstance[] items, int[] amounts, bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, String triggerTarget)
            : this(visual, focusName, items, amounts, isLocked, keyInstance, pickLockString, useWithItem, triggerTarget, true, true)
        { }
        public MobContainer(String visual, String focusName, ItemInstance[] items, int[] amounts, bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic)
            : this(new GUC.WorldObjects.Mobs.MobContainer(), visual, focusName, items, amounts, isLocked, keyInstance, pickLockString, useWithItem, triggerTarget, cdDyn, cdStatic, true)
        { }

        
        protected MobContainer()
            : this(new GUC.WorldObjects.Mobs.MobContainer(), null, null, null, null, false, null, null, null, null, false, false, false)
        {

        }

        internal MobContainer(GUC.WorldObjects.Mobs.MobContainer mobInter, String visual, String focusName, ItemInstance[] items, int[] amounts, bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic, bool useCreate)
            : base(mobInter, visual, focusName, isLocked, keyInstance, pickLockString, useWithItem, triggerTarget, cdDyn, cdStatic)
        {
            if (items != null && amounts != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    addItem(items[i], amounts[i]);
                }
            }

            if (useCreate)
                CreateVob();
        }

        internal new GUC.WorldObjects.Mobs.MobContainer Proto { get { return (GUC.WorldObjects.Mobs.MobContainer)vob; } }

        public Item addItem(ItemInstance iteminstance, int amount)
        {
            if (iteminstance == null)
                throw new ArgumentNullException("Instance can't be null!");
            if (amount <= 0)
                throw new ArgumentException("amount can't be 0 or lower!");

            if (iteminstance.itemInstances.Flags.HasFlag(Flags.ITEM_MULTI))
            {
                Item wIT = null;
                foreach (WorldObjects.Item wIT2 in this.Proto.itemList)
                {
                    if (wIT2.ItemInstance == iteminstance.itemInstances)
                    {
                        wIT = wIT2.ScriptingProto;
                        break;
                    }
                }

                if (wIT != null)
                {
                    wIT.Amount += amount;
                    return wIT;
                }
            }


            Item i = new Item(iteminstance, amount);

            Proto.addItem(i.ProtoItem);
            
            if (!created)
                return i;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AddItemMessage);
            stream.Write(Proto.ID);
            stream.Write(i.ID);
            
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            
            return i;
        }

        public void removeItem(Item item)
        {
            if (item.ProtoItem.Container != this.Proto)
                throw new ArgumentException("Item is not in the container");

            item.Amount = 0;
        }



        public IEnumerator GetEnumerator()
        {
            foreach(GUC.WorldObjects.Item item in Proto.itemList){
                yield return item.ScriptingProto;
            }
        }

        public int ItemCount { get { return Proto.itemList.Count; } }

        public Item this[int i]
        {
            get { return Proto.itemList[i].ScriptingProto; }
        }


        #region Events
        public event GUC.Server.Scripting.Events.ContainerItemMessage OnPutItemToContainer;
        internal void iOnPutItemToContainer(MobContainer container, Player pl, Item item, int amount)
        {
            if (OnPutItemToContainer != null)
                OnPutItemToContainer(container, pl, item, amount);
        }


        public static event Events.ContainerItemMessage sOnPutItemToContainer;
        internal static void isOnPutItemToContainer(MobContainer container, Player pl, Item item, int amount)
        {
            container.iOnPutItemToContainer(container, pl, item, amount);
            if (sOnPutItemToContainer != null)
                sOnPutItemToContainer(container, pl, item, amount);
        }

        public event GUC.Server.Scripting.Events.ContainerItemMessage OnTakeItemFromContainer;
        internal void iOnTakeItemFromContainer(MobContainer container, Player pl, Item item, int amount)
        {
            if (OnTakeItemFromContainer != null)
                OnTakeItemFromContainer(container, pl, item, amount);
        }


        public static event Events.ContainerItemMessage sOnTakeItemFromContainer;
        internal static void isOnTakeItemMessage(MobContainer container, Player pl, Item item, int amount)
        {
            container.iOnTakeItemFromContainer(container, pl, item, amount);
            if (sOnTakeItemFromContainer != null)
                sOnTakeItemFromContainer(container, pl, item, amount);
        }
        #endregion
    }
}

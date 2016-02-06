using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.Collections
{
    public interface IContainer
    {
        ItemContainer Inventory { get; }
    }

    public partial class ItemContainer
    {
        Dictionary<uint, Item> items = new Dictionary<uint, Item>();

        public IContainer Container { get; protected set; }

        public int Count { get { return items.Count; } }

        public ItemContainer(IContainer container)
        {
            this.Container = container;
        }

        public Item Get(uint id)
        {
            Item item;
            items.TryGetValue(id, out item);
            return item;
        }

        public bool Contains(Item item)
        {
            if (item == null)
                return false;

            return item.Container == this.Container;
        }

        public IEnumerable<Item> GetAll()
        {
            return items.Values;
        }

        partial void pAdd(Item item);
        public void Add(Item item)
        {
            if (item != null && item.IsCreated/* && item != Item.Fists*/)
            {
                if (items.Count >= ushort.MaxValue)
                {
                    throw new Exception("ItemContainer maximum reached! " + ushort.MaxValue);
                }

                if (item.IsSpawned)
                {
                    item.Despawn(); //Fixme?: Send despawn + additem msg in one msg to the new owner
                }

                if (item.Container != null)
                {
                    item.Container.Inventory.Remove(item);
                }

                items.Add(item.ID, item);

                pAdd(item);
            }
        }

        partial void pRemove(Item item);
        public void Remove(Item item)
        {
            if (item != null && this.Contains(item))
            {
                if (this.Container != null && this.Container is NPC)
                {
                    //unequip
                    //undraw                
                }

                items.Remove(item.ID);

                pRemove(item);
            }
        }

        internal void WriteContents(PacketWriter stream)
        {
            stream.Write((ushort)items.Count);
            foreach (Item item in items.Values)
            {
                item.WriteInventoryProperties(stream);
            }
        }
    }
}

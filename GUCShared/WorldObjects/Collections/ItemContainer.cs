using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public partial class ItemContainer
    {
        public interface IContainer
        {
            ItemContainer Inventory { get; }
        }

        /// <summary>
        /// The upper (excluded) limit for Items in an inventory (byte.MaxValue+1).
        /// </summary>
        public const int MaxItems = 256;

        IContainer owner;
        public IContainer Owner { get { return this.owner; } }

        StaticCollection<Item> idColl = new StaticCollection<Item>(MaxItems);
        DynamicCollection<Item> items = new DynamicCollection<Item>(MaxItems);

        internal ItemContainer(IContainer owner)
        {
            //if (owner == null)
            //    throw new ArgumentNullException("Owner is null!");

            this.owner = owner;
        }

        #region Add & Remove

        partial void pAdd(Item item);
        public void Add(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item is null!");
            }
            else if (item.Container != null)
            {
                throw new ArgumentException("Item is already in a container!");
            }
            else if (item.IsSpawned)
            {
                throw new ArgumentException("Item is spawned in the world!");
            }

            idColl.Add(item);
            items.Add(item, ref item.collID);
            item.Container = this.Owner;

            pAdd(item);
        }

        partial void pRemove(Item item);
        public void Remove(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item is null!");
            }
            else if (item.Container != this.Owner)
            {
                throw new ArgumentException("Item is in a different container!");
            }

            idColl.Remove(item);
            items.Remove(ref item.collID);
            item.Container = null;

            pRemove(item);
        }

        #endregion

        #region Access

        public int GetCount() { return items.Count; }

        public bool TryGetItem(int id, out Item item)
        {
            return idColl.TryGet(id, out item);
        }

        public void ForEachItem(Action<Item> action)
        {
            items.ForEach(action);
        }

        /// <summary>
        /// Return FALSE to break the loop.
        /// </summary>
        public void ForEachItem(Predicate<Item> action)
        {
            items.ForEachPredicate(action);
        }

        #endregion
    }
}

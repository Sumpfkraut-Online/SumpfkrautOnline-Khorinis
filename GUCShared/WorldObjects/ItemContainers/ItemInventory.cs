using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GameObjects;
using GUC.GameObjects.Collections;

namespace GUC.WorldObjects.ItemContainers
{
    public partial class ItemInventory : GameObject
    {
        #region ScriptObject

        public partial interface IScriptItemInventory : IScriptGameObject
        {
            void AddItem(Item item);
            void RemoveItem(Item item);
        }

        public new IScriptItemInventory ScriptObject { get { return (IScriptItemInventory)base.ScriptObject; } }

        #endregion

        #region Constructors

        internal ItemInventory(ItemContainer owner, IScriptItemInventory scriptObject) : base(scriptObject)
        {
            if (owner == null)
                throw new ArgumentNullException("Owner is null!");

            this.owner = owner;
        }

        #endregion

        #region Properties

        /// <summary> The upper (excluded) limit for Items in an inventory (byte.MaxValue+1). </summary>
        public const int MaxItems = 256;

        ItemContainer owner;
        public ItemContainer Owner { get { return this.owner; } }

        StaticCollection<Item> idColl = new StaticCollection<Item>(MaxItems);
        DynamicCollection<Item> items = new DynamicCollection<Item>(MaxItems);

        #endregion

        #region Add & Remove

        partial void pAfterAdd(Item item);
        public virtual void Add(Item item)
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

            pAfterAdd(item);
        }

        partial void pAfterRemove(Item item);
        public virtual void Remove(Item item)
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

            pAfterRemove(item);
        }

        #endregion

        #region Access

        /// <summary> Returns the count of all Items in this ItemInventory. </summary>
        public int Count { get { return items.Count; } }

        /// <summary> Gets an Item with the given ID or null. </summary>
        public bool TryGetItem(int id, out Item item)
        {
            return idColl.TryGet(id, out item);
        }

        /// <summary> Loops through all Items in this ItemInventory. </summary>
        public void ForEach(Action<Item> action)
        {
            items.ForEach(action);
        }

        /// <summary> 
        /// Loops through all Items in this ItemInventory. 
        /// Let the predicate return FALSE to BREAK the loop.
        /// </summary>
        public void ForEachPredicate(Predicate<Item> action)
        {
            items.ForEachPredicate(action);
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
        }

        protected override void ReadProperties(PacketReader stream)
        {
        }

        #endregion
    }
}

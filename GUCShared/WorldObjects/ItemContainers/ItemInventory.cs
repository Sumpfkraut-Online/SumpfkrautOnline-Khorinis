using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GameObjects;
using GUC.GameObjects.Collections;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects.ItemContainers
{
    public partial class ItemInventory : GameObject
    {
        #region ScriptObject

        public partial interface IScriptItemInventory : IScriptGameObject
        {
            void AddItem(GUCItemInst item);
            void RemoveItem(GUCItemInst item);
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

        StaticCollection<GUCItemInst> idColl = new StaticCollection<GUCItemInst>(MaxItems);
        DynamicCollection<GUCItemInst> items = new DynamicCollection<GUCItemInst>(MaxItems);

        #endregion

        #region Add & Remove

        partial void pAfterAdd(GUCItemInst item);
        public virtual void Add(GUCItemInst item)
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

        partial void pAfterRemove(GUCItemInst item);
        public virtual void Remove(GUCItemInst item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item is null!");
            }
            else if (item.Container != this.Owner)
            {
                throw new ArgumentException("Item is in a different container!");
            }

            item.Container = null;
            idColl.Remove(item);
            items.Remove(ref item.collID);

            pAfterRemove(item);
        }

        #endregion

        #region Access

        /// <summary> Returns the count of all Items in this ItemInventory. </summary>
        public int Count { get { return items.Count; } }

        /// <summary> Gets an Item with the given ID or null. </summary>
        public bool TryGetItem(int id, out GUCItemInst item)
        {
            return idColl.TryGet(id, out item);
        }

        /// <summary> Loops through all Items in this ItemInventory. </summary>
        public void ForEach(Action<GUCItemInst> action)
        {
            items.ForEach(action);
        }

        /// <summary> 
        /// Loops through all Items in this ItemInventory. 
        /// Let the predicate return FALSE to BREAK the loop.
        /// </summary>
        public void ForEachPredicate(Predicate<GUCItemInst> action)
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

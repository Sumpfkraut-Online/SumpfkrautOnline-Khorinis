using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects;
using GUC.WorldObjects.ItemContainers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances.ItemContainers
{

    public partial class ScriptInventory : ItemInventory.IScriptItemInventory
    {
        public interface IContainer
        {
            ItemInventory BaseInventory { get; }
            ScriptInventory Inventory { get; }
        }

        public delegate void RemoveItemHandler(ItemInst item);
        public event RemoveItemHandler OnRemoveItem;

        public delegate void AddItemHandler(ItemInst item);
        public event AddItemHandler OnAddItem;

        #region Constructors

        public ScriptInventory(IContainer owner)
        {
            if (owner == null)
                throw new ArgumentNullException("Owner is null!");

            this.owner = owner;
        }

        #endregion

        #region Properties

        IContainer owner;
        public IContainer Owner { get { return this.owner; } }

        public ItemInventory BaseInst { get { return this.owner.BaseInventory; } }

        #endregion

        #region Add & Remove Items

        public void ForEachItem(Action<ItemInst> action)
        {
            BaseInst.ForEach(i => action((ItemInst)i.ScriptObject));
        }

        public void ForEachItemPredicate(Predicate<ItemInst> predicate)
        {
            BaseInst.ForEachPredicate(i => { return predicate((ItemInst)i.ScriptObject); });
        }

        public void AddItem(Item item)
        {
            AddItem((ItemInst)item.ScriptObject);
        }

        public void RemoveItem(Item item)
        {
            RemoveItem((ItemInst)item.ScriptObject);
        }
        
        public void AddItem(ItemInst item)
        {
            BaseInst.Add(item.BaseInst);
            item.OnSetAmount += Item_OnSetAmount;
            OnAddItem?.Invoke(item);
        }

        public delegate void OnItemAmountChangeHandler(ItemInst item);
        public event OnItemAmountChangeHandler OnItemAmountChange;
        void Item_OnSetAmount(ItemInst item)
        {
            OnItemAmountChange?.Invoke(item);
        }

        public void RemoveItem(ItemInst item)
        {
            BaseInst.Remove(item.BaseInst);
            item.OnSetAmount -= Item_OnSetAmount;
            OnRemoveItem?.Invoke(item);
        }

        public ItemInst GetItem(int id)
        {
            if (BaseInst.TryGetItem(id, out Item item))
                return (ItemInst)item.ScriptObject;
            return null;
        }

        public void Clear()
        {
            BaseInst.ForEach(i => RemoveItem((ItemInst)i.ScriptObject));
        }

        #endregion

        #region Read & Write

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        public void OnReadProperties(PacketReader stream)
        {
        }

        #endregion
    }
}

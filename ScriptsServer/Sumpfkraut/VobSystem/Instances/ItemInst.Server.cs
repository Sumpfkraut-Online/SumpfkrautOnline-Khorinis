using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ItemInst
    {
        public int Damage { get { return this.Definition.Damage; } }
        public float Range { get { return this.Definition.Range; } }
        public int Protection { get { return this.Definition.Protection; } }

        public ItemInst(ItemDef def) : this()
        {
            this.Definition = def;
        }

        /// <summary>
        /// Creates a new item from the old item with the given amount and reduces the old item's amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public ItemInst Split(int amount)
        {
            int newAmount = this.Amount - amount;

            if (newAmount > 0)
            {
                this.SetAmount(newAmount);
                // split item
                ItemInst newItem = new ItemInst(this.Definition);
                newItem.SetAmount(amount);
                return newItem;
            }
            else if (newAmount < 0)
            {
                this.Remove();
                return this;
            }
            return null;
        }
    }
}

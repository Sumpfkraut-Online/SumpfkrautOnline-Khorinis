using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public class ItemContainer
    {
        public interface IContainer
        {
            ItemContainer Inventory { get; }
        }

        /// <summary>
        /// The upper (excluded) limit for Items in an inventory (byte.MaxValue+1).
        /// </summary>
        public const int MAX_ITEMS = 256;

        IContainer owner;
        public IContainer Owner { get { return this.owner; } }

        public ItemContainer(IContainer owner)
        {
            this.owner = owner;
        }
    }
}

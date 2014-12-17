using System;
using System.Collections.Generic;
using System.Text;

namespace GUC.WorldObjects
{
    internal partial class Item : Vob
    {
        public Item()
            : base()
        {
            this.VobType = Enumeration.VobTypes.Item;
        }

        protected ItemInstance itemInstance = null;
        protected int amount = 0;

        protected IContainer container = null;


        public int Amount { get { return amount; } set { amount = value; } }
        public IContainer Container { get { return container; } set { container = value; } }
        public ItemInstance ItemInstance { get { return itemInstance; } }
    }
}

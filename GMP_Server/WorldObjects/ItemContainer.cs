using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects
{
    public interface ItemContainer
    {
        bool HasItem(ItemInstance instance, int amount);
        bool AddItem(ItemInstance instance, int amount);
        void RemoveItem(ItemInstance instance, int amount);
    }
}

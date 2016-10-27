using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.ItemContainers
{
    public interface ItemContainer
    {
        ItemInventory Inventory { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects;

namespace GUC.WorldObjects.ItemContainers
{
    public partial class ItemInventory : GameObject
    {
        public static ItemInventory PlayerInventory { get { return NPC.Hero?.Inventory; } }
    }
}

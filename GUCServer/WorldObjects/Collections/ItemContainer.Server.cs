using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network.Messages;

namespace GUC.WorldObjects.Collections
{
    public partial class ItemContainer
    {
        partial void pAdd(Item item)
        {
            if (this.Owner is NPC && ((NPC)this.Owner).IsSpawned)
                InventoryMessage.WriteAddItem(((NPC)this.Owner).client, item);
        }

        partial void pRemove(Item item)
        {
            if (this.Owner is NPC && ((NPC)this.Owner).IsSpawned)
                InventoryMessage.WriteRemoveItem(((NPC)this.Owner).client, item);
        }
    }
}

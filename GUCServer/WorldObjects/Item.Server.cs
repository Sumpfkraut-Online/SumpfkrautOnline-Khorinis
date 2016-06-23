using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network.Messages;

namespace GUC.WorldObjects
{
    public partial class Item : Vob
    {

        /*public override void Delete()
        {
            if (Container != null)
            {
                Container.Inventory.Remove(this);
            }
            base.Delete();
        }

        internal override void WriteSpawnMessage(IEnumerable<Client> list)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldItemSpawnMessage);
            this.WriteSpawnProperties(stream);

            foreach (Client client in list)
            {
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            }
        }*/

        partial void pSetAmount(int amount)
        {
            if (this.Container != null && this.Container is NPC)
            {
                NPC owner = (NPC)this.Container;
                if (owner.IsPlayer)
                {
                    if (amount > 0)
                    {
                        InventoryMessage.WriteChangeItemAmount(owner.client, this, amount);
                    }
                    else
                    {
                        InventoryMessage.WriteRemoveItem(owner.client, this);
                    }
                }
            }
        }
    }
}

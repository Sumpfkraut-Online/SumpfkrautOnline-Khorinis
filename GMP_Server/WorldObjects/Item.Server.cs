using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Server.Network;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class Item : Vob
    {   
        public override void Delete()
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
        }
    }
}

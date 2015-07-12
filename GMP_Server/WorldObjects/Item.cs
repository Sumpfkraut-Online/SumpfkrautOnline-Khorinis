using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Server.Network;
using GUC.Enumeration;
using GUC.Types;

namespace GUC.Server.WorldObjects
{
    public class Item : AbstractDropVob
    {
        ItemInstance instance = null;
        public ItemInstance Instance { get { return instance; } }
        public int Amount = 1;
        
        public Item(ushort instanceID)  : this(ItemInstance.InstanceList[instanceID])
        {
        }

        public Item(String instanceName) : base()
        {
            ItemInstance.InstanceDict.TryGetValue(instanceName, out instance);
            if (instance == null)
            {
                sWorld.RemoveVob(this);
                throw new Exception("Create item failed: instance name unknown: " + instanceName);
            }
        }

        public Item(ItemInstance inst) : base()
        {
            instance = inst;
        }

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldItemSpawnMessage);
            stream.mWrite(ID);
            stream.mWrite(instance.ID);
            stream.mWrite(pos);
            stream.mWrite(dir);
            stream.mWrite(physicsEnabled);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, client.guid, false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Server.Network;
using GUC.Server.Network.Messages;
using GUC.Enumeration;
using GUC.Types;

namespace GUC.Server.WorldObjects
{
    public class Item : AbstractDropVob
    {
        /// <summary>
        /// Gets the ItemInstance of this item.
        /// </summary>
        public ItemInstance instance { get; protected set; }

        /// <summary>
        /// Gets or sets the amount of this item. Setting the amount to zero will delete the item.
        /// </summary>
        public ushort amount
        {
            get { return iAmount; }
            set
            {
                if (value > 0)
                {
                    iAmount = value;
                    if (this.owner != null)
                    {
                        InventoryMessage.WriteAmountUpdate(this.owner.client, this);
                    }
                }
                else
                {
                    RemoveFromServer();
                }

            }
        }

        internal ushort iAmount = 1;

        /// <summary>
        /// Stackable items will combine themselves to one single object in inventories.
        /// Set this to false for unique items like user-written scrolls, worn weapons etc.
        /// Default is TRUE.
        /// </summary>
        public bool stackable = true;

        /// <summary>
        /// Gets the NPC who is carrying this item or NULL.
        /// </summary>
        public NPC owner { get; internal set; }

        public ushort condition;

        #region Constructors

        /// <summary>
        /// Creates and returns an Item object from the given ItemInstance-ID.
        /// Returns NULL when the ID is not found!
        /// </summary>
        public static Item Create(ushort instanceID)
        {
            ItemInstance inst = ItemInstance.Table.Get(instanceID);
            if (inst != null)
            {
                return Create(inst);
            }
            else
            {
                Log.Logger.logError("Item creation failed! Instance ID not found: " + instanceID);
                return null;
            }
        }

        /// <summary>
        /// Creates and returns an Item object from the given ItemInstance-Name.
        /// Returns NULL when the name is not found!
        /// </summary>
        public static Item Create(string instanceName)
        {
            ItemInstance inst = ItemInstance.Table.Get(instanceName);
            if (inst != null)
            {
                return Create(inst);
            }
            else
            {
                Log.Logger.logError("Item creation failed! Instance name not found: " + instanceName);
                return null;
            }
        }

        /// <summary>
        /// Creates and returns an Item object from the given ItemInstance.
        /// Returns NULL when the ItemInstance is NULL!
        /// </summary>
        public static Item Create(ItemInstance instance)
        {
            if (instance != null)
            {
                Item item = new Item();
                item.instance = instance;
                item.condition = instance.condition;
                return item;
            }
            else
            {
                Log.Logger.logError("Item creation failed! Instance can't be NULL!");
                return null;
            }
        }

        protected Item()
        {
        }

        #endregion

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldItemSpawnMessage);
            stream.mWrite(ID);
            stream.mWrite(instance.ID);
            stream.mWrite(pos);
            stream.mWrite(dir);
            stream.mWrite(physicsEnabled);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        public override void RemoveFromServer()
        {
            if (owner == null)
            {
                Despawn();
            }
            else
            {
                owner.RemoveItem(this);
            }
            sWorld.RemoveVob(this);
        }
    }
}

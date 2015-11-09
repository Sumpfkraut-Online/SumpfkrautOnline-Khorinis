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
        public ItemInstance Instance { get; protected set; }

        public ItemType Type { get { return Instance.Type; } }

        /// <summary>
        /// Gets or sets the amount of this item. Setting the amount to zero will delete the item. Can't be set when spawned. 
        /// </summary>
        public ushort Amount
        {
            get { return amount; }
            set
            {
                if (value > 0)
                {
                    if (this.Spawned)
                        return;

                    amount = value;
                    if (this.Owner != null)
                    {
                        InventoryMessage.WriteAmountUpdate(this.Owner.client, this);
                    }
                }
                else
                {
                    RemoveFromServer();
                }

            }
        }
        internal ushort amount = 1;

        /// <summary>
        /// Stackable items will combine themselves to one single object in inventories.
        /// Set this to false for unique items like user-written scrolls, worn weapons etc.
        /// Default is TRUE.
        /// </summary>
        public bool Stackable = true;

        /// <summary>
        /// Gets the NPC who is carrying this item or NULL.
        /// </summary>
        public NPC Owner { get; internal set; }

        /// <summary>
        /// Gets the equipment slot for the owner NPC.
        /// </summary>
        public int Slot { get; internal set; }

        /// <summary>
        /// The condition of this item. Only used for weapons, armors, etc. Can't be set when spawned. 
        /// </summary>
        public ushort Condition
        {
            get { return condition; }
            set
            {
                if (!Spawned)
                {
                    condition = value;
                }
            }
        }
        protected ushort condition;

        /// <summary>
        /// Changeable extra line in the item description box in the inventory. Use for signatures, etc.
        /// </summary>
        public string SpecialLine
        {
            get { return specialLine; }
            set
            {
                specialLine = value;
                if (Owner != null)
                {

                }
            }
        }
        protected string specialLine = "";

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
                item.Instance = instance;
                item.condition = instance.Condition;
                item.Slot = -1;
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

        public static Item Copy(Item source)
        {
            Item newItem = new Item();
            newItem.Instance = source.Instance;
            newItem.condition = source.condition;
            newItem.specialLine = source.specialLine;
            return newItem;
        }

        #endregion

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldItemSpawnMessage);
            stream.mWrite(ID);
            stream.mWrite(Instance.ID);
            stream.mWrite(pos);
            stream.mWrite(dir);
            stream.mWrite(amount);
            if (Instance.Type <= ItemType.Armor)
                stream.mWrite(condition);
            stream.mWrite(physicsEnabled);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        public override void RemoveFromServer()
        {
            if (Owner == null)
            {
                Despawn();
            }
            else
            {
                Owner.RemoveItem(this);
            }
            sWorld.RemoveVob(this);
        }
    }
}

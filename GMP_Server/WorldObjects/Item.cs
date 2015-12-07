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
        public static readonly Item Fists = CreateFists();
        static Item CreateFists()
        {
            Item fists = new Item(null);
            fists.ID = 0;
            fists.Slot = 0;
            Network.Server.sAllVobsDict.Remove(0);
            return fists;
        }

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
                    if (this.IsSpawned)
                        return;

                    amount = value;
                    if (this.Owner != null)
                    {
                        InventoryMessage.WriteAmountUpdate(this.Owner.client, this);
                    }
                }
                else
                {
                    Delete();
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
        public byte Slot { get; internal set; }

        /// <summary>
        /// The condition of this item. Only used for weapons, armors, etc. Can't be set when spawned. 
        /// </summary>
        public ushort Condition
        {
            get { return condition; }
            set
            {
                if (!IsSpawned)
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
        public static Item Create(ushort instanceID, object scriptObject)
        {
            ItemInstance inst = ItemInstance.Table.Get(instanceID);
            if (inst != null)
            {
                return Create(inst, scriptObject);
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
        public static Item Create(string instanceName, object scriptObject)
        {
            ItemInstance inst = ItemInstance.Table.Get(instanceName);
            if (inst != null)
            {
                return Create(inst, scriptObject);
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
        public static Item Create(ItemInstance instance, object scriptObject)
        {
            if (instance != null)
            {
                Item item = new Item(scriptObject);
                item.Instance = instance;
                item.condition = instance.Condition;
                Server.Network.Server.sItemDict.Add(item.ID, item);
                return item;
            }
            else
            {
                Log.Logger.logError("Item creation failed! Instance can't be NULL!");
                return null;
            }
        }

        internal Item(object scriptObject) : base(scriptObject)
        {
        }

        public override void Delete()
        {
            if (Owner == null)
            {
                Despawn();
            }
            else
            {
                Owner.RemoveItem(this);
            }
            base.Delete();
            Server.Network.Server.sItemDict.Remove(this.ID);
        }

        public static Item Copy(Item source)
        {
            Item newItem = Item.Create(source.Instance, source.ScriptObj);
            newItem.condition = source.condition;
            newItem.specialLine = source.specialLine;
            return newItem;
        }

        #endregion

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.WorldItemSpawnMessage);
            stream.Write(ID);
            stream.Write(Instance.ID);
            stream.Write(pos);
            stream.Write(dir);
            stream.Write(amount);
            if (Instance.Type <= ItemType.Armor)
                stream.Write(condition);
            stream.Write(physicsEnabled);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }
    }
}

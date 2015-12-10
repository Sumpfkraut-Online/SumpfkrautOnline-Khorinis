using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Server.Network;
using GUC.Enumeration;
using GUC.Server.WorldObjects.Collections;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    public class Item : Vob
    {
        new public static readonly VobDictionary Vobs = Network.Server.sVobs.GetDict(ItemInstance.sVobType);

        public static readonly Item Fists = CreateFists();
        static Item CreateFists()
        {
            Item fists = new Item(ItemInstance.FistInstance, null);
            fists.ID = 0;
            fists.Slot = 0;
            return fists;
        }

        public const int TextAndCountLen = ItemInstance.TextAndCountLen;

        /// <summary>
        /// Gets the ItemInstance of this item.
        /// </summary>
        new public ItemInstance Instance { get; protected set; }

        public string Name { get { return Instance.Name; } }
        public ItemType Type { get { return Instance.Type; } }
        public ItemMaterial Material { get { return Instance.Material; } }
        public string[] Text { get { return Instance.Text; } }
        public ushort[] Count { get { return Instance.Count; } }
        public string Description { get { return Instance.Description; } }
        public string VisualChange { get { return Instance.VisualChange; } }
        public string Effect { get { return Instance.Effect; } }
        public ItemInstance Munition { get { return Instance.Munition; } }

        /// <summary>
        /// Gets or sets the amount of this item. Setting the amount to zero will delete the item. Can't be set when spawned. 
        /// </summary>
        public ushort Amount = 1;

        /// <summary>
        /// Gets the NPC who is carrying this item or NULL.
        /// </summary>
        public NPC Owner { get; internal set; }

        /// <summary>
        /// Gets the equipment slot for the owner NPC.
        /// </summary>
        public byte Slot { get; internal set; }
        
        public Item(ItemInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }

        public override void Delete()
        {
            if (Owner != null)
            {
                Owner.RemoveItem(this);
            }
            base.Delete();
        }

        new public static Action<Item, PacketWriter> OnWriteSpawn;
        internal override void WriteSpawn(PacketWriter stream)
        {
            base.WriteSpawn(stream);

            if (Item.OnWriteSpawn != null)
                Item.OnWriteSpawn(this, stream);
        }

        internal override void WriteSpawnMessage(IEnumerable<Client> list)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.WorldItemSpawnMessage);
            this.WriteSpawn(stream);

            foreach (Client client in list)
            {
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            }
        }

        public static Action<Item, PacketWriter> OnWriteEquipped;
        internal void WriteEquipped(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.Instance.ID);

            if (OnWriteEquipped != null)
                OnWriteEquipped(this, stream);
        }

        public static Action<Item, PacketWriter> OnWriteInventory;
        internal void WriteInventory(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.Instance.ID);
            stream.Write(this.Amount);

            if (OnWriteInventory != null)
                OnWriteInventory(this, stream);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class Item : Vob
    {
        public override VobTypes VobType { get { return VobTypes.Item; } }

        #region ScriptObject

        public partial interface IScriptItem : IScriptVob
        {
            void WriteEquipProperties(PacketWriter stream);
            void ReadEquipProperties(PacketReader stream);

            void WriteInventoryProperties(PacketWriter stream);
            void ReadInventoryProperties(PacketReader stream);
        }

        new public IScriptItem ScriptObject
        {
            get { return (IScriptItem)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        public override bool IsStatic
        {
            get { return base.IsStatic; }
            set
            {
                if (this.Container != null)
                    throw new Exception("IsStatic can't be changed when the item is in an inventory!");
                base.IsStatic = value;
            }
        }

        new public ItemInstance Instance
        {
            get { return (ItemInstance)base.Instance; }
            set
            {
                if (this.Container != null)
                {
                    throw new Exception("ItemInstance can't be changed while in an Inventory!");
                }
                base.Instance = value;
            }
        }

        /// <summary>
        /// The upper (excluded) limit for Item amounts (ushort.MaxValue + 1).
        /// </summary>
        public const int MAX_AMOUNT = 65536;

        int amount = 1;
        public int Amount { get { return this.amount; } }

        public void SetAmount(int amount)
        {
            if (amount < 0 || amount >= MAX_AMOUNT)
            {
                throw new Exception("Item amount is out of range! 0.." + MAX_AMOUNT);
            }

            this.amount = amount;

            if (this.Container != null)
            {
                // send msg
            }
        }

        public const int SlotNumUnused = 255;
        internal int slot = SlotNumUnused;
        public int Slot { get { return this.slot; } }
        public bool IsEquipped { get { return this.slot != SlotNumUnused; } }

        public ItemContainer.IContainer Container { get; internal set; }

        #endregion

        #region Read & Write

        public void WriteEquipProperties(PacketWriter stream)
        {
            stream.Write((byte)this.ID);
            stream.Write((ushort)this.Instance.ID);
            this.ScriptObject.WriteEquipProperties(stream);
        }

        public void ReadEquipProperties(PacketReader stream)
        {
            this.ID = stream.ReadByte();
            ushort instanceid = stream.ReadUShort();
            ItemInstance inst;
            if (!BaseVobInstance.TryGet(instanceid, out inst))
            {
                throw new Exception("Instance-ID not found! " + instanceid);
            }
            this.instance = inst;

            this.ScriptObject.ReadEquipProperties(stream);
        }

        public void WriteInventoryProperties(PacketWriter stream)
        {
            stream.Write((ushort)this.Instance.ID);
            stream.Write((ushort)this.Amount);

            this.ScriptObject.WriteInventoryProperties(stream);
        }

        public void ReadInventoryProperties(PacketReader stream)
        {
            ushort instanceid = stream.ReadUShort();
            ItemInstance inst;
            if (!BaseVobInstance.TryGet(instanceid, out inst))
            {
                throw new Exception("Instance-ID not found! " + instanceid);
            }
            this.instance = inst;
            this.amount = stream.ReadUShort();

            this.ScriptObject.ReadInventoryProperties(stream);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using GUC.WorldObjects.Definitions;
using GUC.WorldObjects.ItemContainers;
using GUC.Network;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class GUCItemInst : GUCVobInst
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.Item; } }

        #region ScriptObject

        public partial interface IScriptItem : IScriptVob
        {
            void SetAmount(int amount);

            void WriteEquipProperties(PacketWriter stream);
            void ReadEquipProperties(PacketReader stream);

            void WriteInventoryProperties(PacketWriter stream);
            void ReadInventoryProperties(PacketReader stream);
        }
        
        new public IScriptItem ScriptObject { get { return (IScriptItem)base.ScriptObject; } }

        #endregion

        #region Constructors
        
        public GUCItemInst(Model.IScriptModel scriptModel, IScriptItem scriptObject) : base(scriptModel, scriptObject)
        {
        }

        #endregion

        #region Properties

        protected override void CanChangeNow()
        {
            base.CanChangeNow();

            if (this.Container != null)
                throw new NotSupportedException("Can't change value when the Item is in an ItemContainer!");
        }

        public override Type InstanceType { get { return typeof(GUCItemDef); } }
        new public GUCItemDef Instance
        {
            get { return (GUCItemDef)base.Instance; }
            set { SetInstance(value); }
        }

        /// <summary>
        /// The upper (excluded) limit for Item amounts (ushort.MaxValue + 1).
        /// </summary>
        public const int MAX_AMOUNT = 65536;

        int amount = 1;
        public int Amount { get { return this.amount; } }

        partial void pSetAmount(int amount);
        public void SetAmount(int amount)
        {
            if (amount >= MAX_AMOUNT)
            {
                throw new Exception("Item amount is out of range! 0.." + MAX_AMOUNT);
            }

            if (amount > 0)
            {
                this.amount = amount;
                pSetAmount(amount);
            }
            else
            {
                this.amount = 0;
                if (this.IsSpawned)
                {
                    this.ScriptObject.Despawn();
                }
                else if (this.Container != null)
                {
                    this.Container.Inventory.ScriptObject.RemoveItem(this);
                }
            }
        }

        // equipment
        public const int SlotNumUnused = 255;
        internal int slot = SlotNumUnused;
        public int Slot { get { return this.slot; } }
        public bool IsEquipped { get { return this.slot != SlotNumUnused; } }

        public ItemContainer Container { get; internal set; }

        #endregion

        #region Read & Write

        public void WriteEquipProperties(PacketWriter stream)
        {
            stream.Write((byte)this.ID); 
            stream.Write((ushort)Instance.ID);
            this.ScriptObject.WriteEquipProperties(stream);
        }

        public void ReadEquipProperties(PacketReader stream)
        {
            this.ID = stream.ReadByte();

            ushort instanceID = stream.ReadUShort();
            GUCItemDef inst;
            if (!GUCBaseVobDef.TryGet(instanceID, out inst))
            {
                throw new Exception("ItemInstance-ID not found! " + instanceID);
            }
            SetInstance(inst);

            this.ScriptObject.ReadEquipProperties(stream);
        }

        public void WriteInventoryProperties(PacketWriter stream)
        {
            stream.Write((ushort)Instance.ID);
            stream.Write((ushort)this.amount);

            this.ScriptObject.WriteInventoryProperties(stream);
        }

        public void ReadInventoryProperties(PacketReader stream)
        {
            ushort instanceid = stream.ReadUShort();
            GUCItemDef inst;
            if (!GUCBaseVobDef.TryGet(instanceid, out inst))
            {
                throw new Exception("Instance-ID not found! " + instanceid);
            }
            SetInstance(inst);

            this.amount = stream.ReadUShort();

            this.ScriptObject.ReadInventoryProperties(stream);
        }

        #endregion

        /// <summary>
        /// Despawn or remove from inventory
        /// </summary>
        public void Remove()
        {
            if (this.IsSpawned)
                this.Despawn();
            else if (this.Container != null)
                this.Container.Inventory.Remove(this);
        }
    }
}

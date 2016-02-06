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

        new public IScriptItem ScriptObject { get { return (IScriptItem)base.ScriptObject; } }

        #endregion

        #region Properties

        new public ItemInstance Instance { get { return (ItemInstance)base.Instance; } }

        public byte Slot { get; internal set; }
        public ushort Amount { get; internal set; }

        //public IContainer Container { get; internal set; }

        public string Name { get { return Instance.Name; } }
        public ItemMaterials Material { get { return Instance.Material; } }
        public string Effect { get { return Instance.Effect; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public Item(IScriptItem scriptObject, ItemInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public Item(IScriptItem scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        internal void WriteEquipProperties(PacketWriter stream)
        {
            /*stream.Write(this.Instance.ID);
            this.ScriptObject.WriteEquipProperties(stream);*/
        }

        internal void ReadEquipProperties(PacketReader stream)
        {
            /*ushort instanceid = stream.ReadUShort();
            this.Instance = (ItemInstance)VobInstance.AllInstances.Get(instanceid);
            if (this.Instance == null)
                throw new Exception("Item.ReadEquipProperties failed: Instance-ID not found!");

            this.ScriptObject.ReadEquipProperties(stream);*/
        }
        
        internal void WriteInventoryProperties(PacketWriter stream)
        {
            /*stream.Write(this.ID);
            stream.Write(this.Instance.ID);
            stream.Write(this.Amount);

            this.ScriptObject.WriteInventoryProperties(stream);*/
        }

        internal void ReadInventoryProperties(PacketReader stream)
        {
            /*this.ID = stream.ReadUInt();
            ushort instanceid = stream.ReadUShort();
            this.Instance = (ItemInstance)VobInstance.AllInstances.Get(instanceid);
            if (this.Instance == null)
            {
                throw new Exception("Item.ReadInventoryProperties failed: Instance-ID not found!");
            }
            this.Amount = stream.ReadUShort();

            this.ScriptObject.ReadInventoryProperties(stream);*/
        }

        #endregion
    }
}

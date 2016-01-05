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
    public partial interface IScriptItem : IScriptVob
    {
        void WriteEquipProperties(PacketWriter stream);
        void ReadEquipProperties(PacketReader stream);

        void WriteInventoryProperties(PacketWriter stream);
        void ReadInventoryProperties(PacketReader stream);
    }

    public partial class Item : Vob
    {
        #region Fists
        public static readonly Item Fists = CreateFists();
        static Item CreateFists()
        {
            Item fists = new Item();
            fists.Instance = ItemInstance.FistInstance;
            return fists;
        }
        #endregion

        public static readonly VobDictionary Items = Vob.AllVobs.GetDict(VobTypes.Item);

        new public IScriptItem ScriptObj { get; protected set; }
        new public ItemInstance Instance { get; protected set; }

        public byte Slot { get; internal set; }
        public ushort Amount { get; internal set; }

        public string Name { get { return Instance.Name; } }
        public ItemTypes Type { get { return Instance.Type; } }
        public ItemMaterials Material { get { return Instance.Material; } }
        public string VisualChange { get { return Instance.VisualChange; } }
        public string Effect { get { return Instance.Effect; } }

        internal Item()
        {
        }
        
        internal void WriteEquipProperties(PacketWriter stream)
        {
            stream.Write(this.Instance.ID);

            this.ScriptObj.WriteEquipProperties(stream);
        }

        internal void ReadEquipProperties(PacketReader stream)
        {
            ushort instanceid = stream.ReadUShort();
            this.Instance = (ItemInstance)VobInstance.AllInstances.Get(instanceid);
            if (this.Instance == null)
            {
                throw new Exception("Item.ReadEquipProperties failed: Instance-ID not found!");
            }

            this.ScriptObj.ReadEquipProperties(stream);
        }
        
        internal void WriteInventoryProperties(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.Instance.ID);
            stream.Write(this.Amount);

            this.ScriptObj.WriteInventoryProperties(stream);
        }

        internal void ReadInventoryProperties(PacketReader stream)
        {
            this.ID = stream.ReadUInt();
            ushort instanceid = stream.ReadUShort();
            this.Instance = (ItemInstance)VobInstance.AllInstances.Get(instanceid);
            if (this.Instance == null)
            {
                throw new Exception("Item.ReadInventoryProperties failed: Instance-ID not found!");
            }
            this.Amount = stream.ReadUShort();

            this.ScriptObj.ReadInventoryProperties(stream);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    // create an inherited class for each type ?
    public enum ItemTypes : byte
    {
        Misc,
        Wep1H,
        Wep2H,
        Armor
    }

    public partial class ItemDef : VobDef, ItemInstance.IScriptItemInstance
    {
        #region Properties

        new public ItemInstance BaseDef { get { return (ItemInstance)base.BaseDef; } }

        string name = "";
        /// <summary>The standard name of this item.</summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value == null ? "" : value; }
        }

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterials Material = ItemMaterials.Wood;

        string effect = "";
        /// <summary>Magic effect when laying in the world. (case insensitive) See _work/Data/Scripts/System/VisualFX/VisualFxInst.d</summary>
        public string Effect
        {
            get { return this.effect; }
            set { this.effect = value == null ? "" : value.ToUpper(); }
        }

        string visualChange = "";
        /// <summary>For Armors</summary>
        public string VisualChange
        {
            get { return this.visualChange; }
            set { this.visualChange = value == null ? "" : value.ToUpper(); }
        }

        public ItemTypes ItemType = ItemTypes.Misc;

        #endregion

        public ItemDef() : base(new ItemInstance())
        {
        }

        public override void OnWriteProperties(PacketWriter stream)
        {
            base.OnWriteProperties(stream);
            stream.Write((byte)this.ItemType);
            stream.Write(this.name);
            stream.Write(this.visualChange);
            stream.Write((byte)this.Material);
        }

        public override void OnReadProperties(PacketReader stream)
        {
            base.OnReadProperties(stream);
            this.ItemType = (ItemTypes)stream.ReadByte();
            this.name = stream.ReadString();
            this.visualChange = stream.ReadString();
            this.Material = (ItemMaterials)stream.ReadByte();
        }
    }
}
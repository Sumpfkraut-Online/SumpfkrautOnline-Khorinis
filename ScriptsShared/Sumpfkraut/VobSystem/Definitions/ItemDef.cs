using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Network;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public enum ItemMaterials : byte
    {
        Wood = 0,
        Stone = 1,
        Metal = 2,
        Leather = 3,
        Clay = 4,
        Glass = 5
    }

    // create an inherited class for each type ?
    public enum ItemTypes : byte
    {
        Misc,
        Wep1H,
        Wep2H,
        WepBow,
        WepXBow,
        MAXWEAPON,

        Armor,
        AmmoBow,
        AmmoXBow,
        Drinkable,
        SmallEatable,
        LargeEatable,
        Rice,
        Mutton,
        Readable,
        Torch,
        Joint,
        Slutable,

        Rune,
        Scroll,
    }

    public partial class ItemDef : NamedVobDef, ItemInstance.IScriptItemInstance
    {
        #region Properties

        new public ItemInstance BaseDef { get { return (ItemInstance)base.BaseDef; } }

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterials Material = ItemMaterials.Wood;

        string effect = "";
        /// <summary>Magic effect when laying in the world. (case insensitive) See _work/Data/Scripts/System/VisualFX/VisualFxInst.d</summary>
        public string Effect
        {
            get { return this.effect; }
            set { this.effect = value == null ? "" : value.ToUpperInvariant(); }
        }

        string visualChange = "";
        /// <summary>For Armors</summary>
        public string VisualChange
        {
            get { return this.visualChange; }
            set { this.visualChange = value == null ? "" : value.ToUpperInvariant(); }
        }

        public ItemTypes ItemType = ItemTypes.Misc;

        public bool IsWeapon { get { return this.ItemType >= ItemTypes.Wep1H && this.ItemType <= ItemTypes.WepXBow; } }
        public bool IsWepRanged { get { return this.ItemType >= ItemTypes.WepBow && this.ItemType <= ItemTypes.WepXBow; } }
        public bool IsWepMelee { get { return this.ItemType >= ItemTypes.Wep1H && this.ItemType <= ItemTypes.Wep2H; } }

        #endregion

        #region Constructors

        protected override BaseVobInstance CreateVobInstance()
        {
            return new ItemInstance(this);
        }

        partial void pConstruct();
        public ItemDef()
        {
            effectHandler = effectHandler ?? new EffectSystem.EffectHandlers.ItemEffectHandler(null, this);
            pConstruct();
        }

        #endregion

        #region Read & Write

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

        #endregion
    }
}
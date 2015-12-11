using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects.Instances
{
    public class ItemInstance : VobInstance
    {
        public static readonly ItemInstance FistInstance = CreateFists();
        static ItemInstance CreateFists()
        {
            ItemInstance fists = new ItemInstance();
            fists.Name = "Fäustedummy";
            return fists;
        }

        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.Item;

        #region Properties

        public const int TextAndCountLen = 6;

        /// <summary>The standard name of this item.</summary>
        public String Name = "";

        /// <summary>The type of this item.</summary>
        public ItemType Type = ItemType.Misc;

        /// <summary>The material of this item. Controls the dropping sound.</summary>
        public ItemMaterial Material = ItemMaterial.Wood;

        /// <summary>Text lines shown on the left side of the description box in the inventory.</summary>
        public readonly string[] Text = new string[TextAndCountLen] { "", "", "", "", "", "" };
        /// <summary>Values shown on the right side of the description box in the inventory.</summary>
        public readonly ushort[] Count = new ushort[TextAndCountLen] { 0, 0, 0, 0, 0, 0 };

        /// <summary>The name shown in the description box in the inventory. If it's the same as 'Name', just leave this empty.</summary>
        public string Description = "";

        string visualChange = "";
        /// <summary>The ASC-Mesh for armors which is put over the NPC's character model.</summary>
        public String VisualChange
        {
            get { return visualChange; }
            set { visualChange = value.Trim().ToUpper(); }
        }

        string effect = "";
        /// <summary>Magic Effect. See Scripts/System/VisualFX/VisualFxInst.d</summary>
        public String Effect
        {
            get { return effect; }
            set { effect = value.Trim().ToUpper(); }
        }

        /// <summary>The ItemInstance which is used for ammunition.</summary>
        //public ItemInstance Munition = null;

        public int MainFlags;
        public int Flags;
        public int Wear = 0;

        public bool IsMeleeWeapon { get { return MainFlags == oCItem.MainFlags.ITEM_KAT_NF; } }
        public bool IsRangedWeapon { get { return MainFlags == oCItem.MainFlags.ITEM_KAT_FF; } }
        public bool IsArmor { get { return MainFlags == oCItem.MainFlags.ITEM_KAT_ARMOR; } }

        #endregion


        public ItemInstance()
        {
            this.VobType = sVobType;
        }

        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Name = stream.ReadString();
            this.Type = (ItemType)stream.ReadByte();
            this.Material = (ItemMaterial)stream.ReadByte();
            for (int i = 0; i < TextAndCountLen; i++)
            {
                this.Text[i] = stream.ReadString();
                this.Count[i] = stream.ReadUShort();
            }
            this.Description = stream.ReadString();
            this.VisualChange = stream.ReadString();
            this.Effect = stream.ReadString();
            //this.Munition 

            //...

            SetFlags();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(Name);
            stream.Write((byte)Type);
            stream.Write((byte)Material);
            for (int i = 0; i < TextAndCountLen; i++)
            {
                stream.Write(Text[i]);
                stream.Write(Count[i]);
            }
            stream.Write(Description);
            stream.Write(VisualChange);
            stream.Write(Effect);
            //stream.Write(Munition == null ? ushort.MinValue : Munition.ID);

            //...
        }

        void SetFlags()
        {
            switch (Type)
            {
                case ItemType.Sword_1H:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                    Flags = oCItem.ItemFlags.ITEM_SWD;
                    break;
                case ItemType.Sword_2H:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                    Flags = oCItem.ItemFlags.ITEM_2HD_SWD;
                    break;
                case ItemType.Blunt_1H:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                    Flags = oCItem.ItemFlags.ITEM_AXE;
                    break;
                case ItemType.Blunt_2H:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                    Flags = oCItem.ItemFlags.ITEM_2HD_AXE;
                    break;
                case ItemType.Bow:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_FF;
                    Flags = oCItem.ItemFlags.ITEM_BOW;
                    break;
                case ItemType.XBow:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_FF;
                    Flags = oCItem.ItemFlags.ITEM_CROSSBOW;
                    break;
                case ItemType.Armor:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_ARMOR;
                    Flags = 0;
                    Wear = 1;
                    break;
                case ItemType.Arrow:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_MUN;
                    Flags = oCItem.ItemFlags.ITEM_BOW;
                    break;
                case ItemType.XBolt:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_MUN;
                    Flags = oCItem.ItemFlags.ITEM_CROSSBOW;
                    break;
                case ItemType.Ring:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                    Flags = oCItem.ItemFlags.ITEM_RING;
                    break;
                case ItemType.Amulet:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                    Flags = oCItem.ItemFlags.ITEM_AMULET;
                    break;
                case ItemType.Belt:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                    Flags = oCItem.ItemFlags.ITEM_BELT;
                    break;
                case ItemType.Food_Small:
                case ItemType.Food_Huge:
                case ItemType.Drink:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_FOOD;
                    Flags = 0;
                    break;
                case ItemType.Potions:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_POTIONS;
                    Flags = 0;
                    break;
                case ItemType.Document:
                case ItemType.Book:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_DOCS;
                    Flags = 0;
                    break;
                case ItemType.Rune:
                case ItemType.Scroll:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_RUNE;
                    Flags = 0;
                    break;
                case ItemType.Misc:
                case ItemType.Misc_Usable:
                default:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_NONE;
                    Flags = 0;
                    break;
            }

            Flags |= MainFlags;
        }
    }
}
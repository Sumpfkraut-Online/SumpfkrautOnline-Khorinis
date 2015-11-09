using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using Gothic.zTypes;
using Gothic.zClasses;
using System.IO;


namespace GUC.Client.WorldObjects
{
    class ItemInstance : AbstractInstance
    {
        public static InstanceManager<ItemInstance> Table = new InstanceManager<ItemInstance>("items.pak");

        public String name;
        public ushort weight;

        public ushort condition;

        public Gender gender;

        public ItemType type;
        public ItemMaterial material;

        public String description;
        public String[] text;
        public ushort[] count;
        //Visuals:
        public String visual;
        public String visualChange;
        public String effect;

        public ushort munition;

        public oCItem.MainFlags mainFlags;
        public oCItem.ItemFlags flags;
        public int wear = 0;

        public bool IsMeleeWeapon { get { return mainFlags == oCItem.MainFlags.ITEM_KAT_NF; } }
        public bool IsRangedWeapon { get { return mainFlags == oCItem.MainFlags.ITEM_KAT_FF; } }
        public bool IsArmor { get { return mainFlags == oCItem.MainFlags.ITEM_KAT_ARMOR; } }

        internal override void Read(BinaryReader br)
        {
            ID = br.ReadUInt16();
            name = br.ReadString();
            weight = br.ReadUInt16();
            type = (ItemType)br.ReadByte();
            material = (ItemMaterial)br.ReadByte();
            text = new string[4];
            count = new ushort[4];
            for (int i = 0; i < 4; i++)
            {
                text[i] = br.ReadString();
                count[i] = br.ReadUInt16();
            }
            description = br.ReadString();
            visual = br.ReadString();
            visualChange = br.ReadString();
            effect = br.ReadString();
            munition = br.ReadUInt16();
            gender = (Gender)br.ReadByte();
            condition = br.ReadUInt16();

            SetFlags();
        }

        internal override void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(name);
            bw.Write(weight);
            bw.Write((byte)type);
            bw.Write((byte)material);
            for (int i = 0; i < 4; i++)
            {
                bw.Write(text[i]);
                bw.Write(count[i]);
            }
            bw.Write(description);
            bw.Write(visual);
            bw.Write(visualChange);
            bw.Write(effect);
            bw.Write(munition);
            bw.Write((byte)gender);
            bw.Write(condition);
        }

        void SetFlags()
        {
            switch (type)
            {
                case ItemType.Sword_1H:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                    flags = oCItem.ItemFlags.ITEM_SWD;
                    break;
                case ItemType.Sword_2H:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                    flags = oCItem.ItemFlags.ITEM_2HD_SWD;
                    break;
                case ItemType.Blunt_1H:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                    flags = oCItem.ItemFlags.ITEM_AXE;
                    break;
                case ItemType.Blunt_2H:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                    flags = oCItem.ItemFlags.ITEM_2HD_AXE;
                    break;
                case ItemType.Bow:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_FF;
                    flags = oCItem.ItemFlags.ITEM_BOW;
                    break;
                case ItemType.XBow:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_FF;
                    flags = oCItem.ItemFlags.ITEM_CROSSBOW;
                    break;
                case ItemType.Armor:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_ARMOR;
                    flags = 0;
                    wear = 1;
                    break;
                case ItemType.Arrow:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_MUN;
                    flags = oCItem.ItemFlags.ITEM_BOW;
                    break;
                case ItemType.XBolt:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_MUN;
                    flags = oCItem.ItemFlags.ITEM_CROSSBOW;
                    break;
                case ItemType.Ring:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                    flags = oCItem.ItemFlags.ITEM_RING;
                    break;
                case ItemType.Amulet:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                    flags = oCItem.ItemFlags.ITEM_AMULET;
                    break;
                case ItemType.Belt:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                    flags = oCItem.ItemFlags.ITEM_BELT;
                    break;
                case ItemType.Food_Small:
                case ItemType.Food_Huge:
                case ItemType.Drink:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_FOOD;
                    flags = 0;
                    break;
                case ItemType.Potions:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_POTIONS;
                    flags = 0;
                    break;
                case ItemType.Document:
                case ItemType.Book:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_DOCS;
                    flags = 0;
                    break;
                case ItemType.Rune:
                case ItemType.Scroll:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_RUNE;
                    flags = 0;
                    break;
                case ItemType.Misc:
                case ItemType.Misc_Usable:
                default:
                    mainFlags = oCItem.MainFlags.ITEM_KAT_NONE;
                    flags = 0;
                    break;
            }
        }
    }
}

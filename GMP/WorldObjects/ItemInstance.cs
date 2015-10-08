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
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Client.WorldObjects
{
    class ItemInstance : IDisposable
    {
        public static Dictionary<ushort, ItemInstance> InstanceList;

        public ushort ID;

        public zString Name;
        public int Range;
        public int Weight;

        public ItemType Type;
        public ItemMaterial Material;

        public zString Description;
        public zString[] Text;
        public int[] Count;
        //Visuals:
        public zString Visual;
        public zString Visual_Change;
        public zString Effect;

        public int Munition;

        public oCItem.MainFlags MainFlags;
        public oCItem.ItemFlags Flags;
        public int Wear = 0;


        public oCItem CreateItem()
        {
            return CreateItem(oCItem.Create(Program.Process));
        }

        public oCItem CreateItem(oCItem item)
        {
            item.Instanz = ID;
            item.Name.Set(Name);

            item.Range = Range;
            item.Material = (int)Material;

            item.Description.Set(Description);
            for (int i = 0; i < 6; i++)
            {
                item.Text[i].Set(Text[i]);
                item.Count[i] = Count[i];
            }
            item.SetVisual(Visual);
            item.VisualChange.Set(Visual_Change);
            item.Effect.Set(Effect);

            item.Munition = Munition;
            item.MainFlag = (int)MainFlags;
            item.Flags = (int)Flags;
            item.Wear = Wear;

            return item;
        }

        static string FileName = States.StartupState.getDaedalusPath() + "Data2.pak";

        public static byte[] ReadFile()
        {
            try
            {
                if (File.Exists(FileName))
                {
                    byte[] data = File.ReadAllBytes(FileName);
                    ReadData(data);

                    byte[] hash;
                    using (MD5 md5 = new MD5CryptoServiceProvider())
                    {
                        md5.TransformFinalBlock(data, 0, data.Length);
                        hash = md5.Hash;
                    }

                    return hash;
                }
            }
            catch { }
            return new byte[16];
        }

        public static void ReadData(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;

                using (GZipStream gz = new GZipStream(ms, CompressionMode.Decompress))
                using (BinaryReader br = new BinaryReader(gz, Encoding.UTF8))
                {
                    // dispose old instances
                    if (InstanceList != null)
                    for (int i = 0; i < InstanceList.Count; i++)
                    {
                        InstanceList.ElementAt(i).Value.Dispose();
                    }

                    // read new instances
                    ItemInstance inst;
                    ushort num = br.ReadUInt16();
                    InstanceList = new Dictionary<ushort, ItemInstance>(num);
                    for (int i = 0; i < num; i++)
                    {
                        inst = new ItemInstance();

                        inst.ID = br.ReadUInt16();
                        inst.Name = zString.Create(Program.Process, br.ReadString());
                        inst.Range = br.ReadUInt16();
                        inst.Weight = br.ReadUInt16();
                        inst.Type = (ItemType)br.ReadByte();
                        inst.Material = (ItemMaterial)br.ReadByte();
                        inst.Description = zString.Create(Program.Process, br.ReadString());
                        inst.Text = new zString[6];
                        inst.Count = new int[6];
                        for (int l = 0; l < 6; l++)
                        {
                            inst.Text[l] = zString.Create(Program.Process, br.ReadString());
                            inst.Count[l] = br.ReadUInt16();
                        }
                        inst.Visual = zString.Create(Program.Process, br.ReadString());
                        inst.Visual_Change = zString.Create(Program.Process, br.ReadString());
                        inst.Effect = zString.Create(Program.Process, br.ReadString());
                        inst.Munition = br.ReadUInt16();

                        inst.SetFlags();

                        InstanceList.Add(inst.ID, inst);
                    }
                }
            }
        }

        public static void WriteFile()
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
            using (BinaryWriter bw = new BinaryWriter(gz, Encoding.UTF8))
            {
                bw.Write((ushort)InstanceList.Count);
                //ordered by IDs
                foreach (ItemInstance inst in InstanceList.Values.OrderBy(n => n.ID))
                {
                    bw.Write(inst.ID);
                    bw.Write(inst.Name.Value);
                    bw.Write((ushort)inst.Range);
                    bw.Write((ushort)inst.Weight);
                    bw.Write((byte)inst.Type);
                    bw.Write((byte)inst.Material);
                    bw.Write(inst.Description.Value);
                    for (int i = 0; i < 6; i++)
                    {
                        bw.Write(inst.Text[i].Value);
                        bw.Write((ushort)inst.Count[i]);
                    }
                    bw.Write(inst.Visual.Value);
                    bw.Write(inst.Visual_Change.Value);
                    bw.Write(inst.Effect.Value);
                    bw.Write((ushort)inst.Munition);
                }
            }
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
                case ItemType.Arrow:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_MUN;
                    Flags = oCItem.ItemFlags.ITEM_BOW;
                    break;
                case ItemType.XBolt:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_MUN;
                    Flags = oCItem.ItemFlags.ITEM_CROSSBOW;
                    break;
                case ItemType.Armor:
                    MainFlags = oCItem.MainFlags.ITEM_KAT_ARMOR;
                    Flags = 0;
                    Wear = 1;
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
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Name.Dispose();
                Description.Dispose();
                for (int i = 0; i < 6; i++)
                {
                    Text[i].Dispose();
                }
                Visual.Dispose();
                Visual_Change.Dispose();
                Effect.Dispose();
                disposed = true;
            }
        }
    }
}

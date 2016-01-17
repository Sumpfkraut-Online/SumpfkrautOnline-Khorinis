using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class oCItem : zCVob
    {
        new public class VarOffsets : zCVob.VarOffsets
        {
            public const int name = 0x0124,
            nameID = 0x0138,
            hp = 0x14C,
            hp_max = 0x150,
            mainflag = 0x154,
            flags = 0x0158,
            weigth = 0x15C,
            value = 0x160,
            damageType = 0x164,
            damageTotal = 0x168,
            damage = 0x16C,//int[8]
            wear = 0x18C,
            protection = 0x190,
            nutrition = 0x1B0,
            cond_atr = 0x1B4,//int[3]
            cond_value = 0x1C0,//int[3]
            change_atr = 0x1CC,//int[3]
            change_value = 0x1D8,//int[3]
            magic = 0x1E4,
            on_equip = 0x1E8,
            on_unequip = 0x1EC,
            on_state = 0x1F0,//int[4]
            owner = 0x200,//npc*
            ownerGuild = 0x204,
            disguiseGuild = 0x208,
            itemvisual = 0x20C,//string
            visual_change = 0x220,//string
            visual_effect = 0x234,//string
            visual_skin = 0x248,//int
            scemeName = 0x24C, //string
            material = 0x260,
            munition = 0x264,
            spell = 0x268,
            range = 0x26C,
            mag_circle = 0x270,
            description = 0x274,
            text = 0x288,//string[6]
            count = 0x300,//int[6]
            inv_zbias = 0x318,
            inv_rotx = 0x31C,
            inv_roty = 0x320,
            inv_rotz = 0x324,
            inv_animate = 0x328,
            amount = 0x32C,
            instance = 0x330,
            c_manipulation = 0x334,
            last_manipulation = 0x338,
            magic_value = 0x33C,
            effectVob = 0x340,
            next = 0x344;
        }


        new public abstract class FuncAddresses : zCVob.FuncAddresses
        {
            public const int MultiSlot = 0x007125A0,
            ThisVobAddedToWorld = 0x00712DF0,
            CreateVisual = 0x00711930,
            RenderItem = 0x00713AC0;
        }

        public abstract class MainFlags
        {
            // categories (mainflag)
            public const int ITEM_KAT_NONE = 1 << 0;  // Sonstiges
            public const int ITEM_KAT_NF = 1 << 1;  // Nahkampfwaffen
            public const int ITEM_KAT_FF = 1 << 2;  // Fernkampfwaffen
            public const int ITEM_KAT_MUN = 1 << 3; // Munition (MULTI)
            public const int ITEM_KAT_ARMOR = 1 << 4;  // Ruestungen
            public const int ITEM_KAT_FOOD = 1 << 5;  // Nahrungsmittel (MULTI)
            public const int ITEM_KAT_DOCS = 1 << 6;  // Dokumente
            public const int ITEM_KAT_POTIONS = 1 << 7;  // Traenke
            public const int ITEM_KAT_LIGHT = 1 << 8;  // Lichtquellen
            public const int ITEM_KAT_RUNE = 1 << 9;  // Runen/Scrolls
            public const int ITEM_KAT_MAGIC = 1 << 31;  // Ringe/Amulette/Guertel
            public const int ITEM_KAT_KEYS = ITEM_KAT_NONE;
        }

        public abstract class ItemFlags
        {
            // weapon type (flags)
            public const int ITEM_DAG = 1 << 13;  // (OBSOLETE!)
            public const int ITEM_SWD = 1 << 14;  // Schwert
            public const int ITEM_AXE = 1 << 15;  // Axt
            public const int ITEM_2HD_SWD = 1 << 16;  // Zweihaender
            public const int ITEM_2HD_AXE = 1 << 17;  // Streitaxt
            public const int ITEM_SHIELD = 1 << 18;  // (OBSOLETE!)
            public const int ITEM_BOW = 1 << 19;  // Bogen
            public const int ITEM_CROSSBOW = 1 << 20;  // Armbrust
            // magic type (flags)
            public const int ITEM_RING = 1 << 11;  // Ring
            public const int ITEM_AMULET = 1 << 22;  // Amulett
            public const int ITEM_BELT = 1 << 24;  // Guertel
            // attributes (flags)
            public const int ITEM_DROPPED = 1 << 10;  // (INTERNAL!)
            public const int ITEM_MISSION = 1 << 12;  // Missionsgegenstand
            public const int ITEM_MULTI = 1 << 21;  // Stapelbar
            public const int ITEM_NFOCUS = 1 << 23;  // (INTERNAL!)
            public const int ITEM_CREATEAMMO = 1 << 25;  // Erzeugt Munition selbst (magisch)
            public const int ITEM_NSPLIT = 1 << 26;  // Kein Split-Item (Waffe als Interact-Item!)
            public const int ITEM_DRINK = 1 << 27;  // (OBSOLETE!)
            public const int ITEM_TORCH = 1 << 28;  // Fackel
            public const int ITEM_THROW = 1 << 29;  // (OBSOLETE!)
            public const int ITEM_ACTIVE = 1 << 30;  // (INTERNAL!)
        }

        public oCItem(int address)
            : base(address)
        {

        }

        public oCItem()
        {

        }
        
        public zString[] Text
        {
            get
            {
                return new zString[]{
                    new zString(Address+VarOffsets.text),
                    new zString(Address+VarOffsets.text + 20),
                    new zString(Address+VarOffsets.text + 20*2),
                    new zString(Address+VarOffsets.text + 20*3),
                    new zString(Address+VarOffsets.text + 20*4),
                    new zString(Address+VarOffsets.text + 20*5)
                };
            }
        }

        public int[] Count
        {
            get
            {
                return new int[]{
                    Process.ReadInt(Address+VarOffsets.count),
                    Process.ReadInt(Address+VarOffsets.count + 4),
                    Process.ReadInt(Address+VarOffsets.count + 4*2),
                    Process.ReadInt(Address+VarOffsets.count + 4*3),
                    Process.ReadInt(Address+VarOffsets.count + 4*4),
                    Process.ReadInt(Address+VarOffsets.count + 4*5)
                };
            }
        }

        public void setCount(int id, int value)
        {
            if (id >= 6)
                throw new ArgumentException("id => 0-5");
            Process.Write(value, Address + VarOffsets.count + 4 * id);
        }

        public void setText(int id, String value)
        {
            if (id >= 6)
                throw new ArgumentException("id => 0-5");
            Text[id].Set(value);
        }

        public void setDamage(int id, int value)
        {
            if (id >= 8)
                throw new ArgumentException("id => 0-7");
            Process.Write(value, Address + VarOffsets.damage + 4 * id);
        }

        public void setProtection(int id, int value)
        {
            if (id >= 8)
                throw new ArgumentException("id => 0-7");
            Process.Write(value, Address + VarOffsets.protection + 4 * id);
        }

        public void setConditionalAttribute(int id, int value)
        {
            if (id >= 3)
                throw new ArgumentException("id => 0-2");
            Process.Write(value, Address + VarOffsets.cond_atr + 4 * id);
        }

        public void setConditionalValue(int id, int value)
        {
            if (id >= 3)
                throw new ArgumentException("id => 0-2");
            Process.Write(value, Address + VarOffsets.cond_value + 4 * id);
        }

        public void CreateVisual()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CreateVisual);
        }

        public void ThisVobAddedToWorld(zCWorld slot)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ThisVobAddedToWorld, slot);
        }

        public int MultiSlot()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.MultiSlot).Value;
        }

        public zString Name
        {
            get { return new zString(Address + VarOffsets.name); }
        }
        public zString NameID
        {
            get { return new zString(Address + VarOffsets.nameID); }
        }

        public zString Description
        {
            get { return new zString(Address + VarOffsets.description); }
        }

        public zString ScemeName
        {
            get { return new zString(Address + VarOffsets.scemeName); }
        }

        public zString ItemVisual
        {
            get { return new zString(Address + VarOffsets.itemvisual); }
        }

        public zString VisualChange
        {
            get { return new zString(Address + VarOffsets.visual_change); }
        }

        public zString Effect
        {
            get { return new zString(Address + VarOffsets.visual_effect); }
        }

        public int VisualSkin
        {
            get { return Process.ReadInt(Address + VarOffsets.visual_skin); }
            set { Process.Write(value, Address + VarOffsets.visual_skin); }
        }

        public int HP
        {
            get { return Process.ReadInt(Address + VarOffsets.hp); }
            set { Process.Write(value, Address + VarOffsets.hp); }
        }

        public int HPMax
        {
            get { return Process.ReadInt(Address + VarOffsets.hp_max); }
            set { Process.Write(value, Address + VarOffsets.hp_max); }
        }

        public int Wear
        {
            get { return Process.ReadInt(Address + VarOffsets.wear); }
            set { Process.Write(value, Address + VarOffsets.wear); }
        }

        public int Material
        {
            get { return Process.ReadInt(Address + VarOffsets.material); }
            set { Process.Write(value, Address + VarOffsets.material); }
        }

        public int Munition
        {
            get { return Process.ReadInt(Address + VarOffsets.munition); }
            set { Process.Write(value, Address + VarOffsets.munition); }
        }

        public int DamageType
        {
            get { return Process.ReadInt(Address + VarOffsets.damageType); }
            set { Process.Write(value, Address + VarOffsets.damageType); }
        }

        public int DamageTotal
        {
            get { return Process.ReadInt(Address + VarOffsets.damageTotal); }
            set { Process.Write(value, Address + VarOffsets.damageTotal); }
        }

        public int Value
        {
            get { return Process.ReadInt(Address + VarOffsets.value); }
            set { Process.Write(value, Address + VarOffsets.value); }
        }

        public int Amount
        {
            get { return Process.ReadInt(Address + VarOffsets.amount); }
            set { Process.Write(value, Address + VarOffsets.amount); }
        }

        public int MainFlag
        {
            get { return Process.ReadInt(Address + VarOffsets.mainflag); }
            set { Process.Write(value, Address + VarOffsets.mainflag); }
        }

        public int Flags
        {
            get { return Process.ReadInt(Address + VarOffsets.flags); }
            set { Process.Write(value, Address + VarOffsets.flags); }
        }

        public int Range
        {
            get { return Process.ReadInt(Address + VarOffsets.range); }
            set { Process.Write(value, Address + VarOffsets.range); }
        }

        public int CondAtr1
        {
            get { return Process.ReadInt(Address + VarOffsets.cond_atr); }
            set { Process.Write(value, Address + VarOffsets.cond_atr); }
        }

        public int CondAtr2
        {
            get { return Process.ReadInt(Address + VarOffsets.cond_atr + 4); }
            set { Process.Write(value, Address + VarOffsets.cond_atr + 4); }
        }

        public int CondAtr3
        {
            get { return Process.ReadInt(Address + VarOffsets.cond_atr + 8); }
            set { Process.Write(value, Address + VarOffsets.cond_atr + 8); }
        }

        public int CondValue1
        {
            get { return Process.ReadInt(Address + VarOffsets.cond_value); }
            set { Process.Write(value, Address + VarOffsets.cond_value); }
        }

        public int CondValue2
        {
            get { return Process.ReadInt(Address + VarOffsets.cond_value + 4); }
            set { Process.Write(value, Address + VarOffsets.cond_value + 4); }
        }

        public int CondValue3
        {
            get { return Process.ReadInt(Address + VarOffsets.cond_value + 8); }
            set { Process.Write(value, Address + VarOffsets.cond_value + 8); }
        }




        public int[] Damage
        {
            get
            {
                return new int[] { 
                    Process.ReadInt(Address + VarOffsets.damage),
                    Process.ReadInt(Address + VarOffsets.damage + 4),
                    Process.ReadInt(Address + VarOffsets.damage + 4*2),
                    Process.ReadInt(Address + VarOffsets.damage + 4*3),
                    Process.ReadInt(Address + VarOffsets.damage + 4*4),
                    Process.ReadInt(Address + VarOffsets.damage + 4*5),
                    Process.ReadInt(Address + VarOffsets.damage + 4*6),
                    Process.ReadInt(Address + VarOffsets.damage + 4*7),
                };
            }
        }

        public int[] Protection
        {
            get
            {
                return new int[] { 
                    Process.ReadInt(Address + VarOffsets.protection),
                    Process.ReadInt(Address + VarOffsets.protection + 4),
                    Process.ReadInt(Address + VarOffsets.protection + 4*2),
                    Process.ReadInt(Address + VarOffsets.protection + 4*3),
                    Process.ReadInt(Address + VarOffsets.protection + 4*4),
                    Process.ReadInt(Address + VarOffsets.protection + 4*5),
                    Process.ReadInt(Address + VarOffsets.protection + 4*6),
                    Process.ReadInt(Address + VarOffsets.protection + 4*7),
                };
            }
        }

        public int Spell
        {
            get { return Process.ReadInt(Address + VarOffsets.spell); }
            set { Process.Write(value, Address + VarOffsets.spell); }
        }

        public int Instanz
        {
            get { return Process.ReadInt(Address + VarOffsets.instance); }
            set { Process.Write(value, Address + VarOffsets.instance); }
        }

        public void RenderItem(zCWorld world, View.zCView view, float x)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RenderItem, world, view, new FloatArg(x));
        }

        new public const int ByteSize = 0x348;

        new public static oCItem Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x714440); //_CreateInstance()
            Process.THISCALL<NullReturnCall>(address, 0x00711290); //Konstruktor...
            return new oCItem(address);
        }

        /*public static oCItem Create()
        {
            oCItem r = null;

            IntPtr ptr = Process.Alloc(0x348);
            zCClassDef.ObjectCreated(Process, ptr.ToInt32(), 0x00AB1168);
            Process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (int)0x00711290, new CallValue[] { });

            r = new oCItem(Process, ptr.ToInt32());

            return r;
        }

        public static zCClassDef getClassDef(Process process)
        {
            return new zCClassDef(process, 0x00AB1168);
        }*/
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using Gothic.zTypes;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    class ItemInstance
    {
        private static Dictionary<uint, ItemInstance> instanceDict = new Dictionary<uint, ItemInstance>();
        public static Dictionary<uint, ItemInstance> InstanceDict { get { return instanceDict; } }

        private uint id;
        private string name, scemeName, description, visual, visual_change, effect;
        private ushort range = 0;
        private ushort weight = 0;
        private MainFlags mainFlags = 0;
        private Flags flags = 0;
        private ArmorFlags wear = 0;
        private MaterialType material = 0;
        private string[] text = new string[6];
        private ushort[] count = new ushort[6];
        private byte visual_skin = 0;

        public uint ID { get { return id; } }

        public string Name { get { return name; } }
        public string ScemeName { get { return scemeName; } }

        public ushort Range { get { return range; } }

        public ushort Weight { get { return weight; } }

        public MainFlags MainFlags { get { return mainFlags; } }
        public Flags Flags { get { return flags; } }
        public ArmorFlags Wear { get { return wear; } }

        public MaterialType Material { get { return material; } }

        public string Description { get { return description; } }
        public string[] Text { get { return text; } }
        public ushort[] Count { get { return count; } }

        //Visuals:
        public string Visual { get { return visual; } }
        public string Visual_Change { get {return visual_change; } }
        public string Effect { get { return effect; } }

        public byte Visual_Skin { get { return visual_skin;} }

        public ItemInstance Munition { get; private set; }

        public bool IsKeyInstance { get; private set; }
        public bool IsTorch { get; private set; }
        public bool IsTorchBurned { get; private set; }
        public bool IsTorchBurning { get; private set; }
        public bool IsGold { get; private set; }

        //public Spell Spell = null;

        private ItemInstance()
        {
        }

        public oCItem CreateItem()
        {
            oCItem gItem = oCItem.Create(Program.Process);

            gItem.Instanz = (int)this.id;

            if (name != null) gItem.Name.Set(name);
            if (scemeName != null) gItem.ScemeName.Set(scemeName);
            if (effect != null) gItem.Effect.Set(effect);
            if (visual != null) gItem.Visual.Set(visual);
            if (visual_change != null) gItem.VisualChange.Set(visual_change);
            if (description != null) gItem.Description.Set(description);

            gItem.Range = range;
            gItem.MainFlag = (int)mainFlags;
            gItem.Flags = (int)flags | (int)mainFlags;
            gItem.Wear = (int)wear;
            gItem.Material = (int)material;
            gItem.VisualSkin = visual_skin;
            
            for (int i = 0; i < 6; i++)
            {
                if (text[i] != null) gItem.Text[i].Set(text[i]);
                gItem.Count[i] = count[i];
            }
            gItem.Munition = 9999;

            return gItem;
        }

        public static void ReadNew(BitStream stream)
        {
            ItemInstance ii = new ItemInstance();

            ii.id = stream.mReadUInt();

            if (stream.ReadBit())
                ii.name = stream.mReadString();

            if (stream.ReadBit())
                ii.scemeName = stream.mReadString();

            if (stream.ReadBit())
                ii.range = stream.mReadUShort();

            if (stream.ReadBit())
                ii.weight = stream.mReadUShort();

            if (stream.ReadBit())
                ii.mainFlags = (MainFlags)stream.mReadInt();

            if (stream.ReadBit())
                ii.flags = (Flags)stream.mReadInt();

            if (stream.ReadBit())
                ii.wear = (ArmorFlags)stream.mReadInt();

            if (stream.ReadBit())
                ii.material = (MaterialType)stream.mReadByte();

            if (stream.ReadBit())
                ii.description = stream.mReadString();

            for (int i = 0; i < 6; i++ )
            {
                if (stream.ReadBit())
                    ii.text[i] = stream.mReadString();
            }

            for (int i = 0; i < 6; i++)
            {
                if (stream.ReadBit())
                    ii.count[i] = stream.mReadUShort();
            }

            if (stream.ReadBit())
                ii.visual = stream.mReadString();

            if (stream.ReadBit())
                ii.visual_change = stream.mReadString();

            if (stream.ReadBit())
                ii.effect = stream.mReadString();

            if (stream.ReadBit())
                ii.visual_skin = stream.mReadByte();

            if (stream.ReadBit())
            {
                ushort muni = stream.mReadUShort();
                //Munition
            }

            ii.IsKeyInstance = stream.ReadBit();
            ii.IsTorch = stream.ReadBit();
            ii.IsTorchBurned = stream.ReadBit();
            ii.IsTorchBurning = stream.ReadBit();
            ii.IsGold = stream.ReadBit();

            instanceDict.Add(ii.ID, ii);
        }
    }
}

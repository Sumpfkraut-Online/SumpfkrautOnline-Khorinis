using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Network;
using RakNet;
using GUC.Network;

namespace GUC.Server.WorldObjects
{
    public abstract class ItemInstance
    {
        protected static List<ItemInstance> instanceList = new List<ItemInstance>();
        protected static Dictionary<string, ItemInstance> instanceDict = new Dictionary<string, ItemInstance>();
        public static List<ItemInstance> InstanceList { get { return instanceList; } }
        public static Dictionary<string, ItemInstance> InstanceDict { get { return instanceDict; } }

        public uint ID { get; protected set; }

        public String Name { get; protected set; }
        public String ScemeName { get; protected set; }

        public ushort[] Protection { get; protected set; }

        public DamageTypes DamageType { get; protected set; }
        public ushort TotalDamage { get; protected set; }
        public ushort[] Damages { get; protected set; }
        public ushort Range { get; protected set; }

        public ushort[] ConditionAttributes { get; protected set; }
        public ushort[] ConditionValues { get; protected set; }

        public ushort Weight { get; protected set; }

        public MainFlags MainFlags { get; protected set; }
        public Flags Flags { get; protected set; }
        public ArmorFlags Wear { get; protected set; }

        public MaterialType Materials { get; protected set; }

        public String Description { get; protected set; }
        public String[] Text { get; protected set; }
        public ushort[] Count { get; protected set; }

        //Visuals:
        public String Visual { get; protected set; }
        public String Visual_Change { get; protected set; }
        public String Effect { get; protected set; }

        public byte Visual_Skin { get; protected set; }
        public ItemInstance Munition { get; protected set; }

        public bool IsKeyInstance { get; protected set; }
        public bool IsTorch { get; protected set; }
        public bool IsTorchBurned { get; protected set; }
        public bool IsTorchBurning { get; protected set; }
        public bool IsGold { get; protected set; }

        //public Spell Spell = null;

        protected delegate void OnEquipHandler(NPC npc, Item item);
        protected event OnEquipHandler OnEquip = null;
        protected event OnEquipHandler OnUnequip = null;

        protected ItemInstance()
        {
            Name = "";
            ScemeName = "";
            Protection = new ushort[8];
            DamageType = DamageTypes.DAM_INVALID;
            TotalDamage = 0;
            Damages = new ushort[8];
            Range = 0;

            ConditionAttributes = new ushort[3];
            ConditionValues = new ushort[3];

            Weight = 0;
            MainFlags = 0;
            Flags = 0;
            Wear = 0;
            Materials = 0;

            Description = "";
            Text = new String[6] { "", "", "", "", "", "" };
            Count = new ushort[6];

            Visual = "";
            Visual_Change = "";
            Effect = "";
            Visual_Skin = 0;

            Munition = null;

            IsGold = false;
            IsKeyInstance = false;
            IsTorch = false;
            IsTorchBurned = false;
            IsTorchBurning = false;
        }

        public static void AddInstance(ItemInstance inst)
        {
            string codename = inst.GetType().Name.ToUpper();
            if (instanceDict.ContainsKey(codename))
            {
                throw new Exception("Iteminstance " + codename + " is already existing.");
            }
            inst.ID = (uint)instanceList.Count;
            instanceList.Add(inst);
            instanceDict.Add(codename, inst);

        }

        public virtual bool CanTake(NPC npc) //requirement check for FIRST taking (plants from the ground, fur from animals etc)
        {
            return true;
        }

        public virtual void Use(NPC npc)
        {
            //FIXME: Add boolean for usable
        }

        internal void Write(BitStream stream)
        {
            stream.mWrite(ID);

            if (Name != null && Name.Length > 0)
            {
                stream.Write1();
                stream.mWrite(Name);
            }
            else
            {
                stream.Write0();
            }

            if (ScemeName != null && ScemeName.Length > 0)
            {
                stream.Write1();
                stream.mWrite(ScemeName);
            }
            else
            {
                stream.Write0();
            }

            if (Range > 0)
            {
                stream.Write1();
                stream.mWrite(Range);
            }
            else
            {
                stream.Write0();
            }

            if (Weight > 0)
            {
                stream.Write1();
                stream.mWrite(Weight);
            }
            else
            {
                stream.Write0();
            }

            if (MainFlags != 0)
            {
                stream.Write1();
                stream.mWrite((int)MainFlags);
            }
            else
            {
                stream.Write0();
            }

            if (Flags != 0)
            {
                stream.Write1();
                stream.mWrite((int)Flags);
            }
            else
            {
                stream.Write0();
            }

            if (Wear != 0)
            {
                stream.Write1();
                stream.mWrite((int)Wear);
            }
            else
            {
                stream.Write0();
            }

            if (Materials > 0)
            {
                stream.Write1();
                stream.mWrite((byte)Materials);
            }
            else
            {
                stream.Write0();
            }

            if (Description != null && Description.Length > 0)
            {
                stream.Write1();
                stream.mWrite(Description);
            }
            else
            {
                stream.Write0();
            }

            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] != null && Text[i].Length > 0)
                {
                    stream.Write1();
                    stream.mWrite(Text[i]);
                }
                else
                {
                    stream.Write0();
                }
            }

            for (int i = 0; i < Count.Length; i++)
            {
                if (Count[i] > 0)
                {
                    stream.Write1();
                    stream.mWrite(Count[i]);
                }
                else
                {
                    stream.Write0();
                }
            }

            if (Visual != null && Visual.Length > 0)
            {
                stream.Write1();
                stream.mWrite(Visual);
            }
            else
            {
                stream.Write0();
            }

            if (Visual_Change != null && Visual_Change.Length > 0)
            {
                stream.Write1();
                stream.mWrite(Visual_Change);
            }
            else
            {
                stream.Write0();
            }

            if (Effect != null && Effect.Length > 0)
            {
                stream.Write1();
                stream.mWrite(Effect);
            }
            else
            {
                stream.Write0();
            }

            if (Visual_Skin > 0)
            {
                stream.Write1();
                stream.mWrite(Visual_Skin);
            }
            else
            {
                stream.Write0();
            }

            if (Munition != null)
            {
                stream.Write1();
                stream.mWrite(Munition.ID);
            }
            else
            {
                stream.Write0();
            }

            stream.mWrite(IsKeyInstance);
            stream.mWrite(IsTorch);
            stream.mWrite(IsTorchBurned);
            stream.mWrite(IsTorchBurning);
            stream.mWrite(IsGold);
        }
    }
}

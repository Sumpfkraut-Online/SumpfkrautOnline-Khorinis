using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Enumeration;
using RakNet;
using GUC.Network;
using GUC.Types;

namespace GUC.Client.WorldObjects
{
    class Item : Vob
    {
        protected ushort condition;
        public ushort Condition
        {
            get { return condition; }
            set 
            {
                if (condition != value)
                {
                    condition = value;
                    if (Spawned)
                    {
                        gItem.Name.Set(name);
                    }
                }
            }
        }

        protected ushort amount;
        public ushort Amount
        {
            get { return amount; }
            set
            {
                if (amount != value)
                {
                    amount = value;
                    if (Spawned)
                    {
                        gItem.Name.Set(name);
                    }
                }
            }
        }

        public String specialLine;

        public ItemInstance instance;
        public String name
        {
            get
            {
                if (amount > 1)
                {
                    return String.Format("{0} ({1})", GetAdjective(instance.name), amount);
                }
                else
                {
                    return GetAdjective(instance.name);
                }
            }
        }
        public String description
        {
            get
            {
                return GetAdjective(instance.description.Length > 0 ? instance.description : instance.name);
            }
        }

        public override string Visual { get { return instance.visual; } }
        public String visualChange { get { return instance.visualChange; } }
        public String effect { get { return instance.effect; } }

        String GetAdjective(string name)
        {
            if (instance.type <= ItemType.XBow) //weapon
            {
                return GetWeaponAdjective() + name;
            }
            else if (instance.type == ItemType.Armor) //armor
            {
                return GetArmorAdjective() + name;
            }
            //else if //werkzeuge! 
            return name;
        }
        String GetWeaponAdjective()
        {
            string adj = "";

            float percent = (float)Condition / (float)instance.condition;

            if (percent > 1.0f)
            {
                adj = "Geschärft";
            }
            else if (percent < 0.05f)
            {
                adj = "Kaputt";
            }
            else if (percent < 0.3f)
            {
                adj = "Rostig";
            }
            else if (percent < 0.6f)
            {
                adj = "Abgenutzt";
            }

            if (adj.Length > 0)
            {
                return adj + GetGenderEnding();
            }

            return adj;
        }
        String GetArmorAdjective()
        {
            string adj = "";

            float percent = (float)Condition / (float)instance.condition;

            if (percent > 1.0f)
            {
                adj = "Verstärkt";
            }
            else if (percent < 0.05f)
            {
                adj = "Kaputt";
            }
            else if (percent < 0.3f)
            {
                adj = "Alt";
            }
            else if (percent < 0.6f)
            {
                adj = "Abgetragen";
            }

            if (adj.Length > 0)
            {
                return adj + GetGenderEnding();
            }

            return adj;
        }

        String GetGenderEnding()
        {
            switch (gender)
            {
                case Gender.Masculine:
                    return "er ";
                case Gender.Feminine:
                    return "e ";
                case Gender.Neuter:
                    return "es ";
            }
            return null;
        }

        public ItemMaterial material { get { return instance.material; } }
        public String[] text { get { return instance.text; } }
        public ushort[] count { get { return instance.count; } }
        public oCItem.MainFlags mainFlags { get { return instance.mainFlags; } }
        public oCItem.ItemFlags flags { get { return instance.flags; } }
        public int wear { get { return instance.wear; } }
        public ushort munition { get { return instance.munition; } }
        public Gender gender { get { return instance.gender; } }
        public ushort weight { get { return instance.weight; } }

        public oCItem gItem
        {
            get
            {
                return new oCItem(Program.Process, gVob.Address);
            }
        }

        public Item(uint id, ushort instanceID)
            : base(id)
        {
            instance = ItemInstance.Table.Get(instanceID);
        }

        protected override void CreateVob(bool createNew)
        {
            
            if (createNew)
            {
                gVob = oCItem.Create(Program.Process);
            }
            gItem.Amount = 1;
            gItem.Instanz = instance.ID;
            gItem.Visual.Set(Visual);
            gItem.Name.Set(name);
        }
    }
}

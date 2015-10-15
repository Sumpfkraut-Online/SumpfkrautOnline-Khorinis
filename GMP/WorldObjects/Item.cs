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
        public ushort condition;
        public ushort amount;
        public String specialLine;
        public ushort weight;

        public ItemInstance instance;
        public String name
        {
            get
            {
                return String.Format("{0} ({1})", GetAdjective(instance.name), amount);
            }
        }
        public String description
        {
            get
            {
                return GetAdjective(instance.description);
            }
        }
        public String iVisual { get { return instance.visual; } }
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

            float percent = (float)condition / (float)instance.condition;

            if (percent > 1.0f)
            {
                adj = "geschärft";
            }
            else if (percent < 0.05f)
            {
                adj = "kaputt";
            }
            else if (percent < 0.3f)
            {
                adj = "rostig";
            }
            else if (percent < 0.6f)
            {
                adj = "abgenutzt";
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

            float percent = (float)condition / (float)instance.condition;

            if (percent > 1.0f)
            {
                adj = "verstärkt";
            }
            else if (percent < 0.05f)
            {
                adj = "kaputt";
            }
            else if (percent < 0.3f)
            {
                adj = "alt";
            }
            else if (percent < 0.6f)
            {
                adj = "abgetragen";
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
            instance = ItemInstance.Get(instanceID);
        }

        protected override void CreateVob(bool createNew)
        {
            if (createNew)
            {
                gVob = oCItem.Create(Program.Process);
            }
            visual = iVisual;
            //cdDyn = true;
            //cdStatic = true;
        }
    }
}

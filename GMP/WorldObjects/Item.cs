using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Enumeration;
using GUC.Client.WorldObjects.Instances;

namespace GUC.Client.WorldObjects
{
    class Item : Vob
    {
        public static readonly Item Fists = new Item();
        private Item() : base(0, 0, null)
        {
        }




        public byte Slot = 0;

        public ushort Amount = 1;

        new public oCItem gVob { get; protected set; }

        new public ItemInstance Instance { get; protected set; }

        public string Name { get { return Instance.Name; } }
        public ItemType Type { get { return Instance.Type; } }
        public ItemMaterial Material { get { return Instance.Material; } }
        public string[] Text { get { return Instance.Text; } }
        public ushort[] Count { get { return Instance.Count; } }
        public string Description { get { return Instance.Description; } }
        public string VisualChange { get { return Instance.VisualChange; } }
        public string Effect { get { return Instance.Effect; } }
        //public ItemInstance Munition { get { return Instance.Munition; } }

        public int MainFlags { get { return Instance.MainFlags; } }
        public int Flags { get { return Instance.Flags; } }
        public int Wear { get { return Instance.Wear; } }

        public bool IsMeleeWeapon { get { return Instance.IsMeleeWeapon; } }
        public bool IsRangedWeapon { get { return Instance.IsRangedWeapon; } }
        public bool IsArmor { get { return Instance.IsArmor; } }

        

        public Item(uint id, ushort instanceid)
            : this(id, instanceid, null)
        {
        }

        public Item(uint id, ushort instanceid, oCItem item) : base(id, instanceid, item)
        {
            if (this.gVob == null)
            {
                CreateVob();
                SetProperties();
            }
        }

        protected override void CreateVob()
        {
            base.gVob = oCItem.Create(Program.Process);

        }
        protected override void SetProperties()
        {
            base.SetProperties();

            oCItem gi = gVob;
            gi.Amount = 1;
            gi.Instanz = Instance.ID;
            gi.Visual.Set(Visual);
            gi.VisualChange.Set(VisualChange);
            gi.Name.Set(Name);
            gi.Material = (int)Material;
            gi.MainFlag = MainFlags;
            gi.Flags = Flags;
            gi.Wear = Wear;
        }
    }
}

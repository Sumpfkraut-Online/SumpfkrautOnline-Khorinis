using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Weapons.Melee
{
    public abstract class AbstractMelee : ItemInstance
    {
        protected AbstractMelee() : base()
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_NF;
            Flags = Enumeration.Flags.ITEM_AXE;
            Materials = Enumeration.MaterialType.MAT_METAL;
            Weight = 5;
        }

        public override void Use(NPC npc)
        {
            npc.Equip(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Armor
{
    public abstract class AbstractArmor : ItemInstance
    {
        protected AbstractArmor() : base()
        {
            Name = "Rüstung";
            MainFlags = Enumeration.MainFlags.ITEM_KAT_ARMOR;

            Materials = Enumeration.MaterialType.MAT_LEATHER;
            Wear = Enumeration.ArmorFlags.WEAR_TORSO;

            Weight = 10;
        }

        public override void Use(NPC npc)
        {
            npc.Equip(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Weapons.Range
{
    public abstract class AbstractRanged : ItemInstance
    {
        protected AbstractRanged() : base()
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_FF;
            Flags = Enumeration.Flags.ITEM_BOW;
            Materials = Enumeration.MaterialType.MAT_WOOD;
            DamageType = Enumeration.DamageTypes.DAM_POINT;
            Weight = 5;
        }

        public override void Use(NPC npc)
        {
            npc.Equip(this);
        }
    }
}

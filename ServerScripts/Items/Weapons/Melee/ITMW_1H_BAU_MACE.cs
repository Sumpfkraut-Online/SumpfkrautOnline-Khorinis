using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Items.Weapons.Melee
{
    public class ITMW_1H_BAU_MACE : AbstractMelee
    {
        public ITMW_1H_BAU_MACE() : base()
        {
            Flags = Enumeration.Flags.ITEM_AXE;

            Materials = Enumeration.MaterialType.MAT_WOOD;

            TotalDamage = 10;
            DamageType = Enumeration.DamageTypes.DAM_BLUNT;

            Range = 50;

            Name = "Schwerer Ast";
            Description = Name;

            Visual = "ItMw_010_1h_Club_01.3DS";

            Weight = 5;
        }
    }
}

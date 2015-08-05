using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Items.Weapons.Melee
{
    public class ITMW_SCHWERT4_03 : AbstractMelee
    {
        public ITMW_SCHWERT4_03()
            : base()
        {
            Flags = Enumeration.Flags.ITEM_SWD;

            Materials = Enumeration.MaterialType.MAT_METAL;

            TotalDamage = 60;
            DamageType = Enumeration.DamageTypes.DAM_EDGE;

            Range = 90;

            Name = "Edles Langschwert";
            Description = Name;

            Visual = "ItMw_045_1h_Sword_long_04.3DS";

            Weight = 15;
        }
    }
}

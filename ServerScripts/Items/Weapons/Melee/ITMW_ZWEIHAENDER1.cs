using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Items.Weapons.Melee
{
    public class ITMW_ZWEIHAENDER1 : AbstractMelee
    {
        public ITMW_ZWEIHAENDER1()
            : base()
        {
            Flags = Enumeration.Flags.ITEM_2HD_SWD;

            Materials = Enumeration.MaterialType.MAT_METAL;

            TotalDamage = 60;
            DamageType = Enumeration.DamageTypes.DAM_EDGE;

            Range = 110;

            Name = "Leichter Zweihänder";
            Description = Name;

            Visual = "ItMw_032_2h_sword_light_01.3DS";

            Weight = 20;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Weapons.Melee
{
    public class ITMW_1H_BAU_MACE : AbstractMelee
    {
        static ITMW_1H_BAU_MACE ii;
        public static ITMW_1H_BAU_MACE get()
        {
            if (ii == null)
                ii = new ITMW_1H_BAU_MACE();
            return ii;
        }


        protected ITMW_1H_BAU_MACE()
            : base("ITMW_1H_BAU_MACE")
        {
            MainFlags = Enumeration.MainFlags.ITEM_KAT_NF;
            Flags = Enumeration.Flags.ITEM_AXE;

            Materials = Enumeration.MaterialType.MAT_WOOD;

            TotalDamage = 50;
            DamageType = Enumeration.DamageTypes.DAM_BLUNT;

            Range = 50;

            Name = "Schwerer Ast";
            Description = Name;

            Visual = "ItMw_010_1h_Club_01.3DS";

            CreateItemInstance();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Items.Weapons.Range
{
    public class ITRW_BOW_M_03 : AbstractRanged
    {
        public ITRW_BOW_M_03()
            : base()
        {
            Flags = Enumeration.Flags.ITEM_BOW;

            Materials = Enumeration.MaterialType.MAT_WOOD;

            TotalDamage = 45;

            Name = "Langbogen";
            Description = Name;

            Visual = "ItRw_Bow_M_03.mms";

            Weight = 7;
        }
    }
}

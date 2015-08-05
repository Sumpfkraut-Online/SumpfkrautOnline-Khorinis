using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Items.Weapons.Range
{
    class ITRW_CROSSBOW_L_01 : AbstractRanged
    {
        public ITRW_CROSSBOW_L_01()
            : base()
        {
            Flags = Enumeration.Flags.ITEM_CROSSBOW;

            TotalDamage = 50;

            Name = "Jagdarmbrust";
            Description = Name;

            Visual = "ItRw_Crossbow_L_01.mms";

            Weight = 8;
        }
    }
}

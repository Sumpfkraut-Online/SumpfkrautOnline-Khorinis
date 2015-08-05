using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Items.Armor
{
    public class ITAR_BDT_M : AbstractArmor
    {
        public ITAR_BDT_M()
            : base()
        {
            Name = "Mittlere Banditenrüstung";

            Protection[0] = 35;
            Protection[1] = 35;
            Protection[2] = 35;

            Visual = "ItAr_Bdt_M.3ds";
            Visual_Change = "Armor_Bdt_M.asc";

            Description = Name;

            Text[0] = "Eine Banditenrüstung";

            Text[1] = "Schutz vor Ulumulu";
            Count[1] = 42;
        }
    }
}

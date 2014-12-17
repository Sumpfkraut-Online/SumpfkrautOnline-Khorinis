using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_BDT_M : AbstractArmors
    {
        static ITAR_BDT_M ii;
        public static ITAR_BDT_M get()
        {
            if (ii == null)
                ii = new ITAR_BDT_M();
            return ii;
        }


        protected ITAR_BDT_M()
            : base("ITAR_BDT_M")
        {
            Visual = "ItAr_Bdt_M.3ds";
            Visual_Change = "Armor_Bdt_M.asc";
            Name = "Mittlere Banditenrüstung";
            Description = Name;

            ProtectionEdge = 35;
            ProtectionBlunt = 35;
            ProtectionPoint = 35;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}

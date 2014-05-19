using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_LESTER : AbstractArmors
    {
        static ITAR_LESTER ii;
        public static ITAR_LESTER get()
        {
            if (ii == null)
                ii = new ITAR_LESTER();
            return ii;
        }


        protected ITAR_LESTER()
            : base("ITAR_LESTER")
        {
            Visual = "ItAr_Lester.3ds";
            Visual_Change = "Armor_Lester.asc";
            Name = "Lesters Robe";
            Description = Name;

            ProtectionEdge = 25;
            ProtectionBlunt = 25;
            ProtectionPoint = 25;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_PAL_M : AbstractArmors
    {
        static ITAR_PAL_M ii;
        public static ITAR_PAL_M get()
        {
            if (ii == null)
                ii = new ITAR_PAL_M();
            return ii;
        }


        protected ITAR_PAL_M()
            : base("ITAR_PAL_M")
        {
            Visual = "ItAr_Pal_M.3ds";
            Visual_Change = "Armor_Pal_M.asc";
            Name = "Ritterrüstung";
            Description = Name;

            ProtectionEdge = 100;
            ProtectionBlunt = 100;
            ProtectionPoint = 100;
            ProtectionFire = 50;
            ProtectionMagic = 25;

            CreateItemInstance();
        }
    }
}

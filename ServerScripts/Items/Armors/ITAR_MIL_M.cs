using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_MIL_M : AbstractArmors
    {
        static ITAR_MIL_M ii;
        public static ITAR_MIL_M get()
        {
            if (ii == null)
                ii = new ITAR_MIL_M();
            return ii;
        }


        protected ITAR_MIL_M()
            : base("ITAR_MIL_M")
        {
            Visual = "ItAr_MIL_M.3ds";
            Visual_Change = "Armor_MIL_M.asc";
            Name = "Schwere Milizrüstung";
            Description = Name;

            ProtectionEdge = 70;
            ProtectionBlunt = 70;
            ProtectionPoint = 70;
            ProtectionFire = 10;
            ProtectionMagic = 10;

            CreateItemInstance();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_DJG_M : AbstractArmors
    {
        static ITAR_DJG_M ii;
        public static ITAR_DJG_M get()
        {
            if (ii == null)
                ii = new ITAR_DJG_M();
            return ii;
        }


        protected ITAR_DJG_M()
            : base("ITAR_DJG_M")
        {
            Visual = "ItAr_Djg_M.3ds";
            Visual_Change = "Armor_Djg_M.asc";
            Name = "Mittlere Drachenjägerrüstung";
            Description = Name;

            ProtectionEdge = 120;
            ProtectionBlunt = 120;
            ProtectionPoint = 120;
            ProtectionFire = 75;
            ProtectionMagic = 35;

            CreateItemInstance();
        }
    }
}

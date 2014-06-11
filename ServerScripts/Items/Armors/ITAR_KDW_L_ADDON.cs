using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_KDW_L_ADDON : AbstractArmors
    {
        static ITAR_KDW_L_ADDON ii;
        public static ITAR_KDW_L_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_KDW_L_ADDON();
            return ii;
        }


        protected ITAR_KDW_L_ADDON()
            : base("ITAR_KDW_L_ADDON")
        {
            Visual = "ItAr_KDW_L_ADDON.3ds";
            Visual_Change = "Armor_KDW_L_ADDON.asc";
            Name = "Leichte KDW";
            Description = Name;

            ProtectionEdge = 50;
            ProtectionBlunt = 50;
            ProtectionPoint = 50;
            ProtectionFire = 25;
            ProtectionMagic = 25;

            CreateItemInstance();
        }
    }
}

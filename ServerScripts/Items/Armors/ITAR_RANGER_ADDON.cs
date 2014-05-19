using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_RANGER_ADDON : AbstractArmors
    {
        static ITAR_RANGER_ADDON ii;
        public static ITAR_RANGER_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_RANGER_ADDON();
            return ii;
        }


        protected ITAR_RANGER_ADDON()
            : base("ITAR_RANGER_ADDON")
        {
            Visual = "ItAr_Ranger_ADDON.3ds";
            Visual_Change = "Armor_Ranger_ADDON.asc";
            Name = "Rüstung des 'Rings des Wassers'";
            Description = Name;

            ProtectionEdge = 50;
            ProtectionBlunt = 50;
            ProtectionPoint = 50;
            ProtectionFire = 0;
            ProtectionMagic = 10;

            CreateItemInstance();
        }
    }
}

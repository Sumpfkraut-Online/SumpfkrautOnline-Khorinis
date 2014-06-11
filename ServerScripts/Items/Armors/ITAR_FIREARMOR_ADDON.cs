using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_FIREARMOR_ADDON : AbstractArmors
    {
        static ITAR_FIREARMOR_ADDON ii;
        public static ITAR_FIREARMOR_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_FIREARMOR_ADDON();
            return ii;
        }


        protected ITAR_FIREARMOR_ADDON()
            : base("ITAR_FIREARMOR_ADDON")
        {
            Visual = "ItAr_Xardas.3ds";
            Visual_Change = "Armor_Xardas.asc";
            Name = "Magische Rüstung";
            Description = Name;

            ProtectionEdge = 100;
            ProtectionBlunt = 100;
            ProtectionPoint = 100;
            ProtectionFire = 50;
            ProtectionMagic = 50;

            Wear = Enumeration.ArmorFlags.WEAR_TORSO | Enumeration.ArmorFlags.WEAR_EFFECT;
            Effect = "SPELLFX_FIREARMOR";

            CreateItemInstance();
        }
    }
}

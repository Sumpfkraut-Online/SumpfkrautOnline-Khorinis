using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_BLOODWYN_ADDON : AbstractArmors
    {
        static ITAR_BLOODWYN_ADDON ii;
        public static ITAR_BLOODWYN_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_BLOODWYN_ADDON();
            return ii;
        }


        protected ITAR_BLOODWYN_ADDON()
            : base("ITAR_BLOODWYN_ADDON")
        {
            Visual = "ItAr_Bloodwyn_ADDON.3ds";
            Visual_Change = "Armor_Bloodwyn_ADDON.asc";
            Name = "Bloodwyn Rüstung";
            Description = Name;

            ProtectionEdge = 70;
            ProtectionBlunt = 70;
            ProtectionPoint = 70;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}

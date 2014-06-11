using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_RAVEN_ADDON : AbstractArmors
    {
        static ITAR_RAVEN_ADDON ii;
        public static ITAR_RAVEN_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_RAVEN_ADDON();
            return ii;
        }


        protected ITAR_RAVEN_ADDON()
            : base("ITAR_RAVEN_ADDON")
        {
            Visual = "ItAr_Raven_ADDON.3ds";
            Visual_Change = "Armor_Raven_ADDON.asc";
            Name = "Ravens Rüstung";
            Description = Name;

            ProtectionEdge = 100;
            ProtectionBlunt = 100;
            ProtectionPoint = 100;
            ProtectionFire = 100;
            ProtectionMagic = 100;

            CreateItemInstance();
        }
    }
}

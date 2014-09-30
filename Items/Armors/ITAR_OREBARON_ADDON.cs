using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_OREBARON_ADDON : AbstractArmors
    {
        static ITAR_OREBARON_ADDON ii;
        public static ITAR_OREBARON_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_OREBARON_ADDON();
            return ii;
        }


        protected ITAR_OREBARON_ADDON()
            : base("ITAR_OREBARON_ADDON")
        {
            Visual = "ItAr_CHAOS_ADDON.3ds";
            Visual_Change = "Armor_CHAOS_ADDON.asc";
            Name = "Erzbaronrüstung";
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

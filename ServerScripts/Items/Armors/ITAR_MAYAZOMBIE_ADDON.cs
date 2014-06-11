using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_MAYAZOMBIE_ADDON : AbstractArmors
    {
        static ITAR_MAYAZOMBIE_ADDON ii;
        public static ITAR_MAYAZOMBIE_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_MAYAZOMBIE_ADDON();
            return ii;
        }


        protected ITAR_MAYAZOMBIE_ADDON()
            : base("ITAR_MAYAZOMBIE_ADDON")
        {
            Visual = "ItAr_Raven_ADDON.3ds";
            Visual_Change = "Armor_MayaZombie_Addon.asc";
            Name = "alte Rüstung";
            Description = Name;

            ProtectionEdge = 50;
            ProtectionBlunt = 50;
            ProtectionPoint = 50;
            ProtectionFire = 50;
            ProtectionMagic = 50;

            CreateItemInstance();
        }
    }
}

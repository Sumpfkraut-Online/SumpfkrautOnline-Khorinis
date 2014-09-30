using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_PIR_M_ADDON : AbstractArmors
    {
        static ITAR_PIR_M_ADDON ii;
        public static ITAR_PIR_M_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_PIR_M_ADDON();
            return ii;
        }


        protected ITAR_PIR_M_ADDON()
            : base("ITAR_PIR_M_ADDON")
        {
            Visual = "ItAr_PIR_M_ADDON.3ds";
            Visual_Change = "Armor_PIR_M_ADDON.asc";
            Name = "Piratenrüstung";
            Description = Name;

            ProtectionEdge = 55;
            ProtectionBlunt = 55;
            ProtectionPoint = 55;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}

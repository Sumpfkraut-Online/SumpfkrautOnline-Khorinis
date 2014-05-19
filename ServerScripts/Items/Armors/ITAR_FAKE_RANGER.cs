using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_FAKE_RANGER : AbstractArmors
    {
        static ITAR_FAKE_RANGER ii;
        public static ITAR_FAKE_RANGER get()
        {
            if (ii == null)
                ii = new ITAR_FAKE_RANGER();
            return ii;
        }


        protected ITAR_FAKE_RANGER()
            : base("ITAR_FAKE_RANGER")
        {
            Visual = "ItAr_Ranger_ADDON.3ds";
            Visual_Change = "Armor_Ranger_ADDON.asc";
            Name = "zerissene Rüstung";
            Description = Name;

            ProtectionEdge = 0;
            ProtectionBlunt = 0;
            ProtectionPoint = 0;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_DEMENTOR : AbstractArmors
    {
        static ITAR_DEMENTOR ii;
        public static ITAR_DEMENTOR get()
        {
            if (ii == null)
                ii = new ITAR_DEMENTOR();
            return ii;
        }


        protected ITAR_DEMENTOR()
            : base("ITAR_DEMENTOR")
        {
            Visual = "ItAr_Xardas.3ds";
            Visual_Change = "Armor_Dementor.asc";
            Name = "Dunkler Umhang";
            Description = Name;

            ProtectionEdge = 130;
            ProtectionBlunt = 130;
            ProtectionPoint = 130;
            ProtectionFire = 65;
            ProtectionMagic = 65;

            CreateItemInstance();
        }
    }
}

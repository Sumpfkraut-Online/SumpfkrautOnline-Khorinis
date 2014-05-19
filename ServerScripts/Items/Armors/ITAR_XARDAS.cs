using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_XARDAS : AbstractArmors
    {
        static ITAR_XARDAS ii;
        public static ITAR_XARDAS get()
        {
            if (ii == null)
                ii = new ITAR_XARDAS();
            return ii;
        }


        protected ITAR_XARDAS()
            : base("ITAR_XARDAS")
        {
            Visual = "ItAr_Xardas.3ds";
            Visual_Change = "Armor_Xardas.asc";
            Name = "Robe der Dunklen Künste";
            Description = Name;

            ProtectionEdge = 100;
            ProtectionBlunt = 100;
            ProtectionPoint = 100;
            ProtectionFire = 50;
            ProtectionMagic = 50;

            CreateItemInstance();
        }
    }
}

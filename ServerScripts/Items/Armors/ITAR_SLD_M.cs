using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_SLD_M : AbstractArmors
    {
        static ITAR_SLD_M ii;
        public static ITAR_SLD_M get()
        {
            if (ii == null)
                ii = new ITAR_SLD_M();
            return ii;
        }


        protected ITAR_SLD_M()
            : base("ITAR_SLD_M")
        {
            Visual = "ItAr_Sld_M.3ds";
            Visual_Change = "Armor_Sld_M.asc";
            Name = "Mittlere Söldnerrüstung";
            Description = Name;

            ProtectionEdge = 50;
            ProtectionBlunt = 50;
            ProtectionPoint = 50;
            ProtectionFire = 0;
            ProtectionMagic = 5;

            CreateItemInstance();
        }
    }
}

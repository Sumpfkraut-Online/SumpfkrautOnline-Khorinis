using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_SLD_H : AbstractArmors
    {
        static ITAR_SLD_H ii;
        public static ITAR_SLD_H get()
        {
            if (ii == null)
                ii = new ITAR_SLD_H();
            return ii;
        }


        protected ITAR_SLD_H()
            : base("ITAR_SLD_H")
        {
            Visual = "ItAr_Sld_H.3ds";
            Visual_Change = "Armor_Sld_H.asc";
            Name = "Schwere Söldnerrüstung";
            Description = Name;

            ProtectionEdge = 80;
            ProtectionBlunt = 80;
            ProtectionPoint = 80;
            ProtectionFire = 5;
            ProtectionMagic = 10;

            CreateItemInstance();
        }
    }
}

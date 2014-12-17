using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_KDW_H : AbstractArmors
    {
        static ITAR_KDW_H ii;
        public static ITAR_KDW_H get()
        {
            if (ii == null)
                ii = new ITAR_KDW_H();
            return ii;
        }


        protected ITAR_KDW_H()
            : base("ITAR_KDW_H")
        {
            Visual = "ItAr_KdW_H.3ds";
            Visual_Change = "Armor_KdW_H.asc";
            Name = "Wassermagierrobe";
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

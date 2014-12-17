using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_KDF_H : AbstractArmors
    {
        static ITAR_KDF_H ii;
        public static ITAR_KDF_H get()
        {
            if (ii == null)
                ii = new ITAR_KDF_H();
            return ii;
        }


        protected ITAR_KDF_H()
            : base("ITAR_KDF_H")
        {
            Visual = "ItAr_KdF_H.3ds";
            Visual_Change = "Armor_Kdf_H.asc";
            Name = "Schwere Feuerrobe";
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

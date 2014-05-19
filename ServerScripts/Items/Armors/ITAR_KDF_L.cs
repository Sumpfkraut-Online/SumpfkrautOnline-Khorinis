using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_KDF_L : AbstractArmors
    {
        static ITAR_KDF_L ii;
        public static ITAR_KDF_L get()
        {
            if (ii == null)
                ii = new ITAR_KDF_L();
            return ii;
        }


        protected ITAR_KDF_L()
            : base("ITAR_KDF_L")
        {
            Visual = "ItAr_KdF_L.3ds";
            Visual_Change = "Armor_Kdf_L.asc";
            Name = "Feuermagierrobe";
            Description = Name;

            ProtectionEdge = 40;
            ProtectionBlunt = 40;
            ProtectionPoint = 40;
            ProtectionFire = 20;
            ProtectionMagic = 20;

            CreateItemInstance();
        }
    }
}

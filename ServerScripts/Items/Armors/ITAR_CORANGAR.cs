using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_CORANGAR : AbstractArmors
    {
        static ITAR_CORANGAR ii;
        public static ITAR_CORANGAR get()
        {
            if (ii == null)
                ii = new ITAR_CORANGAR();
            return ii;
        }


        protected ITAR_CORANGAR()
            : base("ITAR_CORANGAR")
        {
            Visual = "ItAr_CorAngar.3ds";
            Visual_Change = "Armor_CorAngar.asc";
            Name = "Cor Angars Rüstung";
            Description = Name;

            ProtectionEdge = 100;
            ProtectionBlunt = 100;
            ProtectionPoint = 100;
            ProtectionFire = 50;
            ProtectionMagic = 25;

            CreateItemInstance();
        }
    }
}

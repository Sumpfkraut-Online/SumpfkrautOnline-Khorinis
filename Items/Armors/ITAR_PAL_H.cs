using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_PAL_H : AbstractArmors
    {
        static ITAR_PAL_H ii;
        public static ITAR_PAL_H get()
        {
            if (ii == null)
                ii = new ITAR_PAL_H();
            return ii;
        }


        protected ITAR_PAL_H()
            : base("ITAR_PAL_H")
        {
            Visual = "ItAr_Pal_H.3ds";
            Visual_Change = "Armor_Pal_H.asc";
            Name = "Paladinrüstung";
            Description = Name;

            ProtectionEdge = 150;
            ProtectionBlunt = 150;
            ProtectionPoint = 150;
            ProtectionFire = 100;
            ProtectionMagic = 50;

            CreateItemInstance();
        }
    }
}

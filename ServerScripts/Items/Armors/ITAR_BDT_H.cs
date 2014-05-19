using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_BDT_H : AbstractArmors
    {
        static ITAR_BDT_H ii;
        public static ITAR_BDT_H get()
        {
            if (ii == null)
                ii = new ITAR_BDT_H();
            return ii;
        }


        protected ITAR_BDT_H()
            : base("ITAR_BDT_H")
        {
            Visual = "ItAr_Bdt_H.3ds";
            Visual_Change = "Armor_Bdt_H.asc";
            Name = "Schwere Banditenrüstung";
            Description = Name;

            ProtectionEdge = 50;
            ProtectionBlunt = 50;
            ProtectionPoint = 50;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}

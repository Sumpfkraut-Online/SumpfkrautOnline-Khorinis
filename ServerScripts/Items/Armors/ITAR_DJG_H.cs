using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_DJG_H : AbstractArmors
    {
        static ITAR_DJG_H ii;
        public static ITAR_DJG_H get()
        {
            if (ii == null)
                ii = new ITAR_DJG_H();
            return ii;
        }


        protected ITAR_DJG_H()
            : base("ITAR_DJG_H")
        {
            Visual = "ItAr_Djg_H.3ds";
            Visual_Change = "Armor_Djg_H.asc";
            Name = "Schwere Drachenjägerrüstung";
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

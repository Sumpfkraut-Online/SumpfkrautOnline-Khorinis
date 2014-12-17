using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_NOV_L : AbstractArmors
    {
        static ITAR_NOV_L ii;
        public static ITAR_NOV_L get()
        {
            if (ii == null)
                ii = new ITAR_NOV_L();
            return ii;
        }


        protected ITAR_NOV_L()
            : base("ITAR_NOV_L")
        {
            Visual = "ItAr_Nov_L.3ds";
            Visual_Change = "Armor_Nov_L.asc";
            Name = "Novizenrobe";
            Description = Name;

            ProtectionEdge = 25;
            ProtectionBlunt = 25;
            ProtectionPoint = 25;
            ProtectionFire = 0;
            ProtectionMagic = 10;

            CreateItemInstance();
        }
    }
}

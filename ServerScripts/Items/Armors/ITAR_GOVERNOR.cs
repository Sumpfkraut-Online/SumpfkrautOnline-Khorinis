using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_GOVERNOR : AbstractArmors
    {
        static ITAR_GOVERNOR ii;
        public static ITAR_GOVERNOR get()
        {
            if (ii == null)
                ii = new ITAR_GOVERNOR();
            return ii;
        }


        protected ITAR_GOVERNOR()
            : base("ITAR_GOVERNOR")
        {
            Visual = "ItAr_Governor.3ds";
            Visual_Change = "Armor_Governor.asc";
            Name = "Wams des Statthalters";
            Description = Name;

            ProtectionEdge = 40;
            ProtectionBlunt = 40;
            ProtectionPoint = 40;


            CreateItemInstance();
        }
    }
}

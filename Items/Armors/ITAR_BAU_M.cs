using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_BAU_M : AbstractArmors
    {
        static ITAR_BAU_M ii;
        public static ITAR_BAU_M get()
        {
            if (ii == null)
                ii = new ITAR_BAU_M();
            return ii;
        }


        protected ITAR_BAU_M()
            : base("ITAR_BAU_M")
        {
            Visual = "ItAr_Bau_M.3ds";
            Visual_Change = "Armor_Bau_M.asc";
            Name = "Bauernkleidung";
            Description = Name;

            ProtectionEdge = 15;
            ProtectionBlunt = 15;
            ProtectionPoint = 15;


            CreateItemInstance();
        }
    }
}
